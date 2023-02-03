using DesktopFox.MVVM.ViewModels;
using DesktopFox.MVVM.Views;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;


namespace DesktopFox
{
    /// <summary>
    /// Model der <see cref="MainWindowVM"/> Klasse
    /// </summary>
    public class MainWindowModel : ObserverNotifyChange
    {
        public ObservableCollection<PictureView> _pictureViews { get; set; }
        public ObservableCollection<PictureVM> _pictureViewVMs;
        
        /// <summary>
        /// Konstruktor
        /// </summary>
        public MainWindowModel()
        {
            _pictureViews = new ObservableCollection<PictureView>();
            this._pictureViews.CollectionChanged += new NotifyCollectionChangedEventHandler(PictureViewChanged);
            _pictureViewVMs = new ObservableCollection<PictureVM>();
            this._pictureViewVMs.CollectionChanged += new NotifyCollectionChangedEventHandler(PictureVMChanged);
        }

        private void PictureVMChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            RaiseCollectionChangedVM(nameof(_pictureViewVMs));
        }

        /// <summary>
        /// Evtl Redundant. aber erstmal bleibt es so stehen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PictureViewChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            RaiseCollectionChangedView(nameof(_pictureViews));
        }

        public event CollectionChangeEventHandler? CollectionChangedView;
        public void RaiseCollectionChangedView(string propertyName)
        {
            if (CollectionChangedView != null)
            {
                CollectionChangedView(this, new CollectionChangeEventArgs(CollectionChangeAction.Add, propertyName));
                CollectionChangedView(this, new CollectionChangeEventArgs(CollectionChangeAction.Remove, propertyName));
            }      
        }

        public event CollectionChangeEventHandler? CollectionChangedVM;
        public void RaiseCollectionChangedVM(string propertyName)
        {
            if(CollectionChangedVM != null)
            {
                //CollectionChangedVM(this, new CollectionChangeEventArgs(CollectionChangeAction.Add, propertyName));     Note:
                CollectionChangedVM(this, new CollectionChangeEventArgs(CollectionChangeAction.Remove, propertyName));
            }
        }
    }


  
}
