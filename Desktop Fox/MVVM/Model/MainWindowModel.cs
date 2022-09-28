using DesktopFox;
using DesktopFox.MVVM.ViewModels;
using DesktopFox.MVVM.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Input;
using Windows.Media.Capture.Frames;

namespace DesktopFox
{
    public class MainWindowModel : ObserverNotifyChange
    {
        public ObservableCollection<PictureView> _pictureViews;
        public List<PictureViewVM> _pictureViewVMs;
        

        public MainWindowModel()
        {
            _pictureViews = new ObservableCollection<PictureView>();
            this._pictureViews.CollectionChanged += new NotifyCollectionChangedEventHandler(PictureViewChanged);
            _pictureViewVMs = new List<PictureViewVM>();

        }

        /// <summary>
        /// Evtl Redundant. aber erstmal bleibt es so stehen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PictureViewChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            RaiseCollectionChanged(nameof(_pictureViews));
        }

        public event CollectionChangeEventHandler? CollectionChanged;
        public void RaiseCollectionChanged(string propertyName)
        {
            if (CollectionChanged != null)
            {
                CollectionChanged(this, new CollectionChangeEventArgs(CollectionChangeAction.Add, propertyName));
                CollectionChanged(this, new CollectionChangeEventArgs(CollectionChangeAction.Remove, propertyName));
            }
               
        }
    }


  
}
