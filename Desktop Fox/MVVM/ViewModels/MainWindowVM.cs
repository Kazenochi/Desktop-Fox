using System.Windows.Input;
using DesktopFox.MVVM.Views;
using DesktopFox.MVVM.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Threading.Tasks;
using System.Linq;

namespace DesktopFox
{
    public class MainWindowVM : ObserverNotifyChange
    {
        public AddSetView AddSetView = new AddSetView();
        public Settings_MainView Settings_MainView = new Settings_MainView();
        public ContextPopupView ContextPopupView = new ContextPopupView();
        public PreviewView PreviewView = new PreviewView();
        private MainWindow _mainWindow;
        private Fox DF;
        private GalleryManager GM;

        public MainWindowVM(Fox desktopFox)
        {
            DF = desktopFox;        
            MainWindowModel = new MainWindowModel();
            Preview = PreviewView;
            DF.SettingsManager.Settings.PropertyChanged += Settings_PropertyChanged;
        }

        private void Settings_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName.ToString() != nameof(DF.SettingsManager.Settings.DesktopModeSingle)) return;
            
            Task.Run(() => CheckMultiMonitor());
            return;
        }

        public void SetCurrentMain(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
        }

        public MainWindowModel MainWindowModel { get; set; }

        public AnimatedBaseView CurrentView { get { return _currentView; } 
            set 
            {
                _currentView = value;  
                RaisePropertyChanged(nameof(CurrentView));                  
            } 
        }
        private AnimatedBaseView _currentView;

        public UserControl Preview { get { return _preview; } set { _preview = value; RaisePropertyChanged(nameof(Preview)); } }
        private UserControl _preview;

        public PictureVM SelectedVM { get { return _selectedVM; } set { _selectedVM = value; RaisePropertyChanged(nameof(SelectedVM)); } }
        private PictureVM _selectedVM;

        public int SelectedMonitor { get { return _selectedMonitor; } set { _selectedMonitor = value; RaisePropertyChanged(nameof(SelectedMonitor)); } }
        private int _selectedMonitor = 1;

        public bool MultiMonitor { get { return _multiMonitor; } set { _multiMonitor = value; RaisePropertyChanged(nameof(MultiMonitor)); } }
        private bool _multiMonitor;

        public string MultiMonitorContent { get { return _multiMonitorContent; } set { _multiMonitorContent = value; RaisePropertyChanged(nameof(MultiMonitorContent)); } }
        private string _multiMonitorContent;

        public bool CanActivate { get { return _canActivate; } set { _canActivate = value; RaisePropertyChanged(nameof(CanActivate)); } }
        private bool _canActivate = true;

        public PictureView SelectedItem { get { return _selectedItem; } 
            set 
            { 
                _selectedItem = value;
                if (value != null)
                    SelectedVM = (PictureVM)value.DataContext;
                else
                    SelectedVM = null;

                SChange();
                RaisePropertyChanged(nameof(SelectedItem)); 
            } 
        }
        private PictureView _selectedItem;

        public ICommand ActivateSetCommand { get { return new DF_Command.DelegateCommand(o => ActivateSet()); } }
        public ICommand StopSetCommand { get { return new DF_Command.DelegateCommand(o => StopSet()); } }
        public ICommand AddSetViewCommand { get { return new DF_Command.DelegateCommand(o => SwitchViews(AddSetView)); } }
        public ICommand SettingsMainViewCommand { get { return new DF_Command.DelegateCommand(o => SwitchViews(Settings_MainView)); } }
        public ICommand ContextPopupViewCommand { get { return new DF_Command.DelegateCommand(o => SwitchViews(ContextPopupView)); } }
        public ICommand NextMonitorCommand { get { return new DF_Command.DelegateCommand(o => NextMonitor()) ; } }
        public ICommand HideViewCommand { get { return new DF_Command.DelegateCommand(o => SwitchViews(null)); } }
        public ICommand CloseCommand { get { return new DF_Command.DelegateCommand(o => _mainWindow.Hide()); } }
        public ICommand MinimizeCommand { get { return new DF_Command.DelegateCommand(o => _mainWindow.WindowState = WindowState.Minimized); } }
        public ICommand MaximizeCommand { get { return new DF_Command.DelegateCommand(o => MaximizeWindow()); } }

        private void NextMonitor()
        {
            if(SelectedMonitor < 3)
                SelectedMonitor++;
            else
                SelectedMonitor = 1;

            MultiMonitorContent = "Mon. " + SelectedMonitor.ToString();
            SChange();
        }

        private async Task CheckMultiMonitor()
        {
            
            if (DF.SettingsManager.Settings.DesktopModeSingle)
            {
                MultiMonitor = false;
                SelectedMonitor = 1;
                foreach(var i in MainWindowModel._pictureViewVMs)
                {
                    i.pictureSet.IsActive2 = false;
                    i.pictureSet.IsActive3 = false;
                }

                //DF.GalleryManager.DesktopSwitchCheck(true);
                
                return;
            }
            else
            {
                MultiMonitor = true;
                MultiMonitorContent = "Mon. " + SelectedMonitor.ToString();
                foreach(var i in MainWindowModel._pictureViewVMs)
                {
                    if (i.pictureSet.SetName == DF.GalleryManager.Gallery.activeSetsList.ElementAt(1))
                        i.pictureSet.IsActive2 = true;
                    if (i.pictureSet.SetName == DF.GalleryManager.Gallery.activeSetsList.ElementAt(2))
                        i.pictureSet.IsActive3 = true;
                }
            }           
        }

        private void StopSet()
        {
            switch (SelectedMonitor)
            {
                case 1: SelectedVM.pictureSet.IsActive1 = false; break;
                case 2: SelectedVM.pictureSet.IsActive2 = false; break;
                case 3: SelectedVM.pictureSet.IsActive3 = false; break;
            }
            if(GM.stopActiveSet(SelectedVM.pictureSet.SetName, SelectedMonitor))
                ((SettingsVM)Settings_MainView.DataContext).settings.IsRunning = false;

            CanActivate = true;       
        }

        private void ActivateSet()
        {
            if (SelectedVM == null) return;
            if(GM == null) GM = DF.GetGalleryManager();

            switch (SelectedMonitor)
            {
                case 1:
                    foreach (PictureVM i in MainWindowModel._pictureViewVMs)
                    {
                        if (i.pictureSet.SetName == SelectedVM.pictureSet.SetName)
                            i.pictureSet.IsActive1 = true;
                        else
                            i.pictureSet.IsActive1 = false;
                    }
                    break;
                case 2:
                    foreach (PictureVM i in MainWindowModel._pictureViewVMs)
                    {
                        if (i.pictureSet.SetName == SelectedVM.pictureSet.SetName)
                            i.pictureSet.IsActive2 = true;
                        else
                            i.pictureSet.IsActive2 = false;
                    }
                    break;
                case 3:
                    foreach (PictureVM i in MainWindowModel._pictureViewVMs)
                    {
                        if (i.pictureSet.SetName == SelectedVM.pictureSet.SetName)
                            i.pictureSet.IsActive3 = true;
                        else
                            i.pictureSet.IsActive3 = false;
                    }
                    break;
            }
            GM.setActiveSet(SelectedVM.pictureSet.SetName, SelectedMonitor);
            CanActivate = false;
            ((SettingsVM)Settings_MainView.DataContext).settings.IsRunning = true;
        }

        private void MaximizeWindow()
        {
            if(_mainWindow.WindowState == WindowState.Normal)
                _mainWindow.WindowState = WindowState.Maximized;
            else
                _mainWindow.WindowState = WindowState.Normal;
        }

        private void SChange()
        {
            switch (SelectedMonitor)
            {
                case 1:
                    if (DF.GalleryManager.Gallery.activeSetsList.ElementAt(0) == SelectedVM.pictureSet.SetName)
                        CanActivate = false;
                    else
                        CanActivate = true;
                    break;
                case 2:
                    if (DF.GalleryManager.Gallery.activeSetsList.ElementAt(1) == SelectedVM.pictureSet.SetName)
                        CanActivate = false;
                    else
                        CanActivate = true;
                    break;
                case 3:
                    if (DF.GalleryManager.Gallery.activeSetsList.ElementAt(2) == SelectedVM.pictureSet.SetName)
                        CanActivate = false;
                    else
                        CanActivate = true;
                    break;
            }
        

            foreach(var i in MainWindowModel._pictureViewVMs)
            {
                i.pictureSet.IsSelectedDF = false;                
            }
            SelectedVM.pictureSet.IsSelectedDF = true;
            
            if(CurrentView == ContextPopupView)
                ((ContextPopupVM)ContextPopupView.DataContext).ContentChange(SelectedVM);
            if (CurrentView == AddSetView)
                ((AddSetVM)AddSetView.DataContext).ContentChange(SelectedVM);

            ((PreviewVM)Preview.DataContext).ContentChange(SelectedVM);

        }

        private async Task ContentCleanup(double animationTime)
        {
            await Task.Delay((int)animationTime * 1000);
            _currentView = null;
        }

        private void SwitchViews(AnimatedBaseView newView)
        {
            if (newView != null && newView != CurrentView)
            {
                newView.AnimateIn();
                CurrentView = newView;
            }
            else
            {
                CurrentView.AnimateOut();
                Task.Run(() => ContentCleanup(CurrentView.AnimationTime));
            }         
        }
    }

}
