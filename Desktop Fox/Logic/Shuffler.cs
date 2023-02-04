using DesktopFox;
using DesktopFox.MVVM.Model;
using DesktopFox.MVVM.ViewModels;
using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Timers;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using Timer = System.Timers.Timer;

namespace DesktopFox
{
    public class Shuffler
    {
        private readonly MainWindowVM MWVM;
        private readonly PreviewVM previewVM;
        private readonly VirtualDesktop vDesk;
        private readonly GalleryManager GM;
        private readonly SettingsManager SM;
        public Boolean isDay;

        private readonly LockListQueue[] lockListQueues = new LockListQueue[3];

        private int previewCount = 0;
        private Timer previewTimer;
        private Timer desktopShuffleTimer;
        private Timer daytimeTimer;
        private readonly TimeSpan PreviewShuffleTime = TimeSpan.FromSeconds(10);
        
        private readonly Random random = new Random();

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="mainWindowVM"></param>
        /// <param name="galleryManager"></param>
        /// <param name="settingsManager"></param>
        /// <param name="previewVM"></param>
        /// <param name="virtualDesktop"></param>
        public Shuffler(MainWindowVM mainWindowVM, GalleryManager galleryManager, SettingsManager settingsManager, PreviewVM previewVM, VirtualDesktop virtualDesktop)
        {
            MWVM = mainWindowVM;
            GM = galleryManager;
            SM = settingsManager;
            this.previewVM = previewVM;
            vDesk = virtualDesktop;
            SM.Settings.PropertyChanged += Shuffler_Settings_PropertyChanged;
            mainWindowVM.PropertyChanged += MainWindowVM_PropertyChanged;
            Task.Run(() => DaytimeTimerStart());
            if (SM.Settings.IsRunning) PicShuffleStart();
        }

        /// <summary>
        /// Wird benötigt um auf das ändern des aktuell ausgewählten Sets zu reagieren 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindowVM_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(MWVM.SelectedItem)) return;
        }

        /// <summary>
        /// Listener für Änderungen in den Settings auf die der Shuffler reagieren muss.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Shuffler_Settings_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(SM.Settings.IsRunning):
                    Debug.WriteLine("IsRunning Property Changed Raised: " + SM.Settings.IsRunning);
                    if (SM.Settings.IsRunning)
                        PicShuffleStart();                       
                    else
                        PicShuffleStop();

                    break;
                    
                case nameof(SM.Settings.DesktopModeSingle):
                    if (SM.Settings.IsRunning)
                        PicShuffleStart();
                    break;

                //Reagiert auf Night da Beide angestoßen werden und Night der Letzte Wert ist der geändert wird.
                case nameof(SM.Settings.NightStart):
                    DaytimeTimerStart();
                    break;

                default: return;
            }
        }


        #region Preview Steuerelemente

        /// <summary>
        /// Zeigt im Preview Fenster das nächste Bild an
        /// </summary>
        public void previewForward()
        {
            if (previewVM.PreviewModel.FaderLock == false && GM.getPreviewSet() != null)
            {
                PreviewTimerReset();
                PreviewShuffleAsync();
            }
        }

        /// <summary>
        /// Aktualisiert das Bilder im Preview Fenster
        /// </summary>
        public void previewRefresh()
        {
            PreviewTimerReset();
            if(previewCount > 0)
                previewCount--;

            PreviewShuffleAsync();
        }

        /// <summary>
        /// Zeigt im Preview Fenster das vorherige Bild an
        /// </summary>
        public void previewBackward()
        {
            if (previewVM.PreviewModel.FaderLock == false && GM.getPreviewSet() != null)
            {
                PreviewTimerReset();
                previewCount = previewCount - 2;
                if (previewCount < 0)
                {
                    previewCount = GM.GetCollection(previewVM.PreviewModel.Day, previewVM.PreviewModel.SetName).singlePics.Count - 1;
                }
                PreviewShuffleAsync();
            }
        }

        #endregion

        #region Shuffler & Logik

        /// <summary>
        /// Shuffle Funktion für den Windows eigenen Shuffel Modus. 
        /// </summary>
        /// <param name="path"></param>
        public void WinPicShuffle(String path)
        {
            /* Shuffle Modus vom Wrapper. Shufflet
             * ACHTUNG Wechsel Passiert nur auf einem Monitor mit zwei monitoren verdoppelt sich die Zeit pro Monitor
             * Ist die Beste Methode um Bilder aus einer Collection zu Shufflen
             * Keine Bilder aus unterschiedlichen Ordnern zur selben zeit
             * Bilder die auf beide Monitore gestretched werden können, werden automatisch gespliced wenn Fill mode an ist
            vDesk.getWrapper.SetSlideshow("F:\\Test");                  //Ordner aus dem die Bilder Genommen werden sollen. bild wird aus dem live ordner genommen
            vDesk.getWrapper.SetSlideshowTransitionInterval(10000);     //Wechselinverval
            vDesk.getWrapper.AdvanceSlideshow();                        //Wechselt das Bild direkt für den nächsten Monitor der drann wäre
            vDesk.getWrapper.SetSlideshowShuffle(false);                //Ob der Shuffle Modus aktiviert ist
            */
            if (SM.Settings.Shuffle)
                vDesk.getWrapper.SetSlideshowShuffle(true);
            else
                vDesk.getWrapper.SetSlideshowShuffle(false);

            Debug.WriteLine("Winpic Shuffle Mode Setting: " + SM.Settings.Shuffle);
            Debug.WriteLine("Winpic Shuffle Desktop Mode: " + vDesk.getWrapper.GetSlideshowStatus());

            //Überprüft ob sich die Einstellung geändert hat und passt diese ggf. an
            if (vDesk.getWrapper.GetPosition() != SM.getDesktopFillMode())
                vDesk.getWrapper.SetPosition(SM.getDesktopFillMode());

            vDesk.getWrapper.SetSlideshow(path);
            vDesk.getWrapper.SetSlideshowTransitionInterval((uint)SM.Settings.ShuffleTime.TotalMilliseconds);
        }

        /// <summary>
        /// Startet anhand der Einstellungen in <see cref="Settings.DesktopModeSingle"/> die jeweiligen Manager.
        /// </summary>
        public void PicShuffleStart()
        {  
            if (SM.Settings.DesktopModeSingle && GM.getActiveSet() == null) return;
            if (GM.getActiveSet(any: true) == null) return;

            Debug.WriteLine("Start des Wechsels von Windows und DF Shuffle. Einzelmodus = " + SM.Settings.DesktopModeSingle);

            IsDayCheck();

            for (int i = 0; i < GM.Gallery.activeSetsList.Count; i++)
            {
                if (GM.Gallery.activeSetsList[i] == "Empty") continue;   
                lockListQueues[i] = new LockListQueue(GM.GetCollection(isDay, GM.Gallery.activeSetsList[i]).singlePics.Count);
            }

            if (SM.Settings.DesktopModeSingle)
            {
                StopDesktopTimer();
                WinPicShuffle(GM.GetCollection(isDay, GM.getActiveSet().SetName).folderDirectory);
            }
            else
            {
                StartDesktopTimer();
                StopWinDesktop();
            }

        }

        /// <summary>
        /// Stoppt den Shuffle für den Desktophintergrund
        /// </summary>
        public void PicShuffleStop()
        {
            if (desktopShuffleTimer != null)
            {
                desktopShuffleTimer.Stop();
                //desktopShuffleTimer.Dispose();
            }
            StopWinDesktop();
        }

        /// <summary>
        /// Eigener Shuffler. Shuffelt/Rotiert die Bilder der Collection und setzt sie als Hintergundbild des Monitors. 
        /// </summary>
        /// <param name="monitorID"></param>
        /// <param name="activeCol"></param>
        private void DF_PicShuffle(Monitor monitor, Collection activeCol)
        {
            LockListQueue queue = lockListQueues.ElementAt((int)monitor.Name - 1);

            if (queue.PictureCount != activeCol.singlePics.Count)
                queue.ResetPictureCount(activeCol.singlePics.Count);


            if (SM.Settings.Shuffle)
                vDesk.getWrapper.SetWallpaper(monitor.ID, activeCol.singlePics.ElementAt(queue.GetNewRandomItem()).Key);
            else
                vDesk.getWrapper.SetWallpaper(monitor.ID, activeCol.singlePics.ElementAt(queue.GetNextItem()).Key);


            /* Note: Absolut unnötiger aufwand. Entfernen falls nichts mehr aufkommt

            int[] tmpLockList;
            switch (monitor.Number)
            {
                case 1: tmpCount = picCount1; tmpLockList = lockList1; break;
                case 2: tmpCount = picCount2; tmpLockList = lockList2; break;
                case 3: tmpCount = picCount3; tmpLockList = lockList3; break;
                default: tmpCount = picCount1; tmpLockList = lockList1; break;
            }

            //Überprüft ob sich die Einstellung geändert hat und passt diese ggf. an
            if (vDesk.getWrapper.GetPosition() != SM.getDesktopFillMode())
                vDesk.getWrapper.SetPosition(SM.getDesktopFillMode());

            //Sollte die Collection weniger als 4 Bilder besitzten, macht es keinen Sinn den Aufwendigeren Shuffler zu benutzen.
            if (SM.Settings.Shuffle && activeCol.singlePics.Count > 4)
            {
                //Shuffle Funktion für die Anzeige

                //"Shiften" des Arrays um eine Position um platz für den Neuen Eintrag zu machen
                var tmpArray = tmpLockList;
                Array.Copy(tmpArray, 0, tmpLockList, 1, tmpArray.Length - 1);
                tmpLockList[0] = tmpCount;

                //Ermitteln eines neuen Random Wertes der noch nicht in den Letzten 3 Bildern vorgekommen ist.
                //Versuche sind hierbei auf 50 begrenzt um das System nicht zu sehr zu belasten oder einen Deadlock zu verursachen

                Boolean match = false;
                for (int i = 1; i < 100; i++)
                {
                    tmpCount = random.Next(activeCol.singlePics.Count);
                    foreach (var item in tmpLockList)
                    {
                        if (item == tmpCount)
                        {
                            Debug.WriteLine("Match für Items: " + item + " = " + tmpCount);
                            match = true;
                            break;
                        }
                    }

                    if (match == false)
                        break;
                }
                vDesk.getWrapper.SetWallpaper(monitor.ID, activeCol.singlePics.ElementAt(tmpCount).Key);
            }
            else
            {
                //Lineare Anzeige aller Bilder im Set
                if (tmpCount < activeCol.singlePics.Count - 1)
                    tmpCount++;
                else
                    tmpCount = 0;

                vDesk.getWrapper.SetWallpaper(monitor.ID, activeCol.singlePics.ElementAt(tmpCount).Key);
            }

            Debug.Write("Ändern des Hintergrundes. Monitor: " + monitor.Number + ", Zahl: " + tmpCount + ", LockListe: ");
            foreach (var item in tmpLockList)
            {
                Debug.Write(item + ", ");
            }
            Debug.WriteLine("");
            //Note: Wofür... Bestimmt wichtig :)... Rückgabe des Counters 
            switch (monitor.Number)
            {
                case 1: picCount1 = tmpCount; break;
                case 2: picCount2 = tmpCount; break;
                case 3: picCount3 = tmpCount; break;
            }
            */

        }

        /// <summary>
        /// Funktion die das Große Preview Bild shuffelt
        /// </summary>
        public async Task PreviewShuffleAsync()
        {
            //Debug.WriteLine("Preview Shuffle ausgelöst");
            var tmpPreviewSet = GM.getPreviewSet();
            PreviewModel tmpPreviewModel = previewVM.PreviewModel;

            if (tmpPreviewSet == null)
            {
                //tmpPreviewModel.ForegroundImage = await Task.Run(() => ImageHandler.dummy());
            }
            else
            {
                if (previewCount >= 0 && previewCount < GM.GetCollection(tmpPreviewModel.Day, tmpPreviewSet.SetName).singlePics.Count)
                {
                    tmpPreviewModel.BackgroundImage = await Task.Run(() => ImageHandler.load(GM.GetCollection(tmpPreviewModel.Day, tmpPreviewSet.SetName).singlePics.ElementAt(previewCount).Key));
                    tmpPreviewModel.PictureCountCurrent = previewCount + 1;
                    tmpPreviewModel.SetName = tmpPreviewSet.SetName;
                    tmpPreviewModel.PictureCountMax = GM.GetCollection(tmpPreviewModel.Day, tmpPreviewSet.SetName).singlePics.Count;
                    previewCount++;
                }
                else
                {
                    //Anzeigen des Bildes an Erster Stelle und setzten des Counters um einene gleichmäßige Rotation zu ermöglichen
                    
                    tmpPreviewModel.BackgroundImage = await Task.Run(() => ImageHandler.load(GM.GetCollection(tmpPreviewModel.Day, tmpPreviewSet.SetName).singlePics.ElementAt(0).Key));                    
                    tmpPreviewModel.PictureCountCurrent = 1;
                    tmpPreviewModel.SetName = tmpPreviewSet.SetName;
                    tmpPreviewModel.PictureCountMax = GM.GetCollection(tmpPreviewModel.Day, tmpPreviewSet.SetName).singlePics.Count;
                    previewCount = 1;
                }
            }
            previewVM.PreviewModel.FaderLock = true;
        }

        /// <summary>
        /// Überprüft anhand der Systemzeit die Tageszeit und setzt das lokale Flag (<see cref="isDay"/>) dafür
        /// </summary>
        private void IsDayCheck()
        {
            TimeSpan dayStart = SM.Settings.DayStart;
            TimeSpan nightStart = SM.Settings.NightStart;
            DateTime timeNow = System.DateTime.Now;
            TimeSpan currentTime = timeNow.TimeOfDay;

            //Unterscheidung zwischen der Anordnung des Tagesstarts und Festlegen des Default falls die Tag/Nacht Zeit gleich ist.
            if (dayStart < nightStart)
            {
                if (currentTime > dayStart && currentTime < nightStart)
                    isDay = true;
                else
                    isDay = false;
            }
            else if (dayStart > nightStart)
            {
                if (currentTime > dayStart || currentTime < nightStart)
                    isDay = true;
                else
                    isDay = false;
            }
            else
            {
                isDay = true;
            }


            /*
            if (SM.Settings.IsRunning)
                Task.Run(() => PicShuffleStart());
            */

            Debug.WriteLine("Tageszeit wurde überprüft. Aktuell ist isDay: " + isDay);
        }

        #endregion

        #region Timer & Trigger

        #region Desktop

        /// <summary>
        /// Startet den Timer für den Desktop Shuffler
        /// </summary>
        public void StartDesktopTimer()
        {
            //Timer der nur für die eigene Shuffle Funktion benötigt wird. Wird ausschließlich für den Multimode verwendet.
            if (desktopShuffleTimer == null)
            {
                desktopShuffleTimer = new Timer(SM.Settings.ShuffleTime.TotalMilliseconds); //Timer läuft in MS Umwandlung ("Wert vom Benutzer" * Minute * Sekunde)
                desktopShuffleTimer.Elapsed += DesktopTimer_Trigger;
                desktopShuffleTimer.Enabled = true;
                Debug.WriteLine("DF Desktop Timer hat gestartet");
            }
            else
            {
                desktopShuffleTimer.Stop();
                desktopShuffleTimer.Interval = SM.Settings.ShuffleTime.TotalMilliseconds;
                desktopShuffleTimer.Start();
                Debug.WriteLine("DF Timer läuft bereits. Zeit wurde zurückgesetzt");
            }
            DesktopTimer_Kickstart();
            //DesktopTimer_Trigger(null, null);
            if (!SM.Settings.IsRunning)
                SM.Settings.IsRunning = true;
                
        }

        /// <summary>
        /// Stoppt den Desktop Timer
        /// </summary>
        public void StopDesktopTimer()
        {
            if (desktopShuffleTimer != null)
            {
                //desktopShuffleTimer.Enabled = false;
                desktopShuffleTimer.Stop();
                Debug.WriteLine("DF Shuffle Timer hat gestoppt");
            }
        }

        /// <summary>
        /// Stoppt den Windows Shuffler
        /// </summary>
        public void StopWinDesktop(Boolean Shutdown = false)
        {
            vDesk.getWrapper.SetSlideshowShuffle(false);
            var monitors = vDesk.getMonitors;
            foreach (var i in monitors)
            {
                vDesk.getWrapper.SetWallpaper(i.Value.ID, vDesk.getWrapper.GetWallpaper(i.Value.ID));
            }
            //Im falles letzten Aufrufs, diese Zusweisung nicht machen um den State von dem Programm abzuspeichern.

            Debug.WriteLine("Die Windows Desktop Shuffle Funktion ist deaktiviert worden");
        }

        /// <summary>
        /// Trigger Event des Desktop Timers. Überprüft die Anzahl der Monitore und gibt die Informationen an den Shuffler weiter.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DesktopTimer_Trigger(object sender, System.Timers.ElapsedEventArgs e)
        {
            for(int i = 0; i < GM.Gallery.activeSetsList.Count; i++)
            {
                if (GM.Gallery.activeSetsList[i] == "Empty") continue;
                DF_PicShuffle(vDesk.GetMonitor(i+1), GM.GetCollection(isDay, GM.Gallery.activeSetsList[i]));
            }
        }

        /// <summary>
        /// Wird für den einmaligen Anstoß von <see cref="DF_PicShuffle(Monitor, Collection)"/> benötigt, 
        /// wenn sich Einstellungen geändert haben und der <see cref="desktopShuffleTimer"/> neu gestartet
        /// oder Initialisiert wurde.
        /// </summary>
        private void DesktopTimer_Kickstart()
        {
            for (int i = 0; i < GM.Gallery.activeSetsList.Count; i++)
            {
                if (GM.Gallery.activeSetsList[i] == "Empty") continue;
                if (GM.LastMonitorChanged() != MonitorEnum.None && (int)GM.LastMonitorChanged() - 1 != i) continue;
                DF_PicShuffle(vDesk.GetMonitor(i + 1), GM.GetCollection(isDay, GM.Gallery.activeSetsList[i]));
            }
            GM.LastMonitorChanged(reset: true);
        }

        #endregion

        #region Preview

        /// <summary>
        /// Start des Preview Timers
        /// </summary>
        public void StartPreviewShuffleTimer()
        {
            if (previewTimer == null)
            {
                previewTimer = new Timer();
                //timer.Tick += new EventHandler(previewShuffle);
                previewTimer.Interval = PreviewShuffleTime.TotalMilliseconds;
                previewTimer.Elapsed += new ElapsedEventHandler(Dispatcher);
                previewTimer.Start();
                Debug.WriteLine("Timer gestartet");
            }
            else
            {
                previewTimer.Stop();
                previewTimer.Start();
            }
            //Einmaliger Aufruf um Bild in der Vorschau zu aktualisieren
            Dispatcher(null, null);
        }

        /// <summary>
        /// Stoppt den Preview Shuffle Timer
        /// </summary>
        public void StopPreviewShuffleTimer()
        {
            if (previewTimer != null)
                previewTimer.Stop();
        }

        ///<summary>
        /// Auslagerungs Methode um die Kollision mit dem UI Thread zu verhindern.
        /// Das Ändern der Bilder wird von diesem Thread an den UI Thread weitergegeben.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Dispatcher(object sender, ElapsedEventArgs e)
        {
            //Fire and Forget. Es muss nicht auf das Laden und wechseln der Bilder gewartet werden.
            PreviewShuffleAsync();
        }

        #endregion


        /// <summary>
        /// Setzt die vergangene Zeit des Preview Timers auf 0 zurück
        /// </summary>
        public void PreviewTimerReset()
        {
            if (previewTimer != null)
            {
                this.previewTimer.Stop();
                this.previewTimer.Start();
            }
        }

        /// <summary>
        /// Gibt den Status des Preview Timers zurück
        /// </summary>
        /// <returns></returns>
        public Boolean IsRunningPreviewTimer()
        {
            if (previewTimer == null)
                return false;
            else
                return true;
        }

        /// <summary>
        /// Berechnet die Zeit bis zum nächsten Tageszeitenwechsel und Startet den Timer falls diese noch nicht vorhanden ist.
        /// Falls ein Tageswechsel erfolgt ist wird auserdem das aktive Set weitergeschoben falls diese option aktiv ist
        /// </summary>
        public void DaytimeTimerStart()
        {
            TimeSpan untilTimeChange;
            DateTime timeNow = System.DateTime.Now;
            TimeSpan currentTime = timeNow.TimeOfDay;

            IsDayCheck();

            if (isDay)
                untilTimeChange = SM.Settings.NightStart - currentTime;
            else
                untilTimeChange = SM.Settings.DayStart - currentTime;

            if (untilTimeChange.Ticks < 0)
                untilTimeChange = untilTimeChange.Add(TimeSpan.FromHours(24));

            if (daytimeTimer == null)
            {
                //Beim Start des Programms wird festgestellt wieviel Zeit noch bis zum nächsten Tageszeitwechsel bleibt.

                daytimeTimer = new Timer(untilTimeChange.TotalMilliseconds);
                daytimeTimer.Elapsed += CurrentDaytime_Trigger;
                daytimeTimer.Enabled = true;
                Debug.WriteLine("Tageszeiten Timer hat gestartet. Verbleibende Zeit zum Wechsel: " + untilTimeChange);
            }
            else
            {
                //Sollte der Timer Bereits laufen und der Tageszeitenwechsel geschieht wärend der laufzeit
                //wird der Timer wieder auf die nächste Zeitspanne eingestellt.
                daytimeTimer.Stop();
                daytimeTimer.Interval = untilTimeChange.TotalMilliseconds;
                daytimeTimer.Start();
                Debug.WriteLine("Tageszeiten Timer hat aktualisiert. Verbleibende Zeit zum Wechsel: " + untilTimeChange);
            }

            AutoSetChange();
        }

        /// <summary>
        /// Neues Setzten des Tageswechsel
        /// Tageswechsel erfolgt immer beim Ende der Nacht und nicht zum wechsel des Datums um einen Wechsel zwischen den Sets wärend der Laufzeit zu verhindern
        /// Ein Set wird als (Tag -> Nacht) und nicht als (Nacht -> Tag -> Nacht) gehandelt 
        /// </summary>
        private void AutoSetChange()
        {

            if (!isDay || DateTime.Now < SM.Settings.NextDaySwitch)
            {
                SM.Settings.NextDaySwitch = DateTime.Now.Subtract(DateTime.Now.TimeOfDay).Add(TimeSpan.FromDays(1).Add(SM.Settings.DayStart));
                Debug.WriteLine("Debugging -> Nächster Tageswechsel: " + SM.Settings.NextDaySwitch);
                return;
            }

            //Weiterschieben der Sets Jedes Set wird am Tageswechsel um eine position in der Liste weitergeschoben
            //
            if (SM.Settings.AutoSetChange)
            {
                for (int i = 1; i <= 3; i++)
                {
                    if (GM.getActiveSet(i) == null) continue;

                    GM.setActiveSet(GM.getNextSet(GM.getActiveSet(i).SetName), i);
                }
            }

            SM.Settings.NextDaySwitch = DateTime.Now.Subtract(DateTime.Now.TimeOfDay).Add(TimeSpan.FromDays(1).Add(SM.Settings.DayStart));
            Debug.WriteLine("SetSwitch ausgelöst. Nächster Tageswechsel: " + SM.Settings.NextDaySwitch);
        }

        /// <summary>
        /// Trigger Event für den Wechsel der Tageszeit wärend der Laufzeit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CurrentDaytime_Trigger(object sender, System.Timers.ElapsedEventArgs e)
        {
            isDay = !isDay;
            Task.Run(() => DaytimeTimerStart());
            if (SM.Settings.IsRunning)
                Task.Run(() => PicShuffleStart());
            Debug.WriteLine("Tageszeittrigger wurde ausgelöst. isDay = " + isDay);
        }

        /// <summary>
        /// Cleanupfunktion des Shufflers. Stoppen und Disposen aller Timer
        /// </summary>
        public void ShufflerStopCleanup()
        {
            //Stoppen aller Timer und anhalten der Automatischen Windows Slideshow
            //Note: evtl. mit einem Marker festellen ob der Desktop wegen dieser Application shuffelt oder standartmäßig
            //mWindow.fader.Stop();
            if (daytimeTimer != null)
            {
                daytimeTimer.Stop();
                daytimeTimer.Dispose();
            }
            if (desktopShuffleTimer != null)
            {
                desktopShuffleTimer.Stop();
                desktopShuffleTimer.Dispose();
            }
            if (previewTimer != null)
            {
                previewTimer.Stop();
                previewTimer.Dispose();
            }

            StopWinDesktop();

            //SM.setRunning(false);
        }


        #endregion
    }
}