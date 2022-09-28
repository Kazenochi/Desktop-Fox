using Newtonsoft.Json;
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
    
    public class PictureSet : ObserverNotifyChange
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
        [JsonIgnore]
        public BitmapImage DayImage { 
            get { return DayCol.getPreview(); }
            set
            {
                _dayImage = value;
                RaisePropertyChanged(nameof(DayImage));
            } 
        }
        private BitmapImage _nightImage;
        [JsonIgnore]
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
        private Collection _dayCol;
        public Collection DayCol { get { return _dayCol ?? _nightCol; } set { _dayCol = value; DayImage = value.getPreview(); RaisePropertyChanged(nameof(DayCol)); } }

        /// <summary>
        /// Sammlung von Bilder die wärend der Nacht angezeigt werden
        /// </summary>
        private Collection _nightCol;
        public Collection NightCol { get { return _nightCol ?? _dayCol; } set { _nightCol = value; NightImage = value.getPreview(); RaisePropertyChanged(nameof(NightCol)); } }

        public PictureSet(string name)
        {
            SetName = name;
            //DayImage = ImageHandler.load("F:\\DesktopFoxTestPicture\\Normal\\1.jpg");
            //NightImage = ImageHandler.load("F:\\DesktopFoxTestPicture\\Normal\\2.jpg");
        }
    }
}