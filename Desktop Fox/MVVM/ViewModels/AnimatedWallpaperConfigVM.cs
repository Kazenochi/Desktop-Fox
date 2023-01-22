using DesktopFox.MVVM.Model;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media.TextFormatting;

namespace DesktopFox.MVVM.ViewModels
{
    public class AnimatedWallpaperConfigVM
    {
        private VirtualDesktop vDesk;
        public AnimatedWallpaperConfigModel animatedWallpaperConfigModel { get; set; } = new();

        public AnimatedWallpaperConfigVM(VirtualDesktop virtualDesktop)
        {
            this.vDesk = virtualDesktop;
            
            int monitorCount = vDesk.getMonitorCount();

            if (monitorCount >= 1)
                animatedWallpaperConfigModel.Monitor1_Visible = true;

            if(monitorCount >= 2)
                animatedWallpaperConfigModel.Monitor2_Visible = true;

            if(monitorCount >= 3)
                animatedWallpaperConfigModel.Monitor3_Visible = true;

            CheckSavedWallpapers();
        }

        public ICommand SelectVideoCommand { get { return new DF_Command.DelegateCommand(o => SelectVideo()); } }

        public ICommand ActivateCommand { get { return new DF_Command.DelegateCommand(o => ActivateVideo()); } }

        public ICommand StopCommand { get { return new DF_Command.DelegateCommand(o => StopVideo()); } }

        public ICommand MuteCommand { get { return new DF_Command.DelegateCommand(o => MuteAudio()); } }

        public ICommand RotateClockwiseCommand { get { return new DF_Command.DelegateCommand(o => animatedWallpaperConfigModel.Rotation += 90); } }

        private void MuteAudio()
        {
            if (vDesk.getWallpapers.First().myModel.Volume != 0)
                vDesk.getWallpapers.First().myModel.Volume = 0;
            else
                vDesk.getWallpapers.First().myModel.Volume = 100;
        }

        private void CheckSavedWallpapers()
        {
            if (vDesk.getWallpapers == null || vDesk.getWallpapers.Count == 0) return;

            var wallpapers = vDesk.getWallpapers;
            animatedWallpaperConfigModel.SourceUri = wallpapers.First().MediaUri;

            foreach(var wallpaper in wallpapers)
            {
                switch (wallpaper.myMonitor.Name)
                {
                    case MonitorEnum.MainMonitor:
                        animatedWallpaperConfigModel.Monitor1 = true;
                        break;
                    
                    case MonitorEnum.SecondMonitor:
                        animatedWallpaperConfigModel.Monitor2 = true;
                        break;
                        
                    case MonitorEnum.ThirdMonitor:
                        animatedWallpaperConfigModel.Monitor3 = true;
                        break;
                }
            }
        }


        private void SelectVideo()
        {            
            animatedWallpaperConfigModel.SourceUri = DF_FolderDialog.openSingleFileDialog() ?? ""; 
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
                vDesk.newAnimatedWPs(monitorList, animatedWallpaperConfigModel.SourceUri, animatedWallpaperConfigModel.Rotation, true);
            }
                
        }

        private void StopVideo()
        {
            vDesk.clearWallpapers();
        }
    }
}
