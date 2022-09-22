using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Media.Imaging;
using static System.Windows.Forms.DataFormats;

namespace DesktopFox
{
    
    public class PictureSet : INotifyPropertyChanged
    {
        public String Name { get; set; }

        private BitmapImage _dayImage;
        public BitmapImage DayImage { 
            get { return DayCol.getPreview(); }
            set
            {
                _dayImage = value;
                RaisePropertyChanged(nameof(DayImage));
            } 
        }
        private BitmapImage _nightImage;
        public BitmapImage NightImage {
            get { return NightCol.getPreview(); }
            set 
            {
                _nightImage = value;
                RaisePropertyChanged(nameof(NightImage));
            } 
        }


        /// <summary>
        /// Sammlung von Bilder die wärend des Tages angezeigt werden
        /// </summary>
        public Collection DayCol { get; set; }

        /// <summary>
        /// Sammlung von Bilder die wärend der Nacht angezeigt werden
        /// </summary>
        public Collection NightCol { get; set; }



        public PictureSet(string name)
        {
            Name = name;
            DayImage = ImageHandler.load("F:\\DesktopFoxTestPicture\\Normal\\1.jpg");
            NightImage = ImageHandler.load("F:\\DesktopFoxTestPicture\\Normal\\2.jpg");
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}