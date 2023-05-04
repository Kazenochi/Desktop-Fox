using DesktopFox.MVVM.Model;
using DesktopFox.MVVM.Views;
using System.Collections.Generic;
using DesktopFox;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media.TextFormatting;
using LibVLCSharp.Shared;
using Windows.UI.WindowManagement;

namespace DesktopFox.MVVM.ViewModels
{
    public class AnimatedWallpaperConfigVM
    {
        private protected VirtualDesktop vDesk;
        public AnimatedWallpaperConfigModel AWPConfigModel { get; set; } = new();
        private Wallpaper tmpWallpaper;

        public AnimatedWallpaperConfigVM(VirtualDesktop virtualDesktop)
        {
            
            this.vDesk = virtualDesktop;

            for(int i = 0; i < vDesk.getMonitorCount(); i++)
            {
                AWPConfigModel._monitorVisibility[i] = true;
            }
            AWPConfigModel.PropertyChanged += AWPConfigModel_PropertyChanged;
            //CheckSavedWallpapers();
        }

        private void AWPConfigModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch(e.PropertyName)
            {
                case (nameof(AWPConfigModel.Monitor1)):
                    if (AWPConfigModel.Monitor1 && tmpWallpaper != null)
                        AWPConfigModel.Monitor1_Video = new AnimatedWallpaperView(tmpWallpaper);                     
                    else
                        AWPConfigModel.Monitor1_Video = null;

                    CheckActivateControlLock();
                    break;

                case (nameof(AWPConfigModel.Monitor2)):
                    if (AWPConfigModel.Monitor2 && tmpWallpaper != null)
                        AWPConfigModel.Monitor2_Video = new AnimatedWallpaperView(tmpWallpaper);
                    else
                        AWPConfigModel.Monitor2_Video = null;

                    CheckActivateControlLock();
                    break;

                case (nameof(AWPConfigModel.Monitor3)):
                    if (AWPConfigModel.Monitor3 && tmpWallpaper != null)
                        AWPConfigModel.Monitor3_Video = new AnimatedWallpaperView(tmpWallpaper);
                    else
                        AWPConfigModel.Monitor3_Video = null;

                    CheckActivateControlLock();
                    break;

                case (nameof(AWPConfigModel.SourceUri)):
                    if(AWPConfigModel.SourceUri == "")
                        AWPConfigModel.VideoPreviewEnable = false;
                    else
                        AWPConfigModel.VideoPreviewEnable = true;
                    break;
                   
            }
        }


        #region Commands

        /// <summary>
        /// Kommando zum Aufrufen des <see cref="DF_FolderDialog.openSingleFileDialog"/> und einlesen des Videopfades
        /// </summary>
        public ICommand SelectVideoCommand { get { return new DF_Command.DelegateCommand(o => SelectVideo()); } }

        /// <summary>
        /// Kommando das die Videos in der Preview auf dem Desktop anzeigt. <see cref="ActivateVideo"/> & <see cref="VirtualDesktop.newAnimatedWPs(List{int}, Wallpaper)"/>
        /// </summary>
        public ICommand ActivateCommand { get { return new DF_Command.DelegateCommand(o => ActivateVideo()); } }

        /// <summary>
        /// Kommando das die Vidoes auf dem Desktop Collected und löscht. <see cref="StopVideo"/> & <see cref="VirtualDesktop.clearWallpapers(bool)"/>
        /// </summary>
        public ICommand StopCommand { get { return new DF_Command.DelegateCommand(o => StopVideo()); } }

        /// <summary>
        /// Kommando das die Lautstärke des Videos Stumm schaltet oder den Alten Wert Wiederherstellt.
        /// </summary>
        public ICommand MuteCommand { get { return new DF_Command.DelegateCommand(o => MuteAudio()); } }

        /// <summary>
        /// Kommando zur Erhöhung der Lautstärke der Videos
        /// </summary>
        public ICommand VolumeUpCommand { get { return new DF_Command.DelegateCommand(o => VolumeUp()); } }

        /// <summary>
        /// Kommando zum Verringern der Lautstärke der Videos
        /// </summary>
        public ICommand VolumeDownCommand { get { return new DF_Command.DelegateCommand(o => VolumeDown()); } }

        /// <summary>
        /// Kommando um die Videos zu Starten (Sowohl Vorschau als auch Desktop) 
        /// </summary>
        public ICommand VideoPlayCommand { get { return new DF_Command.DelegateCommand(o => VideoPlay()); } }

        /// <summary>
        /// Kommando um die Videos zu Pausiere (Sowohl Vorschau als auch Desktop)
        /// </summary>
        public ICommand VideoPauseCommand { get { return new DF_Command.DelegateCommand(o => VideoPause()); } }

        public ICommand VideoSyncCommand { get { return new DF_Command.DelegateCommand(o => VideoSync()); } }

        /// <summary>
        /// Kommando Rotieren des Videos im Uhrzeigersinn in den gespeicherten Werten <see cref="VLCRotation"/>
        /// </summary>
        public ICommand RotateClockwiseCommand { get { return new DF_Command.DelegateCommand(o => RotateClockwise()); } }

        #region Kommandos zum Anzeigen oder Verstecken der Videos
        public ICommand HideVideo1Command { get { return new DF_Command.DelegateCommand(o => AWPConfigModel.Monitor1 = !AWPConfigModel.Monitor1); } }
        public ICommand HideVideo2Command { get { return new DF_Command.DelegateCommand(o => AWPConfigModel.Monitor2 = !AWPConfigModel.Monitor2); } }
        public ICommand HideVideo3Command { get { return new DF_Command.DelegateCommand(o => AWPConfigModel.Monitor3 = !AWPConfigModel.Monitor3); } }
        #endregion

        #endregion

        #region Methoden

        /// <summary>
        /// Auswählen des Videos auf dem Lokalen Rechner und Anzeige in dieser View
        /// </summary>
        private void SelectVideo()
        {
            var tmpUri = DF_FolderDialog.openSingleFileDialog();
            if (tmpUri == null) return;
            AWPConfigModel.SourceUri = tmpUri;

            if(tmpWallpaper == null)
                tmpWallpaper = WallpaperBuilder.makeWallpaper(vDesk, 1, AWPConfigModel.SourceUri, framesPerSecond: FPS.Preview);
            else         
                tmpWallpaper.myMediaUri = AWPConfigModel.SourceUri;

            RefreshVideos();
        }

        private void CheckActivateControlLock()
        {
            AWPConfigModel.ActivateControlEnable = false;
            foreach(var i in AWPConfigModel._monitorVideos)
            {
                if(i != null) AWPConfigModel.ActivateControlEnable = true;
            }
        }

        /// <summary>
        /// Schaltet den Ton des Hintergrundbildes Stumm oder Setzt ihn wieder auf 50% 
        /// (Note: Zwischenspeichern des Wertes um die alte Lautstärke wiederherzustellen)
        /// </summary>
        private void MuteAudio()
        {
            if (vDesk.getWallpapers == null || vDesk.getWallpapers.Count <= 0) return;

            if(vDesk.getWallpapers.First().Volume == VLCVolume.Mute)
            {
                AWPConfigModel.IsMuted = false;
                vDesk.getWallpapers.First().Volume = AWPConfigModel.Volume;
            }       
            else
            {
                AWPConfigModel.Volume = vDesk.getWallpapers.First().Volume;
                AWPConfigModel.IsMuted = true;
                vDesk.getWallpapers.First().Volume = VLCVolume.Mute;
            }              
        }

        /// <summary>
        /// Erhöht die Lautstärke um eine Stufe der Werte in <see cref="VLCVolume"/>
        /// </summary>
        private void VolumeUp()
        {
            if (vDesk.getWallpapers == null || vDesk.getWallpapers.Count <= 0) return;
            if (vDesk.getWallpapers.First().Volume.Next() == VLCVolume.Mute) return;

            vDesk.getWallpapers.First().Volume = vDesk.getWallpapers.First().Volume.Next();
            AWPConfigModel.Volume = vDesk.getWallpapers.First().Volume;
        }

        /// <summary>
        /// Verringert die Lautstärke um eine Stufe der Werte in <see cref="VLCVolume"/>
        /// </summary>
        private void VolumeDown()
        {
            if (vDesk.getWallpapers == null || vDesk.getWallpapers.Count <= 0) return;
            if (vDesk.getWallpapers.First().Volume.Previous() == VLCVolume.Vol_150) return;

            vDesk.getWallpapers.First().Volume = vDesk.getWallpapers.First().Volume.Previous();
            AWPConfigModel.Volume = vDesk.getWallpapers.First().Volume;
        }

        /// <summary>
        /// Startet die Wiedergabe der Videos. Variable <see cref="Wallpaper.PlayPause"/> wird gesetzt und 
        /// von <see cref="AnimatedWallpaperView.Wallpaper_PropertyChanged(object?, System.ComponentModel.PropertyChangedEventArgs)"/> verarbeitet.
        /// </summary>
        private void VideoPlay()
        {
            if(vDesk.getWallpapers != null)
            {
                foreach (var wallpaper in vDesk.getWallpapers)
                {
                    wallpaper.PlayPause = VLCState.Playing;
                }
            }

            if (tmpWallpaper == null) return;
            tmpWallpaper.PlayPause = VLCState.Playing;
        }

        /// <summary>
        /// Stoppt die Wiedergabe der Videos. Variable <see cref="Wallpaper.PlayPause"/> wird gesetzt und 
        /// von <see cref="AnimatedWallpaperView.Wallpaper_PropertyChanged(object?, System.ComponentModel.PropertyChangedEventArgs)"/> verarbeitet.
        /// </summary>
        private void VideoPause()
        {
            if(vDesk.getWallpapers!= null)
            {
                foreach (var wallpaper in vDesk.getWallpapers)
                {
                    wallpaper.PlayPause = VLCState.Paused;
                }
            }

            if (tmpWallpaper == null) return;
            tmpWallpaper.PlayPause = VLCState.Paused;
        }

        /// <summary>
        /// Setzt alle Videos auf Anfang zurück und Synchronisiert damit alle Wiedergaben
        /// </summary>
        private void VideoSync()
        {
            if (vDesk.getWallpapers != null)
            {
                foreach (var wallpaper in vDesk.getWallpapers)
                {
                    wallpaper.PlayPause = VLCState.Stopped;
                    wallpaper.PlayPause = VLCState.Playing;
                }
            }
        }

        /// <summary>
        /// Baut die Vorschau wieder neu auf
        /// </summary>
        private void RefreshVideos()
        {
            if (tmpWallpaper == null) return;

            for (int i = 0; i < AWPConfigModel._monitors.Length; i++)
            {
                if (!AWPConfigModel._monitors[i]) continue;
                AWPConfigModel._monitorVideos[i] = new AnimatedWallpaperView(tmpWallpaper);
            }
            
            AWPConfigModel.RaisePropertyChanged();
        }

        /// <summary>
        /// Setzt die Sichtbarkeit von allen Videos auf Collapsed
        /// </summary>
        public void HideVideosOnClose()
        {
            AWPConfigModel.Video1Visibility = false;
            AWPConfigModel.Video2Visibility = false;
            AWPConfigModel.Video3Visibility = false;
        }

        /// <summary>
        /// Setzt die Sichtbarkeit von allen Videos auf Visible
        /// </summary>
        public void ShowVideosOnOpen()
        {
            if(AWPConfigModel.Monitor1)
                AWPConfigModel.Video1Visibility = true;

            if(AWPConfigModel.Monitor2)
                AWPConfigModel.Video2Visibility = true;

            if(AWPConfigModel.Monitor3)
                AWPConfigModel.Video3Visibility = true;
        }

        /// <summary>
        /// Rotiert die Vorschaubilder im Uhrzeigersinn anhand der in <see cref="VLCRotation"/> vorgegebenen Werte
        /// </summary>
        private void RotateClockwise()
        {
            if (tmpWallpaper == null) return;

            tmpWallpaper.myRotation = tmpWallpaper.myRotation.Next();
            RefreshVideos();
        }

        /// <summary>
        /// Überprüfen der gespeicherten Wallpaper in <see cref="VirtualDesktop.wallpapers"/> und anzeigen der Videos falls welche geladen wurden.
        /// </summary>
        public void CheckSavedWallpapers()
        {
            if (vDesk.getWallpapers == null || vDesk.getWallpapers.Count == 0)
            {
                RefreshVideos();
                return;
            }
                      
            var wallpapers = vDesk.getWallpapers;
            Wallpaper wallpaperBluePrint = DF_Json.objectCopy(wallpapers.First());       
            AWPConfigModel.SourceUri = wallpaperBluePrint.myMediaUri;   
            AWPConfigModel.Volume = wallpaperBluePrint.Volume;
            
            wallpaperBluePrint = WallpaperBuilder.ChangeToPreview(wallpaperBluePrint);
            tmpWallpaper = wallpaperBluePrint;

            foreach (var wallpaper in wallpapers)
            {
                switch (wallpaper.myMonitor.Name)
                {
                    case MonitorEnum.MainMonitor:
                        AWPConfigModel.Monitor1 = true;
                        AWPConfigModel.Monitor1_Video = new AnimatedWallpaperView(wallpaperBluePrint);
                        break;
                    
                    case MonitorEnum.SecondMonitor:
                        AWPConfigModel.Monitor2 = true;
                        AWPConfigModel.Monitor2_Video = new AnimatedWallpaperView(wallpaperBluePrint);
                        break;
                        
                    case MonitorEnum.ThirdMonitor:
                        AWPConfigModel.Monitor3 = true;
                        AWPConfigModel.Monitor3_Video = new AnimatedWallpaperView(wallpaperBluePrint);
                        break;
                    }
            }
            CheckActivateControlLock();
            AWPConfigModel.AudioEnable = true;
        }

        /// <summary>
        /// <see cref="tmpWallpaper"/> wird an <see cref="VirtualDesktop"/> weitergegeben und als Hintergrundvideo auf dem Desktop angezeigt
        /// </summary>
        private void ActivateVideo()
        {
            if (AWPConfigModel.SourceUri == null) return;
            if (tmpWallpaper == null) return;

            
            List<int> monitorList = new List<int>();

            if (AWPConfigModel.Monitor1)
                monitorList.Add(1);

            if (AWPConfigModel.Monitor2)
                monitorList.Add(2);

            if (AWPConfigModel.Monitor3)
                monitorList.Add(3);

            if(monitorList.Count > 0)
            {
                vDesk.newAnimatedWPs(monitorList, tmpWallpaper, AWPConfigModel.Volume);
            }
            AWPConfigModel.AudioEnable = true;
        }

        /// <summary>
        /// Bereinigt den Desktop Hintergrund von allen Fenstern
        /// </summary>
        private void StopVideo()
        {          
            AWPConfigModel.AudioEnable = false;
            vDesk.clearWallpapers();
        }

        #endregion

        #region Funktionsablauf der Lock Variablen
        /*
        1. Auswählen des Ordner Pfads oder laden Des Ordnerpfads rufen den Listener auf und Schalten Die Monitor Frei
        2. Anwählen der Monitore Löst das Toggle Event des Listeners auf und Über die Checkfunktion wird ermittelt ob die Controls verfügbar sind
           Diese können nur gewählt werden wenn mindestens ein Monitor Ausgewählt ist.
        3. Wenn über die Controlls ein Bild als Hintergrund Aktiviert wird. Werden Die Audio Elemente Freigeschaltet.
         
         */
        #endregion
    }
}
