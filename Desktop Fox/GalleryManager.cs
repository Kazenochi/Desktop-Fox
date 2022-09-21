using Desktop_Fox;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
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
    public class GalleryManager : IObservable<Gallery>
    {

        private Gallery _gallery;
        public List<PictureViewVM> _pictureViewVMList = new List<PictureViewVM>();
        private List<IObserver<Gallery>> _observers;
        private IDisposable unsubscribe;

        public GalleryManager()
        {
            _observers = new List<IObserver<Gallery>>();
            _pictureViewVMList = new List<PictureViewVM>();
            _gallery = new Gallery();
        }

        public void addSet(PictureSet pictureSet)
        {
            _gallery.PictureSetList.Add(pictureSet.Name, pictureSet);
            PictureViewVM tmpVM = new PictureViewVM(pictureSet);
            
            this.Subscribe(tmpVM);
            _pictureViewVMList.Add(tmpVM);         
        }

        public void addCollection(Collection collection, String pictureSetName, Boolean day = true)
        {
            if (day)
            {
                _gallery.PictureSetList[pictureSetName].DayCol = collection;
                Notify(_gallery);
            }
        }

        public void Notify(Gallery gallery)
        {
            foreach(var observer in _observers)
            {
                observer.OnNext(gallery);
            }
        }

        public IDisposable Subscribe(IObserver<Gallery> observer)
        {
            _observers.Add(observer);
            return new Unsubscriber(_observers, observer);
            //throw new NotImplementedException();
        }
    }

    public  class Unsubscriber : IDisposable
    {
        private List<IObserver<Gallery>> _observers;
        private IObserver<Gallery> _observer;

        public Unsubscriber(List<IObserver<Gallery>> observers, IObserver<Gallery> observer)
        {
            this._observers = observers;
            this._observer = observer;
        }

        public void Dispose()
        {
            if (_observer != null && _observers.Contains(_observer))
                _observers.Remove(_observer);
        }
    }
}