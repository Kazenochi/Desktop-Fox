using CommunityToolkit.Mvvm.ComponentModel;
using Desktop_Fox;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Desktop_Fox
{
    public class MWViewModel : INotifyPropertyChanged
    {
        private List<PictureViewVM> PVVMList;
        public MWViewModel(GalleryManager galleryManager)
        {
            PVVMList = galleryManager._pictureViewVMList;
        }

        public List<PictureViewVM> PVVM { 
            get { return PVVMList; }
            set
            {
                PVVMList = value;
                RaisePropertyChanged(nameof(PVVM));
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
