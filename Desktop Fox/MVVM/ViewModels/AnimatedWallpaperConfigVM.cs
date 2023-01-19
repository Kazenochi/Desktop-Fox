using DesktopFox.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopFox.MVVM.ViewModels
{
    class AnimatedWallpaperConfigVM
    {
        private VirtualDesktop vDesk;
        public AnimatedWallpaperConfigModel animatedWallpaperConfigModel = new AnimatedWallpaperConfigModel();

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
    }
}
