using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Path = System.IO.Path;

namespace DesktopFox
{
    internal class ImageHandler
    {
        public static string BaseDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        private static string _orientationQuery = "System.Photo.Orientation";           //https://learn.microsoft.com/en-us/windows/win32/properties/props-system-photo-orientation

        /// <summary>
        /// Läd eine Bilddatei, wandelt diese in ein BitmapImage um und gibt dieses zurück
        /// </summary>
        /// <param name="Path"></param>
        /// <returns></returns>
        public static BitmapImage load(String Path)
        {
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            try
            {
                bitmapImage.UriSource = new Uri(Path);
                bitmapImage.DecodePixelHeight = 1000;
                bitmapImage.Rotation = getRotation(Path);
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                RenderOptions.SetBitmapScalingMode(bitmapImage, BitmapScalingMode.LowQuality);               
                bitmapImage = bitmapImage.Clone();
                bitmapImage.Freeze();
            }
            catch
            {
                //bitmapImage.EndInit();
                bitmapImage = dummy();
            }
            
            return bitmapImage;
        }

        /// <summary>
        /// Läd eine Bilddatei und Skaliert diese für die Vorschaubilder herunter.
        /// </summary>
        /// <param name="Path"></param>
        /// <param name="Height"></param>
        /// <returns></returns>
        public static BitmapImage load(String Path, double Height)
        {
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            try
            {           
                bitmapImage.UriSource = new Uri(Path);
                bitmapImage.DecodePixelHeight = (int)Height;
                bitmapImage.Rotation = getRotation(Path);
                bitmapImage.CacheOption= BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                RenderOptions.SetBitmapScalingMode(bitmapImage, BitmapScalingMode.LowQuality);
                bitmapImage.Freeze();
            }
            catch
            {               
                bitmapImage = dummy();
                //bitmapImage.EndInit();
            }           
            return bitmapImage;
        }

        /// <summary>
        /// Backup Methode die ein schwarzes Bild als bitmapImage zurückgibt
        /// </summary>
        /// <returns></returns>
        public static BitmapImage dummy()
        {
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.UriSource = new Uri("pack://application:,,,/Assets/dummy.jpg");
            bitmapImage.EndInit();
            bitmapImage.Freeze();
            return bitmapImage;
        }

        /// <summary>
        /// Läd das Icon der Application aus dem Assets Ordner
        /// </summary>
        /// <returns></returns>
        public static Icon loadIcon()
        {
            return new Icon(BaseDir + "\\Assets\\DF_Icon.ico");
        }

        /// <summary>
        /// Gibt die Rotation des Bildes zurück, welche in den Metadaten steht.
        /// </summary>
        /// <param name="path">Vollständiger Pfad zur Bilddatei</param>
        /// <returns></returns>
        private static Rotation getRotation(String path)
        {
            Rotation rotation = Rotation.Rotate0;
            using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                BitmapFrame bitmapFrame = BitmapFrame.Create(fileStream, BitmapCreateOptions.DelayCreation, BitmapCacheOption.None);
                BitmapMetadata bitmapMetadata = bitmapFrame.Metadata as BitmapMetadata;

                if ((bitmapMetadata != null) && (bitmapMetadata.ContainsQuery(_orientationQuery)))
                {
                    object metaObject = bitmapMetadata.GetQuery(_orientationQuery);

                    #region Diagramm zu EXIF Orientierung
                    /* EXIF Orientierung
                     * -----> Row
                     * |
                     * |
                     * | Column
                     * V
                     *        Row       Column      Rotation            Spiegel 
                     * 1    = Top       Left        
                     * 2 *  = Top       Right                           Y-Achse Gespiegelt
                     * 3    = Bottom    Right       180° Gedreht
                     * 4 *  = Bottom    Left        180° Gedreht        Y-Achse Gespiegelt 
                     * 5 *  = Left      Top          90° Gedreht        X-Achse Gespiegelt
                     * 6    = Right     Top         -90° Gedreht
                     * 7 *  = Right     Bottom      -90° Gedreht        X-Achse Gespiegelt
                     * 8    = Left      Bottom       90° Gedreht
                     */
                    #endregion

                    //Auslesen der Rotation aus dem EXIF und vorbereiten der Variable zum bearbeiten der Bitmap
                    if (metaObject != null)
                    {
                        switch ((ushort)metaObject)
                        {
                            case 6:
                                rotation = Rotation.Rotate90;
                                break;
                            case 3:
                                rotation = Rotation.Rotate180;
                                break;
                            case 8:
                                rotation = Rotation.Rotate270;
                                break;
                        }
                    }
                }
            }
            return rotation;
        }

    }
}