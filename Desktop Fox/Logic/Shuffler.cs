using DesktopFox;
using DesktopFox.MVVM.Model;
using DesktopFox.MVVM.ViewModels;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using Timer = System.Timers.Timer;

namespace DesktopFox
{
    public class Shuffler
    {
        private MainWindow mWindow;
        private MainWindowVM MWVM;
        private PreviewVM previewVM;
        private VirtualDesktop vDesk;
        private GalleryManager GM;
        private SettingsManager SM;
        public Boolean isDay;
        private Boolean previewDay = true;
        private int picCount1 = 0;
        private int picCount2 = 0;
        private int picCount3 = 0;
        private int[] lockList1 = new int[3];
        private int[] lockList2 = new int[3];
        private int[] lockList3 = new int[3];
        private Settings _settings;
        private int previewCount = 0;
        private Timer previewTimer;
        private Timer desktopShuffleTimer;
        private Timer daytimeTimer;
        private TimeSpan PreviewShuffleTime = TimeSpan.FromSeconds(10);
        
        private Random random = new Random();

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
            Task.Run(() => daytimeTimerStart());
        }

        private void MainWindowVM_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(MWVM.SelectedItem)) return;
        }

        /// <summary>
        /// Listener für änderungen in den Settings auf die der Shuffler reagieren muss.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Shuffler_Settings_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(SM.Settings.IsRunning):
                    if (SM.Settings.IsRunning)
                        picShuffleStart();
                    else
                        picShuffleStop();
                    break;

                case nameof(SM.Settings.DesktopModeSingle):
                    if (SM.Settings.IsRunning)
                        picShuffleStart();
                    break;

                //Reagiert auf Night da Beide angestoßen werden und Night der Letzte Wert ist der geändert wird.
                case nameof(SM.Settings.NightStart):
                    daytimeTimerStart();
                    break;

                default: return;
            }
        }

        public void mWinHandler(MainWindow mainWindow)
        {
            mWindow = mainWindow;
        }

        /// <summary>
        /// Shuffle Funktion für den Windows eigenen Shuffel Modus. 
        /// </summary>
        /// <param name="path"></param>
        public void winPicShuffle(String path)
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
        /// Startet anhand der Einstellungen die jeweiligen Manager.
        /// </summary>
        public void picShuffleStart()
        {

            if (SM.Settings.DesktopModeSingle && GM.getActiveSet() == null)
                return;
            else if (GM.getActiveSet(any: true) == null)
                return;

            Debug.WriteLine("Start des Wechsels von Windows und DF Shuffle. Einzelmodus = " + SM.Settings.DesktopModeSingle);

            if (SM.Settings.DesktopModeSingle)
            {
                stopDesktopTimer();
                if (isDay)
                    winPicShuffle(GM.getDayCollection(GM.getActiveSet().SetName).folderDirectory);
                else
                    winPicShuffle(GM.getNightCollection(GM.getActiveSet().SetName).folderDirectory);
            }
            else 
            { 
                startDesktopTimer();
                stopWinDesktop();

                //Einmaliges Anstoßen der Desktopfunktion beim Ändern der Einstellungen
                //desktopTimer_Trigger(null, null);
            }
            
        }

        /// <summary>
        /// Stoppt den Shuffle für den Desktophintergrund
        /// </summary>
        public void picShuffleStop()
        {
            if (desktopShuffleTimer != null)
            {
                desktopShuffleTimer.Stop();
                //desktopShuffleTimer.Dispose();
            }
            stopWinDesktop();
        }

        /// <summary>
        /// Start des Preview Timers
        /// </summary>
        public void startPreviewShuffleTimer()
        {
            if (previewTimer == null)
            {
                previewTimer = new Timer();
                //timer.Tick += new EventHandler(previewShuffle);
                previewTimer.Interval = PreviewShuffleTime.TotalMilliseconds;
                previewTimer.Elapsed += new ElapsedEventHandler(dispatcher);
                previewTimer.Start();
                Debug.WriteLine("Timer gestartet");
            }
            else
            {
                previewTimer.Stop();
                previewTimer.Start();
            }
            //Einmaliger Aufruf um Bild in der Vorschau zu aktualisieren
            dispatcher(null, null);
        }

        /// <summary>
        /// Stoppt den Preview Shuffle Timer
        /// </summary>
        public void stopPreviewShuffleTimer()
        {
            if (previewTimer != null)
                previewTimer.Stop();
        }

        /// <summary>
        /// Setzt die vergangene Zeit des Preview Timers auf 0 zurück
        /// </summary>
        public void previewTimerReset()
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
        public Boolean isRunningPreviewTimer()
        {
            if (previewTimer == null)
                return false;
            else
                return true;
        }

        ///<summary>
        /// Auslagerungs Methode um die Kollision mit dem UI Thread zu verhindern.
        /// Das Ändern der Bilder wird von diesem Thread an den UI Thread weitergegeben.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void dispatcher(object sender, ElapsedEventArgs e)
        {
            //Fire and Forget. Es muss nicht auf das Laden und wechseln der Bilder gewartet werden.
            previewShuffleAsync();
        }

        /// <summary>
        /// Funktion die das Große Preview Bild shuffelt
        /// </summary>
        public async Task previewShuffleAsync()
        {
            Debug.WriteLine("Preview Shuffle ausgelöst");
            var tmpPreviewSet = GM.getPreviewSet();
            PreviewModel tmpPreviewModel = previewVM.PreviewModel;

            if (tmpPreviewSet == null)
            {
                //tmpPreviewModel.ForegroundImage = await Task.Run(() => ImageHandler.dummy());
            }
            else
            {
                if (tmpPreviewModel.Day)
                {
                    if (previewCount >= 0 && previewCount < GM.getDayCollection(tmpPreviewSet.SetName).singlePics.Count)
                    {
                        tmpPreviewModel.ForegroundImage = await Task.Run(() => ImageHandler.load(GM.getDayCollection(tmpPreviewSet.SetName).singlePics.ElementAt(previewCount).Key)) ;
                        previewCount++;
                    }
                    else
                    {
                        //Anzeigen des Bildes an Erster Stelle und setzten des Counters um einene gleichmäßige Rotation zu ermöglichen
                        previewCount = 1;
                        tmpPreviewModel.ForegroundImage = await Task.Run(() => ImageHandler.load(GM.getDayCollection(tmpPreviewSet.SetName).singlePics.ElementAt(0).Key));
                    }
                }
                else
                {
                    if (previewCount >= 0 && previewCount < GM.getNightCollection(tmpPreviewSet.SetName).singlePics.Count)
                    {
                        tmpPreviewModel.ForegroundImage = await Task.Run(() => ImageHandler.load(GM.getNightCollection(tmpPreviewSet.SetName).singlePics.ElementAt(previewCount).Key));
                        previewCount++;
                    }
                    else
                    {
                        previewCount = 1;
                        tmpPreviewModel.ForegroundImage = await Task.Run(() => ImageHandler.load(GM.getNightCollection(tmpPreviewSet.SetName).singlePics.ElementAt(0).Key));
                    }
                }
            }
            //Start des Fade Übergangs

            previewVM.PreviewTransition();
        }

        /// <summary>
        /// Zeigt im Preview Fenster das nächste Bild an
        /// </summary>
        public void previewForward()
        {
            if (/*previewVM.PreviewModel.FaderLock == false &&*/ GM.getPreviewSet() != null)
            {
                previewTimerReset();
                previewShuffleAsync();
                previewVM.PreviewModel.FaderLock = true;
            }
        }

        /// <summary>
        /// Aktualisiert das Bilder im Preview Fenster
        /// </summary>
        public void previewRefresh()
        {
            if (true/*previewVM.PreviewModel.FaderLock == false &&*/)
            {
                previewTimerReset();
                if(previewCount > 0)
                    previewCount--;

                previewShuffleAsync();
                previewVM.PreviewModel.FaderLock = true;
            }
            else
            {
                Debug.WriteLine("Debug");
            }
        }

        /// <summary>
        /// Zeigt im Preview Fenster das vorherige Bild an
        /// </summary>
        public void previewBackward()
        {
            if (/*previewVM.PreviewModel.FaderLock == false &&*/ GM.getPreviewSet() != null)
            {
                previewTimerReset();
                previewCount = previewCount - 2;
                if (previewCount < 0)
                {
                    if (previewDay)
                        previewCount = GM.getDayCollection(GM.getPreviewSet().SetName).singlePics.Count - 1;
                    else
                        previewCount = GM.getNightCollection(GM.getPreviewSet().SetName).singlePics.Count - 1;
                }
                previewShuffleAsync();
                previewVM.PreviewModel.FaderLock = true;
            }
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

            stopWinDesktop();

            //SM.setRunning(false);
        }

        /// <summary>
        /// Startet den Timer für den Desktop Shuffler
        /// </summary>
        public void startDesktopTimer()
        {
            //Timer der nur für die eigene Shuffle Funktion benötigt wird. Wird ausschließlich für den Multimode verwendet.
            if (desktopShuffleTimer == null)
            {
                desktopShuffleTimer = new Timer(SM.Settings.ShuffleTime.TotalMilliseconds); //Timer läuft in MS Umwandlung ("Wert vom Benutzer" * Minute * Sekunde)
                desktopShuffleTimer.Elapsed += desktopTimer_Trigger;
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
            desktopTimer_Trigger(null, null);
            if (!SM.Settings.IsRunning)
                SM.Settings.IsRunning = true;
        }

        /// <summary>
        /// Stoppt den Desktop Timer
        /// </summary>
        public void stopDesktopTimer()
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
        public void stopWinDesktop(Boolean Shutdown = false)
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
        /// Berechnet die Zeit bis zum nächsten Tageszeitenwechsel und Startet den Timer falls diese noch nicht vorhanden ist.
        /// Falls ein Tageswechsel erfolgt ist wird auserdem das aktive Set weitergeschoben falls diese option aktiv ist
        /// </summary>
        public void daytimeTimerStart()
        {
            TimeSpan untilTimeChange;
            DateTime timeNow = System.DateTime.Now;
            TimeSpan currentTime = timeNow.TimeOfDay;

            isDayCheck();
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
                daytimeTimer.Elapsed += currentDaytime_Trigger;
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

            //Neues Setzten des Tageswechsel
            //Tageswechsel erfolgt immer beim ende der Nacht und nicht zum wechsel des Datums um ein wechsel zwischen den Sets wärend der Laufzeit zu verhindern
            //Ein Set wird als (Tag -> Nacht) und nicht als (Nacht -> Tag -> Nacht) gehandelt 
            if (isDay && DateTime.Now > SM.Settings.NextDaySwitch)
            {
                //Weiterschieben der Sets Jedes Set wird am Tageswechsel um eine position in der Liste weitergeschoben
                //
                if (SM.Settings.AutoSetChange)
                {
                    for (int i = 1; i <= 3; i++)
                    {
                        if (GM.getActiveSet(i) != null)
                            GM.setActiveSet(GM.getNextSet(GM.getActiveSet(i).SetName), i);
                    }
                }

                SM.Settings.NextDaySwitch = DateTime.Now.Subtract(DateTime.Now.TimeOfDay).Add(TimeSpan.FromDays(1).Add(SM.Settings.DayStart));
                Debug.WriteLine("SetSwitch ausgelöst. Nächster Tageswechsel: " + SM.Settings.NextDaySwitch);
            }
            else
            {
                SM.Settings.NextDaySwitch = DateTime.Now.Subtract(DateTime.Now.TimeOfDay).Add(TimeSpan.FromDays(1).Add(SM.Settings.DayStart));
                Debug.WriteLine("Debugging -> Nächster Tageswechsel: " + SM.Settings.NextDaySwitch);
            }

            //Debug
            //DateTime newDaySwitch = DateTime.Now.Subtract(DateTime.Now.TimeOfDay).Add(TimeSpan.FromDays(1).Add(SM.getDayStart()));

        }
       
        /// <summary>
        /// Trigger Event für den Wechsel der Tageszeit wärend der Laufzeit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void currentDaytime_Trigger(object sender, System.Timers.ElapsedEventArgs e)
        {
            isDay = !isDay;
            Task.Run(() => daytimeTimerStart());
            if (SM.Settings.IsRunning)
                Task.Run(() => picShuffleStart());
            Debug.WriteLine("Tageszeittrigger wurde ausgelöst. isDay = " + isDay);
        }
         
        /// <summary>
        /// Trigger Event des Desktop Timers. Überprüft die Anzahl der Monitore und gibt die Informationen an den Shuffler weiter.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void desktopTimer_Trigger(object sender, System.Timers.ElapsedEventArgs e)
        {
            //Überprüft wieviele Monitore angesprochen werden müssen und gibt diese
            //Information an den eigenen Shuffler weiter.
            //Aktueller Stand und Bilder werden von Ihm abgefragt und neu belegt bzw. an den         


            //Sicherheitsabfrage falls der benutzer kein aktives Set ausgewählt hat. Für die Anderen Monitore wird dann das erste Set genommen
            Collection tmpCol2;
            Collection tmpCol3;

            if (isDay)
            {
                if (GM.getActiveSet(2) == null)
                    tmpCol2 = GM.getDayCollection(GM.getActiveSet(any: true).SetName);
                else
                    tmpCol2 = GM.getDayCollection(GM.getActiveSet(2, any: true).SetName);

                if (GM.getActiveSet(3) == null)
                    tmpCol3 = GM.getDayCollection(GM.getActiveSet(any: true).SetName);
                else
                    tmpCol3 = GM.getDayCollection(GM.getActiveSet(3, any: true).SetName);
            }
            else
            {
                if (GM.getActiveSet(2) == null)
                    tmpCol2 = GM.getNightCollection(GM.getActiveSet(any: true).SetName);
                else
                    tmpCol2 = GM.getNightCollection(GM.getActiveSet(2, any: true).SetName);

                if (GM.getActiveSet(3) == null)
                    tmpCol3 = GM.getNightCollection(GM.getActiveSet(any: true).SetName);
                else
                    tmpCol3 = GM.getNightCollection(GM.getActiveSet(3, any: true).SetName);
            }



            Debug.WriteLine("Start des Desktop Triggers: isDay = " + isDay);
            //Note: Kann noch mit und zusammenfassung verbessert werdenforeach verbessert/erweitert werden....
            switch (vDesk.getMonitorCount())
            {
                case 1:
                    if (isDay)
                        df_PicShuffle(vDesk.getMainMonitor.ID, GM.getDayCollection(GM.getActiveSet(any: true).SetName), 1);
                    else
                        df_PicShuffle(vDesk.getMainMonitor.ID, GM.getNightCollection(GM.getActiveSet(any: true).SetName), 1);
                    break;
                case 2:
                    if (isDay)
                    {
                        df_PicShuffle(vDesk.getMainMonitor.ID, GM.getDayCollection(GM.getActiveSet(any: true).SetName), 1);
                        df_PicShuffle(vDesk.getSecondMonitor.ID, tmpCol2, 2);
                    }
                    else
                    {
                        df_PicShuffle(vDesk.getMainMonitor.ID, GM.getNightCollection(GM.getActiveSet(any: true).SetName), 1);
                        df_PicShuffle(vDesk.getSecondMonitor.ID, tmpCol2, 2);
                    }
                    break;
                case 3:
                    if (isDay)
                    {
                        df_PicShuffle(vDesk.getMainMonitor.ID, GM.getDayCollection(GM.getActiveSet(any: true).SetName), 1);
                        df_PicShuffle(vDesk.getSecondMonitor.ID, tmpCol2, 2);
                        df_PicShuffle(vDesk.getThirdMonitor.ID, tmpCol3, 3);
                    }
                    else
                    {
                        df_PicShuffle(vDesk.getMainMonitor.ID, GM.getNightCollection(GM.getActiveSet(any: true).SetName), 1);
                        df_PicShuffle(vDesk.getSecondMonitor.ID, tmpCol2, 2);
                        df_PicShuffle(vDesk.getThirdMonitor.ID, tmpCol3, 3);
                    }
                    break;
            }
            tmpCol2 = null;
            tmpCol3 = null;
        }

        /// <summary>
        /// Eigener Shuffler. Shuffelt/Rotiert die Bilder der Collection und setzt sie als Hintergundbild des Monitors. 
        /// </summary>
        /// <param name="monitorID"></param>
        /// <param name="activeCol"></param>
        private void df_PicShuffle(String monitorID, Collection activeCol, int monitor)
        {
            int tmpCount = 0;
            int[] tmpLockList;
            switch (monitor)
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
                vDesk.getWrapper.SetWallpaper(monitorID, activeCol.singlePics.ElementAt(tmpCount).Key);      
            }
            else
            {
                //Lineare Anzeige aller Bilder im Set
                if (tmpCount < activeCol.singlePics.Count - 1)
                    tmpCount++;
                else
                    tmpCount = 0;
                
                vDesk.getWrapper.SetWallpaper(monitorID, activeCol.singlePics.ElementAt(tmpCount).Key);
            }

            Debug.Write("Ändern des Hintergrundes. Monitor: " + monitor + ", Zahl: " + tmpCount + ", LockListe: ");
            foreach(var item in tmpLockList)
            {
                Debug.Write(item + ", ");
            }
            Debug.WriteLine("");
            //Note: Wofür... Bestimmt wichtig :)... Rückgabe des Counters 
            switch (monitor)
            {
                case 1: picCount1 = tmpCount; break;
                case 2: picCount2 = tmpCount; break;
                case 3: picCount3 = tmpCount; break;
            }
        }

        /// <summary>
        /// Überprüft was aktuell für eine Tageszeit ist und setzt das lokale Flag dafür
        /// </summary>
        private async Task isDayCheck()
        {
            TimeSpan dayStart = SM.Settings.DayStart;
            TimeSpan nightStart = SM.Settings.NightStart;
            DateTime timeNow = System.DateTime.Now;
            TimeSpan currentTime = timeNow.TimeOfDay;

            //Unterscheidung zwischen der Anordnung des Tagesstarts und Festlegen des Default falls die Tag/Nacht Zeit gleich ist.
            if(dayStart < nightStart)
            {
                if (currentTime > dayStart && currentTime < nightStart)
                    isDay = true;
                else
                    isDay = false;
            }
            else if(dayStart > nightStart)                                  
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



            if (SM.Settings.IsRunning)
                Task.Run(() => picShuffleStart());

            Debug.WriteLine("Tageszeit wurde überprüft. Aktuell ist isDay: " + isDay);
        }     
    }
}