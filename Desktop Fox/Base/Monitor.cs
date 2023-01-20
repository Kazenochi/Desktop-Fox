using IDesktopWallpaperWrapper;
using System;
using System.Security.RightsManagement;

namespace DesktopFox
{
    public class Monitor
    {

        public String ID;
        public String Name;
        public int Height;
        public int Width;
        public int Number; 

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

            if(monName == "Main")
                this.Number = 1;
            else if(monName == "Second")
                this.Number = 2;
            else
                this.Number = 3;

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