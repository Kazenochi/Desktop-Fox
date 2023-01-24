using DesktopFox.MVVM.Model;
using DesktopFox.MVVM.Views;
using System.Collections.Generic;
using DesktopFox;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media.TextFormatting;
using LibVLCSharp.Shared;

namespace DesktopFox.MVVM.ViewModels
{
    public class AnimatedWallpaperConfigVM
    {
        private VirtualDesktop vDesk;
        public AnimatedWallpaperConfigModel AWPConfigModel { get; set; } = new();
        private Wallpaper tmpWallpaper;

        public AnimatedWallpaperConfigVM(VirtualDesktop virtualDesktop)
        {
            AWPConfigModel.PropertyChanged += AWPConfigModel_PropertyChanged;
            this.vDesk = virtualDesktop;

            for(int i = 0; i < vDesk.getMonitorCount(); i++)
            {
                AWPConfigModel._monitorVisibility[i] = true;
            }

            //CheckSavedWallpapers();
        }

        private void AWPConfigModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch(e.PropertyName)
            {
                case (nameof(AWPConfigModel.Monitor1)):

                    break;
            }
        }

        #region Commands

        public ICommand SelectVideoCommand { get { return new DF_Command.DelegateCommand(o => SelectVideo()); } }

        public ICommand ActivateCommand { get { return new DF_Command.DelegateCommand(o => ActivateVideo()); } }

        public ICommand StopCommand { get { return new DF_Command.DelegateCommand(o => StopVideo()); } }

        public ICommand MuteCommand { get { return new DF_Command.DelegateCommand(o => MuteAudio()); } }

        public ICommand VolumeUpCommand { get { return new DF_Command.DelegateCommand(o => VolumeUp()); } }

        public ICommand VolumeDownCommand { get { return new DF_Command.DelegateCommand(o => VolumeDown()); } }

        public ICommand VideoPlayCommand { get { return new DF_Command.DelegateCommand(o => VideoPlay()); } }

        public ICommand VideoPauseCommand { get { return new DF_Command.DelegateCommand(o => VideoPause()); } }

        public ICommand RotateClockwiseCommand { get { return new DF_Command.DelegateCommand(o => RotateClockwise()); } }

        public ICommand HideVideo1Command { get { return new DF_Command.DelegateCommand(o => AWPConfigModel.Monitor1 = !AWPConfigModel.Monitor1); } }
        public ICommand HideVideo2Command { get { return new DF_Command.DelegateCommand(o => AWPConfigModel.Monitor2 = !AWPConfigModel.Monitor2); } }
        public ICommand HideVideo3Command { get { return new DF_Command.DelegateCommand(o => AWPConfigModel.Monitor3 = !AWPConfigModel.Monitor3); } }

        #endregion

        #region Methoden

        /// <summary>
        /// Auswählen des Videos auf dem Lokalen Rechner und Anzeige in dieser View
        /// </summary>
        private void SelectVideo()
        {
            AWPConfigModel.SourceUri = DF_FolderDialog.openSingleFileDialog() ?? "";
            if (AWPConfigModel.SourceUri == null || AWPConfigModel.SourceUri == "") return;

            tmpWallpaper ??= WallpaperBuilder.makeWallpaper(vDesk, 1, AWPConfigModel.SourceUri, framesPerSecond: FPS.Preview);

            for (int i = 0; i < vDesk.getMonitorCount(); i++)
            {
                AWPConfigModel._monitorVideos[i] = new AnimatedWallpaperView(tmpWallpaper);
            }
            AWPConfigModel.RaisePropertyChanged();
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
                vDesk.getWallpapers.First().Volume = AWPConfigModel.Volume;
            }       
            else
            {
                AWPConfigModel.Volume = vDesk.getWallpapers.First().Volume;
                vDesk.getWallpapers.First().Volume = VLCVolume.Mute;
            }              
        }

        /// <summary>
        /// Erhöht die Lautstärke um eine Stufe der Werte in <see cref="VLCVolume"/>
        /// </summary>
        private void VolumeUp()
        {
            if (vDesk.getWallpapers.Count <= 0) return;
            vDesk.getWallpapers.First().Volume = vDesk.getWallpapers.First().Volume.Next();
        }

        /// <summary>
        /// Verringert die Lautstärke um eine Stufe der Werte in <see cref="VLCVolume"/>
        /// </summary>
        private void VolumeDown()
        {
            if (vDesk.getWallpapers.Count <= 0) return;
            vDesk.getWallpapers.First().Volume = vDesk.getWallpapers.First().Volume.Previous();
        }

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
        /// Rotiert die Vorschaubilder im Uhrzeigersinn anhand der in <see cref="VLCRotation"/> vorgegebenen Werte
        /// </summary>
        private void RotateClockwise()
        {
            if (tmpWallpaper == null) return;

            tmpWallpaper.myRotation = tmpWallpaper.myRotation.Next();

            for (int i = 0; i < vDesk.getMonitorCount(); i++)
            {
                AWPConfigModel._monitorVideos[i] = new AnimatedWallpaperView(tmpWallpaper); 
            }
            AWPConfigModel.RaisePropertyChanged();
        }

        /// <summary>
        /// Überprüfen der gespeicherten Wallpaper in <see cref="VirtualDesktop.wallpapers"/> und anzeigen der Videos.
        /// </summary>
        public void CheckSavedWallpapers()
        {
        if (vDesk.getWallpapers == null || vDesk.getWallpapers.Count == 0) return;

        var wallpapers = vDesk.getWallpapers;
        Wallpaper wallpaperBluePrint = wallpapers.First();
        wallpaperBluePrint = WallpaperBuilder.ChangeToPreview(wallpaperBluePrint);
        AWPConfigModel.SourceUri = wallpapers.First().myMediaUri;   

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
        }

        /// <summary>
        /// <see cref="tmpWallpaper"/> wird an <see cref="VirtualDesktop"/> weitergegeben und als Hintergrundvideo auf dem Desktop angezeigt
        /// </summary>
        private void ActivateVideo()
        {
            if (AWPConfigModel.SourceUri == null) return;
            AWPConfigModel.PlayPauseToggle = true;

            List<int> monitorList = new List<int>();

            if (AWPConfigModel.Monitor1)
                monitorList.Add(1);

            if (AWPConfigModel.Monitor2)
                monitorList.Add(2);

            if (AWPConfigModel.Monitor3)
                monitorList.Add(3);

            if(monitorList.Count > 0)
            {
                vDesk.newAnimatedWPs(monitorList, tmpWallpaper);
            }            
        }

        /// <summary>
        /// Bereinigt den Desktop Hintergrund von allen Fenstern
        /// </summary>
        private void StopVideo()
        {
            AWPConfigModel.PlayPauseToggle = false;
            vDesk.clearWallpapers();
        }

        #endregion
    }
}
