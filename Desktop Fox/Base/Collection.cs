using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Windows.Media.Imaging;

namespace DesktopFox
{
    /// <summary>
    /// Hält die Informationen für ein Bild
    /// </summary>
    public class Picture
    {
        /// <summary>
        /// Name der Bild Datei
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// Absoluter Pfad zum Bild
        /// </summary>
        public String Path { get; set; }    //Speicherort des Bildes

        /// <summary>
        /// Zuweisung des Absoluten Pfades zum Bild und extrahieren des Datei Namens
        /// </summary>
        /// <param name="path">Absoluter Pfad zum Bild</param>
        public Picture(string path)
        {
            this.Path = path;
            this.Name = System.IO.Path.GetFileName(path);
        }
    }

    /// <summary>
    /// Beinhaltet eine Collection von Bildern
    /// </summary>
    public class Collection
    {
        /// <summary>
        /// Collection die die Bilder beinhaltet 
        /// </summary>
        public IDictionary<String, Picture> singlePics { get; set; }                //Note: Dictionary evtl. unnötig. List<Picture> müsste ausreichen 

        /// <summary>
        /// Absoluter Pfad zum Ordner in dem Sich die Bilder befinden
        /// </summary>
        public String folderDirectory;

        /// <summary>
        /// Konstruktor
        /// </summary>
        public Collection()
        {
            singlePics = new Dictionary<String, Picture>();
        }

        /// <summary>
        /// Helfer Methode die das erste Element in der Collection zurück gibt. 
        /// </summary>
        /// <returns></returns>
        public BitmapImage getPreview()
        {
            try
            {
                return ImageHandler.load(singlePics.ElementAt(0).Key, 300);
            }
            catch (System.ArgumentOutOfRangeException)
            {
                return ImageHandler.dummy();
            }
        }
    }
}