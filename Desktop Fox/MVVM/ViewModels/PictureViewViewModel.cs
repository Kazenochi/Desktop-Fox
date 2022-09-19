using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Media.Imaging;

namespace Desktop_Fox
{
    public class PictureViewViewModel : INotifyPropertyChanged
    {
        public PictureViewViewModel(){ }

        #region PictureSet Variablen: DayImage, NightImage, Set Name
        private PictureSet _pictureSet;
        public PictureSet PictureSet 
        { 
            get { return _pictureSet; }
            set 
            {
                _pictureSet = value;
                DayImage = _pictureSet.DayImage;
                NightImage = _pictureSet.NightImage;
                SetName = _pictureSet.Name;
                RaisePropertyChanged(nameof(PictureSet));
            } 
        }
        private BitmapImage _dayImage;
        public BitmapImage DayImage 
        { 
            get { return _dayImage; }
            set 
            { 
                _dayImage = value; 
                RaisePropertyChanged(nameof(DayImage));
            }
        }

        private BitmapImage _nightImage;
        public BitmapImage NightImage
        {
            get { return _nightImage; }
            set
            {
                _nightImage = value;
                RaisePropertyChanged(nameof(NightImage));
            }
        }

        private String _setName;
        public String SetName
        {
            get { return _setName; }
            set
            {
                _setName = value;
                RaisePropertyChanged(nameof(SetName));
            }
        }
        #endregion

        #region Gallery Variablen: Preview Set, Aktiver Marker 1-3
        private Gallery _gallery;
        public Gallery Gallery 
        { 
            get { return Gallery; }
            set
            {
                _gallery = value;
                IsSelected = (_gallery.selectedSet == _setName);
                RaisePropertyChanged(nameof(Gallery));  
            }
        }

        private Boolean _isSelected;
        public Boolean IsSelected 
        { 
            get { return _isSelected; } 
            set
            {
                _isSelected = value;
                RaisePropertyChanged(nameof(IsSelected));
            } 
        }

        private Boolean _activeMarker1;
        public Boolean ActiveMarker1
        {
            get { return _activeMarker1; }
            set
            {
                _activeMarker1 = value;
                RaisePropertyChanged(nameof(ActiveMarker1));
            }
        }

        private Boolean _activeMarker2;
        public Boolean ActiveMarker2
        {
            get { return _activeMarker2; }
            set
            {
                _activeMarker2 = value;
                RaisePropertyChanged(nameof(ActiveMarker2));
            }
        }

        private Boolean _activeMarker3;
        public Boolean ActiveMarker3
        {
            get { return _activeMarker3; }
            set
            {
                _activeMarker3 = value;
                RaisePropertyChanged(nameof(ActiveMarker3));
            }
        }
        #endregion


        public event PropertyChangedEventHandler? PropertyChanged;
        public void RaisePropertyChanged(string propertyName)
        {
            if(PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
