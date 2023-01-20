using DesktopFox.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

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

        }

        public ICommand SelectVideoCommand { get { return new DF_Command.DelegateCommand(o => SelectVideo()); } }

        public ICommand ActivateCommand { get { return new DF_Command.DelegateCommand(o => ActivateVideo()); } }

        public ICommand StopCommand { get { return new DF_Command.DelegateCommand(o => StopVideo()); } }


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
                vDesk.newAnimatedWPs(monitorList, animatedWallpaperConfigModel.SourceUri);
        }

        private void StopVideo()
        {
            vDesk.clearWallpapers();
        }
    }
}
