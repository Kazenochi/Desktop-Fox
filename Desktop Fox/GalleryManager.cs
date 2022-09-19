using Desktop_Fox;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Media.Imaging;

namespace Desktop_Fox
{
    /// <summary>
    /// Verwaltet Änderungen und Informationsbeschaffung von der Gallery
    /// </summary>
    public class GalleryManager : INotifyPropertyChanged
    {
        private Gallery _gallery;
        public GalleryManager()
        {
            _gallery = new Gallery(null);
        }

        public BitmapImage addCollection { 
            get { } 
            set
            {
                DayImage = value;
                RaisePropertyChanged(nameof(addCollection));
            } 
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}