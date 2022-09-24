
using System.Windows.Input;
using System;
using System.ComponentModel;
using DesktopFox.MVVM.Views;
using DesktopFox;
using WinRT;
using System.Diagnostics;

namespace DesktopFox
{
    public class MainWindowVM : INotifyPropertyChanged
    {

        public MainWindowVM()
        {
            MainWindowModel = new MainWindowModel();
        }

        public MainWindowModel MainWindowModel { get; set; }


        private PictureViewVM _selectedVM;
        public PictureViewVM SelectedVM { get { return _selectedVM; } set { _selectedVM = value; RaisePropertyChanged(nameof(SelectedVM)); } }

        private PictureView _selectedItem;
        public PictureView SelectedItem { get { return _selectedItem; } set { _selectedItem = value; SelectedVM = (PictureViewVM)value.DataContext; RaisePropertyChanged(nameof(SelectedItem)); } }


        public void SChange(PictureViewVM selectedVM)
        {
            SelectedVM = selectedVM;
            foreach(var i in MainWindowModel._pictureViewVMs)
            {
                i.pictureSet.IsSelectedDF = false;                
            }
            SelectedVM.pictureSet.IsSelectedDF = true;
        }


        public event PropertyChangedEventHandler? PropertyChanged;
        public void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

    }

}
