using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Media.Imaging;

namespace Desktop_Fox
{

    public class PictureViewVM : INotifyPropertyChanged, IObserver<PictureSet>
    {
        private IDisposable unsubscriber;

        public PictureViewVM(PictureSet pictureSet)
        {
            PictureSet = pictureSet;
        }

        #region PictureSet Variablen: DayImage, NightImage, Set Name
        private PictureSet _pictureSet;
        public PictureSet PictureSet 
        { 
            get { return _pictureSet; }
            set 
            {
                _pictureSet = value;
                SetName = _pictureSet.Name;
                RaisePropertyChanged(nameof(PictureSet));
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

        public BitmapImage DayImage { get; set; }
        public BitmapImage NightImage { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void RaisePropertyChanged(string propertyName)
        {
            if(PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public void OnCompleted()
        {
            Debug.WriteLine("Datenübertragung für {0} ist abgeschlossen", _setName );
            throw new NotImplementedException();
        }

        public void Subscribe(IObservable<Gallery> provider)
        {
            if(provider != null)
                unsubscriber = provider.Subscribe(this);
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }


        public void OnNext(PictureSet value)
        {     
            //throw new NotImplementedException();
        }
    }
}
