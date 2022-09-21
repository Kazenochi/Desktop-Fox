using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Media.Imaging;
using static System.Windows.Forms.DataFormats;

namespace Desktop_Fox
{
    
    public class PictureSet : INotifyPropertyChanged
    {
        public String Name { get; set; }

        private BitmapImage _dayImage;
        public BitmapImage DayImage { 
            get { return _dayImage; }
            set
            {
                _dayImage = value;
                RaisePropertyChanged(nameof(DayImage));
            } 
        }
        private BitmapImage _nightImage;
        public BitmapImage NightImage {
            get { return _nightImage; }
            set 
            {
                _nightImage = value;
                RaisePropertyChanged(nameof(NightImage));
            } 
        }

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