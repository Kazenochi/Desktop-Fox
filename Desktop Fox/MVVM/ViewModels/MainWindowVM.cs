﻿using System.Windows.Input;
using System;
using System.ComponentModel;
using DesktopFox.MVVM.Views;
using DesktopFox;
using WinRT;
using System.Diagnostics;
using DesktopFox.MVVM.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace DesktopFox
{
    public class MainWindowVM : ObserverNotifyChange
    {
        public AddSetView AddSetView = new AddSetView();
        public Settings_MainView Settings_MainView = new Settings_MainView();
        public ContextPopupView ContextPopupView = new ContextPopupView();
        public PreviewView PreviewView = new PreviewView();
        private MainWindow _mainWindow;

        public MainWindowVM()
        {
            MainWindowModel = new MainWindowModel();
            Preview = PreviewView;
        }

        public void SetCurrentMain(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
        }

        public MainWindowModel MainWindowModel { get; set; }

        public UserControl CurrentView { get { return _currentView; } set { _currentView = value; RaisePropertyChanged(nameof(CurrentView)); } }
        private UserControl _currentView;

        public UserControl Preview { get { return _preview; } set { _preview = value; RaisePropertyChanged(nameof(Preview)); } }
        private UserControl _preview;

        public PictureVM SelectedVM { get { return _selectedVM; } set { _selectedVM = value; RaisePropertyChanged(nameof(SelectedVM)); } }
        private PictureVM _selectedVM;
     
        public PictureView SelectedItem { get { return _selectedItem; } 
            set 
            { 
                _selectedItem = value;
                if (value != null)
                    SelectedVM = (PictureVM)value.DataContext;
                else
                    SelectedVM = null;

                SChange(SelectedVM);
                RaisePropertyChanged(nameof(SelectedItem)); 
            } 
        }
        private PictureView _selectedItem;


        public ICommand AddSetViewCommand { get { return new DF_Command.DelegateCommand(o => SwitchViews(AddSetView)); } }
        public ICommand SettingsMainViewCommand { get { return new DF_Command.DelegateCommand(o => SwitchViews(Settings_MainView)); } }
        public ICommand ContextPopupViewCommand { get { return new DF_Command.DelegateCommand(o => SwitchViews(ContextPopupView)); } }
        public ICommand HideViewCommand { get { return new DF_Command.DelegateCommand(o => SwitchViews(null)); } }
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

        public void SChange(PictureVM selectedVM)
        {
            SelectedVM = selectedVM;
            foreach(var i in MainWindowModel._pictureViewVMs)
            {
                i.pictureSet.IsSelectedDF = false;                
            }
            SelectedVM.pictureSet.IsSelectedDF = true;
            
            if(CurrentView == ContextPopupView)
                ((ContextPopupVM)ContextPopupView.DataContext).ContentChange(SelectedVM);
            if (CurrentView == AddSetView)
                ((AddSetVM)AddSetView.DataContext).ContentChange(SelectedVM);
        }

        public void SwitchViews(UserControl newView)
        {
            if (newView != null && (newView != CurrentView || newView == ContextPopupView))
                CurrentView = newView;
            else
                CurrentView = null;

        }
    }

}
