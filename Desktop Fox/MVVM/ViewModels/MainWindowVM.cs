using System.Windows.Input;
using System;
using System.ComponentModel;
using DesktopFox.MVVM.Views;
using DesktopFox;
using WinRT;
using System.Diagnostics;
using DesktopFox.MVVM.ViewModels;
using System.Windows;

namespace DesktopFox
{
    public class MainWindowVM : ObserverNotifyChange
    {
        public AddSetView AddSetView = new AddSetView();
        public Settings_MainView Settings_MainView = new Settings_MainView();
        private MainWindow _mainWindow;

        public MainWindowVM()
        {
            MainWindowModel = new MainWindowModel();
        }

        public void SetCurrentMain(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
        }

        public MainWindowModel MainWindowModel { get; set; }

        private object _currentView;
        public object CurrentView { get { return _currentView; } set { _currentView = value; RaisePropertyChanged(nameof(CurrentView)); } }
        

        private PictureViewVM _selectedVM;
        public PictureViewVM SelectedVM { get { return _selectedVM; } set { _selectedVM = value; RaisePropertyChanged(nameof(SelectedVM)); } }

        private PictureView _selectedItem;
        public PictureView SelectedItem { get { return _selectedItem; } set { _selectedItem = value; SelectedVM = (PictureViewVM)value.DataContext; RaisePropertyChanged(nameof(SelectedItem)); } }

        public ICommand AddSetViewCommand { get { return new DF_Command.DelegateCommand(o => SwitchViews(AddSetView)); } }
        public ICommand SettingsMainViewCommand { get { return new DF_Command.DelegateCommand(o => SwitchViews(Settings_MainView)); } }
        public ICommand CloseCommand { get { return new DF_Command.DelegateCommand(o => _mainWindow.Hide()); } }
        public ICommand MinimizeCommand { get { return new DF_Command.DelegateCommand(o => _mainWindow.WindowState = WindowState.Minimized); } }
        public ICommand MaximizeCommand { get { return new DF_Command.DelegateCommand(o => MaximizeWindow()); } }

        public void MaximizeWindow()
        {
            if(_mainWindow.WindowState == WindowState.Normal)
                _mainWindow.WindowState = WindowState.Maximized;
            else
                _mainWindow.WindowState = WindowState.Normal;
        }

        public void SChange(PictureViewVM selectedVM)
        {
            SelectedVM = selectedVM;
            foreach(var i in MainWindowModel._pictureViewVMs)
            {
                i.pictureSet.IsSelectedDF = false;                
            }
            SelectedVM.pictureSet.IsSelectedDF = true;
        }

        public void SwitchViews(object newView)
        {
            if (newView != CurrentView)
                CurrentView = newView;
            else
                CurrentView = null;
        }
    }

}
