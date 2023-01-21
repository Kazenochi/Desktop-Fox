using IDesktopWallpaperWrapper;
using System;

namespace DesktopFox
{
    public class Monitor
    {

        public String ID;
        public MonitorEnum Name;
        public int Height;
        public int Width;
        public int Number; 

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="monID">ID des Monitors</param>
        /// <param name="monName">Name unter dem der Monitor gespeichert werden soll</param>
        public Monitor(String monID, MonitorEnum monName, int height, int width)
        {
            DesktopWallpaper tmpWrapper = new DesktopWallpaper();
            ID = monID;
            Name = monName;
            Number = ((int)monName);
            Height = height;
            Width = width;
        }
    }
}