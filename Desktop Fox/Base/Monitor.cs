using IDesktopWallpaperWrapper;
using System;

namespace Desktop_Fox
{
    public class Monitor
    {

        public String ID;
        public String Name;
        public int Height;
        public int Width;
        public Monitor(String monID, String monName)
        {
            DesktopWallpaper tmpWrapper = new DesktopWallpaper();
            this.ID = monID;
            this.Name = monName;
            this.Height = tmpWrapper.GetMonitorRECT(monID).Height;
            this.Width = tmpWrapper.GetMonitorRECT(monID).Width;
            //var me = Screen.PrimaryScreen.DeviceFriendlyName();
            Console.WriteLine(Name);
            Console.WriteLine("Monitor Höhe: " + Height);
            Console.WriteLine("Monitor Breite: " + Width);
        }

        public int getWidth()
        {
            return this.Width;
        }
    }
}