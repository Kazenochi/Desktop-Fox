using System.Windows.Media;

namespace DesktopFox
{
    static class WallpaperBuilder
    {
        static public Wallpaper makeWallpaper(VirtualDesktop vDesk, int monitorNr, string mediaUri, Stretch mediaStretch = Stretch.UniformToFill)
        {
            if (monitorNr <= 0 || monitorNr > vDesk.getMonitorCount()) return null;

            Wallpaper wallpaper = new Wallpaper();

            switch (monitorNr) 
            { 
                case 1:
                    wallpaper.myMonitor = vDesk.getMainMonitor;
                    break;
                    
                case 2:
                    wallpaper.myMonitor = vDesk.getSecondMonitor;
                    break;

                case 3:
                    wallpaper.myMonitor = vDesk.getThirdMonitor;
                    break;
            }

            wallpaper.MediaUri = mediaUri;
            wallpaper.myStretch = mediaStretch; 

            return wallpaper;
        }

    }
}
