
using System.Windows.Input;
using System;
using System.ComponentModel;
using DesktopFox.MVVM.Views;
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


        private Boolean _addSetVisible;
        public Boolean AddSetVisible { get { return _addSetVisible; } set { _addSetVisible = value; RaisePropertyChanged(nameof(AddSetVisible)); } }


        private Boolean _settingsVisible;
        public Boolean SettingsVisible { get { return _settingsVisible; } set { _settingsVisible = value; RaisePropertyChanged(nameof(SettingsVisible)); } }



        public ICommand ToggleAddSetCommand { get { return new DelegateCommand(o => this.AddSetVisible = !this.AddSetVisible); } }
        public ICommand ToggleSettingsCommand { get { return new DelegateCommand(o => this.SettingsVisible = !this.SettingsVisible); } }

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

        public class DelegateCommand : ICommand
        {
            private readonly Predicate<object> _canExecute;
            private readonly Action<object> _execute;

            public event EventHandler CanExecuteChanged;

            public DelegateCommand(Action<object> execute) : this(execute, null)
            {
            }

            public DelegateCommand(Action<object> execute, Predicate<object> canExecute)
            {
                _execute = execute;
                _canExecute = canExecute;
            }

            public bool CanExecute(object parameter)
            {
                if (_canExecute == null)
                {
                    return true;
                }

                return _canExecute(parameter);
            }

            public void Execute(object parameter)
            {
                _execute(parameter);
            }

            public void RaiseCanExecuteChanged()
            {
                if (CanExecuteChanged != null)
                {
                    CanExecuteChanged(this, EventArgs.Empty);
                }
            }
        }

    }
}
