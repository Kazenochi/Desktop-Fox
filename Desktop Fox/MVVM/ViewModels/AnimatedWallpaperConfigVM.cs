using DesktopFox.MVVM.Model;
using DesktopFox.MVVM.Views;
using System.Collections.Generic;
using DesktopFox;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media.TextFormatting;

namespace DesktopFox.MVVM.ViewModels
{
    public class AnimatedWallpaperConfigVM
    {
        private VirtualDesktop vDesk;
        public AnimatedWallpaperConfigModel animatedWallpaperConfigModel { get; set; } = new();
        private Wallpaper tempWallpaper;

        public AnimatedWallpaperConfigVM(VirtualDesktop virtualDesktop)
        {
            this.vDesk = virtualDesktop;

            for(int i = 0; i < vDesk.getMonitorCount(); i++)
            {
                animatedWallpaperConfigModel._monitorVisibility[i] = true;
            }

            //CheckSavedWallpapers();
        }

        public ICommand SelectVideoCommand { get { return new DF_Command.DelegateCommand(o => SelectVideo()); } }

        public ICommand ActivateCommand { get { return new DF_Command.DelegateCommand(o => ActivateVideo()); } }

        public ICommand StopCommand { get { return new DF_Command.DelegateCommand(o => StopVideo()); } }

        public ICommand MuteCommand { get { return new DF_Command.DelegateCommand(o => MuteAudio()); } }

        public ICommand VolumeUpCommand { get { return new DF_Command.DelegateCommand(o => VolumeUp()); } }

        public ICommand VolumeDownCommand { get { return new DF_Command.DelegateCommand(o => VolumeDown()); } }

        public ICommand RotateClockwiseCommand { get { return new DF_Command.DelegateCommand(o => animatedWallpaperConfigModel.Rotation += 90); } }


        private void SelectVideo()
        {
            animatedWallpaperConfigModel.SourceUri = DF_FolderDialog.openSingleFileDialog() ?? "";
            if (animatedWallpaperConfigModel.SourceUri == null || animatedWallpaperConfigModel.SourceUri == "") return;

            tempWallpaper ??= WallpaperBuilder.makeWallpaper(vDesk, 1, animatedWallpaperConfigModel.SourceUri, framesPerSecond: FPS.Preview);

            for (int i = 0; i < vDesk.getMonitorCount(); i++)
            {
                animatedWallpaperConfigModel._monitorViews[i] = new AnimatedWallpaperView(tempWallpaper);
                animatedWallpaperConfigModel.RaisePropertyChanged();

            }      
        }

        private void MuteAudio()
        {
            if (vDesk.getWallpapers.Count <= 0) return;

            if(vDesk.getWallpapers.First().Volume == Enums.Volume.Mute)
                vDesk.getWallpapers.First().Volume = Enums.Volume.Vol_50;
            else
                vDesk.getWallpapers.First().Volume = Enums.Volume.Mute;
        }

        private void VolumeUp()
        {
            if (vDesk.getWallpapers.Count <= 0) return;
            vDesk.getWallpapers.First().Volume = vDesk.getWallpapers.First().Volume.Next();
        }

        private void VolumeDown()
        {
            if (vDesk.getWallpapers.Count <= 0) return;
            vDesk.getWallpapers.First().Volume = vDesk.getWallpapers.First().Volume.Next();
        }

        /// <summary>
        /// Überprüfen der Gespeicherten Wallpaper in <see cref="VirtualDesktop.wallpapers"/> und anzeigen der Videos.
        /// </summary>
        public void CheckSavedWallpapers()
        {
            if (vDesk.getWallpapers == null || vDesk.getWallpapers.Count == 0) return;

            var wallpapers = vDesk.getWallpapers;
            Wallpaper wallpaperBluePrint = wallpapers.First();
            wallpaperBluePrint = WallpaperBuilder.ChangeToPreview(wallpaperBluePrint);
            animatedWallpaperConfigModel.SourceUri = wallpapers.First().myMediaUri;   

            foreach (var wallpaper in wallpapers)
            {
                switch (wallpaper.myMonitor.Name)
                {
                    case MonitorEnum.MainMonitor:
                        animatedWallpaperConfigModel.Monitor1 = true;
                        animatedWallpaperConfigModel.Monitor1_Video = new AnimatedWallpaperView(wallpaperBluePrint);
                        break;
                    
                    case MonitorEnum.SecondMonitor:
                        animatedWallpaperConfigModel.Monitor2 = true;
                        animatedWallpaperConfigModel.Monitor2_Video = new AnimatedWallpaperView(wallpaperBluePrint);
                        break;
                        
                    case MonitorEnum.ThirdMonitor:
                        animatedWallpaperConfigModel.Monitor3 = true;
                        animatedWallpaperConfigModel.Monitor3_Video = new AnimatedWallpaperView(wallpaperBluePrint);
                        break;
                }
            }
        }


        private void ActivateVideo()
        {
            if (animatedWallpaperConfigModel.SourceUri == null) return;

            List<int> monitorList = new List<int>();

            if (animatedWallpaperConfigModel.Monitor1)
                monitorList.Add(1);

            if (animatedWallpaperConfigModel.Monitor2)
                monitorList.Add(2);

            if (animatedWallpaperConfigModel.Monitor3)
                monitorList.Add(3);

            if(monitorList.Count > 0)
            {
                vDesk.newAnimatedWPs(monitorList, tempWallpaper);
            }
                
        }

        private void StopVideo()
        {
            vDesk.clearWallpapers();
        }
    }
}
