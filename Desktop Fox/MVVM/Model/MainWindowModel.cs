using DesktopFox;
using DesktopFox.MVVM.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;

namespace DesktopFox
{
    public class MainWindowModel : INotifyPropertyChanged
    {
        public List<PictureView> _pictureViews;
        public List<PictureViewVM> _pictureViewVMs;
        

        public MainWindowModel()
        {
            _pictureViews = new List<PictureView>();
            _pictureViewVMs = new List<PictureViewVM>();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }


  
}
