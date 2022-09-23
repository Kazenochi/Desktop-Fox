using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using static System.Windows.Forms.DataFormats;

namespace DesktopFox
{
    
    public class PictureSet : ListBoxItem, INotifyPropertyChanged
    {
        private String _setName;
        public String SetName { get { return _setName; } set { _setName = value; RaisePropertyChanged(nameof(SetName)); } }

        private Boolean _isSelectedDF = false;
        public Boolean IsSelectedDF { get { return _isSelectedDF; } set { _isSelectedDF = value; RaisePropertyChanged(nameof(IsSelectedDF)); } }

        private Boolean _isActive1 = false;
        public Boolean IsActive1 { get { return _isActive1; } set { _isActive1 = value; RaisePropertyChanged(nameof(IsActive1)); } }

        private Boolean _isActive2 = false;
        public Boolean IsActive2 { get { return _isActive2; } set { _isActive2 = value; RaisePropertyChanged(nameof(IsActive2)); } }

        private Boolean _isActive3 = false;
        public Boolean IsActive3 { get { return _isActive3; } set { _isActive3 = value; RaisePropertyChanged(nameof(IsActive3)); } }


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