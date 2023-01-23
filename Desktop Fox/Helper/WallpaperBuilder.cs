using DesktopFox.Enums;
using System.Windows.Media;

namespace DesktopFox
{
    static class WallpaperBuilder
    {
        /// <summary>
        /// Baut ein Objekt vom Typ <see cref="Wallpaper"/>, mit den benötigten Variablen zusammen.
        /// Dient nach aktuellem stand nur für Animierte Hintergründe
        /// </summary>
        /// <param name="vDesk">Virtual Desktop Klasse</param>
        /// <param name="monitorNr">Auf welchem Monitor das Wallpaper angezeigt werden soll</param>
        /// <param name="mediaUri">Pfad zur Zieldatei die angezeigt werden soll</param>
        /// <param name="imageRotation">Rorationswert den das Hintergrundbild annehmen soll</param>
        /// <param name="muted">Gibt an ob das Hintergrundbild Stumm geschaltet sein soll</param>
        /// <param name="mediaStretch"><see cref="Stretch"/> Wert des Hintergrundbildes</param>
        /// <returns></returns>
        static public Wallpaper makeWallpaper(VirtualDesktop vDesk, int monitorNr, string mediaUri, 
            VLCRotation imageRotation = VLCRotation.None, Volume volume = Volume.Mute, FPS framesPerSecond = FPS.FPS_60)
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

            wallpaper.myMediaUri = mediaUri;
            wallpaper.myFPS = framesPerSecond; 
            wallpaper.myRotation = imageRotation;
            wallpaper.Volume = volume;

            return wallpaper;
        }


        static public Wallpaper ChangeToPreview(Wallpaper wallpaper)
        {
            wallpaper.myFPS = FPS.Preview;
            return wallpaper;
        }
    }
}
