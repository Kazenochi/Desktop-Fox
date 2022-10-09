using IDesktopWallpaperWrapper;
using System;

namespace DesktopFox
{
    public class Monitor
    {

        public String ID;
        public String Name;
        public int Height;
        public int Width;

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="monID">ID des Monitors</param>
        /// <param name="monName">Name unter dem der Monitor gespeichert werden soll</param>
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

        /// <summary>
        /// Gibt die Breite des Monitors zurück
        /// </summary>
        /// <returns></returns>
        public int getWidth()
        {
            return this.Width;
        }
    }
}