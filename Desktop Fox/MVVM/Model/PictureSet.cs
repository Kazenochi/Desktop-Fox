using System;
using System.Drawing;
using System.Linq;
using System.Windows.Media.Imaging;

namespace Desktop_Fox
{
    
    public class PictureSet
    {
        /// <summary>
        /// Name des PictureSets
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// Das erste Bild in der Tages Collection. Falls die Collection keinen Inhalt hat, wird ein schwarzes Bild zurück gegeben.
        /// </summary>
        public BitmapImage DayImage {
            get
            {
                if (DayCol.singlePics.Count() > 0)
                    return ImageHandler.load(this.DayCol.singlePics.ElementAt(0).Value.Name);
                else
                    return ImageHandler.dummy();
            }
            set {} 
        }

        /// <summary>
        /// Das erste Bild in der Nacht Collection. Falls die Collection keinen Inhalt hat, wird ein schwarzes Bild zurück gegeben.
        /// </summary>
        public BitmapImage NightImage
        {
            get
            {
                if (NightCol.singlePics.Count() > 0)
                    return ImageHandler.load(this.NightCol.singlePics.ElementAt(0).Value.Name);
                else
                    return ImageHandler.dummy();
            }
            set { }
        }

        /// <summary>
        /// Sammlung von Bilder die wärend des Tages angezeigt werden
        /// </summary>
        public Collection DayCol { get; set; }

        /// <summary>
        /// Sammlung von Bilder die wärend der Nacht angezeigt werden
        /// </summary>
        public Collection NightCol { get; set; }

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="name">Name den das Set haben soll</param>
        public PictureSet(string name)
        {
            this.Name = name;
            DayCol = new Collection();
            NightCol = new Collection();
        }
    }
}