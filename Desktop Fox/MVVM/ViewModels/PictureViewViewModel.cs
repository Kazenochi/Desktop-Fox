using System.Collections.Generic;
using System.ComponentModel;

namespace Desktop_Fox
{
    class PictureViewViewModel : INotifyPropertyChanged
    {
        private List<PictureSet> pictureSetList;

        public PictureViewViewModel(List<PictureSet> pictureList)
        {
            pictureSetList = pictureList;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void RaisePropertyChanged(string propertyName)
        {
            if(PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
