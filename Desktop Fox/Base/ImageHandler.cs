using System;
using System.Drawing;
using System.IO;
using System.Windows.Media.Imaging;

namespace DesktopFox
{
    internal class ImageHandler
    {
        public static string BaseDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

        /// <summary>
        /// Läd eine Bilddatei, wandelt diese in ein BitmapImage um und gibt dieses zurück
        /// </summary>
        /// <param name="Path"></param>
        /// <returns></returns>
        public static BitmapImage load(String Path)
        {
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.UriSource = new Uri(Path);
            bitmapImage.EndInit();
            bitmapImage.Freeze();
            return bitmapImage;
        }
        public static BitmapImage load(String Path, double Height)
        {
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.UriSource = new Uri(Path);
            bitmapImage.DecodePixelHeight = (int)Height;
            //bitmapImage.DecodePixelWidth = (int)Width;
            bitmapImage.EndInit();

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
            bitmapImage.UriSource = new Uri("F:\\DesktopFoxTestPicture\\Normal\\1.jpg");
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
    }
}