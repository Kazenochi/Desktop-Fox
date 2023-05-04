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

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="monID">ID des Monitors</param>
        /// <param name="monName">Name unter dem der Monitor gespeichert werden soll</param>
        /// <param name="height">Höhe des Monitors</param>
        /// <param name="width">Breite des Monitors</param>
        public Monitor(String monID, MonitorEnum monName, int height, int width)
        {
            DesktopWallpaper tmpWrapper = new DesktopWallpaper();
            ID = monID;
            Name = monName;   
            Height = height;
            Width = width;
        }
    }
}