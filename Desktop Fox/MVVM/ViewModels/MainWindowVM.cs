using System.Windows.Input;
using DesktopFox.MVVM.Views;
using DesktopFox.MVVM.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Threading.Tasks;
using System.Linq;
using System.Windows.Media;

namespace DesktopFox
{
    /// <summary>
    /// ViewModel der <see cref="MainWindow"/> Klasse
    /// </summary>
    public class MainWindowVM : ObserverNotifyChange
    {
        public AddSetView AddSetView = new AddSetView();
        public Settings_MainView Settings_MainView = new Settings_MainView();
        public ContextPopupView ContextPopupView = new ContextPopupView();
        public PreviewView PreviewView = new PreviewView();
        private MainWindow _mainWindow;
        private Fox DF;
        private GalleryManager GM;

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="desktopFox"></param>
        public MainWindowVM(Fox desktopFox)
        {
            DF = desktopFox;        
            MainWindowModel = new MainWindowModel();
            Preview = PreviewView;
            DF.SettingsManager.Settings.PropertyChanged += Settings_PropertyChanged;
            Task.Run(() => CheckMultiMonitor());
        }

        /// <summary>
        /// Listener der auf Änderungen in den Einstellungen reagiert
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Settings_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {          
            if (e.PropertyName == nameof(DF.SettingsManager.Settings.PreviewFillMode))
                ((PreviewVM)PreviewView.DataContext).PreviewModel.ImageStretch = (Stretch)DF.SettingsManager.Settings.PreviewFillMode;

            if (e.PropertyName != nameof(DF.SettingsManager.Settings.DesktopModeSingle)) return;

            Task.Run(() => CheckMultiMonitor());
            return;
        }

        /// <summary>
        /// Aktualisiert die zuweisung des Hauptfensters
        /// </summary>
        /// <param name="mainWindow"></param>
        public void SetCurrentMain(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
            ((PreviewVM)PreviewView.DataContext).PreviewModel.ImageStretch = (Stretch)DF.SettingsManager.Settings.PreviewFillMode;
            if(MainWindowModel._pictureViews.Count > 0)
                SelectedItem = MainWindowModel._pictureViews.ElementAt(0);
        }

        /// <summary>
        /// Gibt das Model dieser Klasse zurück
        /// </summary>
        public MainWindowModel MainWindowModel { get; set; }

        #region Binding Variablen
        /// <summary>
        /// Gibt die View im Hauptfenster an die im Context angezeigt werden soll. <see cref="MainWindow.ContextViews"/> 
        /// </summary>
        public AnimatedBaseView CurrentView { get { return _currentView; } 
            set 
            {
                _currentView = value;  
                RaisePropertyChanged(nameof(CurrentView));                  
            } 
        }
        private AnimatedBaseView _currentView;

        /// <summary>
        /// Gibt die View im Haupfenster an die die Vorschaubilder beinhaltet. <see cref="MainWindow.PreviewContext"/>
        /// </summary>
        public UserControl Preview { get { return _preview; } set { _preview = value; RaisePropertyChanged(nameof(Preview)); } }
        private UserControl _preview;

        /// <summary>
        /// Enthält das Objekt das Aktuell in der Listbox des Hauptfensters angezeigt wird. <see cref="MainWindow.lbPictures"/>
        /// </summary>
        public PictureVM SelectedVM { get { return _selectedVM; } set { _selectedVM = value; RaisePropertyChanged(nameof(SelectedVM)); } }
        private PictureVM _selectedVM;

        /// <summary>
        /// Gibt an für welchen Monitor das ausgewählte Set aktiviert werden soll.
        /// </summary>
        public int SelectedMonitor { get { return _selectedMonitor; } set { _selectedMonitor = value; RaisePropertyChanged(nameof(SelectedMonitor)); } }
        private int _selectedMonitor = 1;

        /// <summary>
        /// Flag ob der Button angezeigt werden soll. Wird nur im Multi Modus angezeigt. <see cref="Settings.DesktopModeSingle"/>
        /// </summary>
        public bool MultiMonitor { get { return _multiMonitor; } set { _multiMonitor = value; RaisePropertyChanged(nameof(MultiMonitor)); } }
        private bool _multiMonitor;

        /// <summary>
        /// Anzeige für welchen Monitor das ausgewählte Set aktiviert werden soll. <see cref= "MainWindow.button_ActiveSet" />
        /// </summary>
        public string MultiMonitorContent { get { return _multiMonitorContent; } set { _multiMonitorContent = value; RaisePropertyChanged(nameof(MultiMonitorContent)); } }
        private string _multiMonitorContent;

        /// <summary>
        /// Flag ob das Set Aktiviert werden kann.
        /// </summary>
        public bool CanActivate { get { return _canActivate; } set { _canActivate = value; RaisePropertyChanged(nameof(CanActivate)); } }
        private bool _canActivate = true;

        /// <summary>
        /// Helferklasse die das gespeicherte Viewmodel des ausgewählten Sets aktualisiert und Informiert notwendige Klassen <see cref="SChange"/>
        /// </summary>
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

        #endregion

        #region Kommandos
        /// <summary>
        /// Kommando das das ausgewählte Set aktiviert
        /// </summary>
        public ICommand ActivateSetCommand { get { return new DF_Command.DelegateCommand(o => ActivateSet()); } }

        /// <summary>
        /// Kommando das das ausgewählte Set deaktiviert
        /// </summary>
        public ICommand StopSetCommand { get { return new DF_Command.DelegateCommand(o => StopSet()); } }

        /// <summary>
        /// Kommando das die AddSet View aufzeigt <see cref="MVVM.Views.AddSetView"/> 
        /// </summary>
        public ICommand AddSetViewCommand { get { return new DF_Command.DelegateCommand(o => SwitchViews(AddSetView)); } }

        /// <summary>
        /// Kommando das die Settings View aufzeigt <see cref="MVVM.Views.Settings_MainView"/> 
        /// </summary>
        public ICommand SettingsMainViewCommand { get { return new DF_Command.DelegateCommand(o => SwitchViews(Settings_MainView)); } }

        /// <summary>
        /// Kommando das die ContextPopup View aufzeigt <see cref="MVVM.Views.ContextPopupView"/> 
        /// </summary>
        public ICommand ContextPopupViewCommand { get { return new DF_Command.DelegateCommand(o => ContextSwitch()); } }

        /// <summary>
        /// Kommando das den Nächsten Monitor auswählt
        /// </summary>
        public ICommand NextMonitorCommand { get { return new DF_Command.DelegateCommand(o => NextMonitor()) ; } }

        /// <summary>
        /// Kommando das alle Views versteckt
        /// </summary>
        public ICommand HideViewCommand { get { return new DF_Command.DelegateCommand(o => SwitchViews(null)); } }

        /// <summary>
        /// Kommando das das Hauptfenster schliest
        /// </summary>
        public ICommand CloseCommand { get { return new DF_Command.DelegateCommand(o => _mainWindow.Close()); } }

        /// <summary>
        /// Kommando das das Hauptfenster minimiert
        /// </summary>
        public ICommand MinimizeCommand { get { return new DF_Command.DelegateCommand(o => _mainWindow.WindowState = WindowState.Minimized); } }

        /// <summary>
        /// Kommando das das Hauptfenster maximiert
        /// </summary>
        public ICommand MaximizeCommand { get { return new DF_Command.DelegateCommand(o => MaximizeWindow()); } }
        #endregion

        #region Methoden
        /// <summary>
        /// Wählt den nächsten monitor aus. Note: nicht richtig. sollte unterscheiden ob es 2 oder 3 monitore gibt
        /// </summary>
        private void NextMonitor()
        {
            if(SelectedMonitor < DF.VirtualDesktop.getMonitorCount())
                SelectedMonitor++;
            else
                SelectedMonitor = 1;

            MultiMonitorContent = "Mon. " + SelectedMonitor.ToString();
            SChange();
        }

        /// <summary>
        /// Überprüft bei Änderungen des Desktop Modus die Einstellungen und Passt die Anzeige der Bilder Marker an 
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Stoppt das Set für den gewählten Monitor und passt die Bild Marker an
        /// </summary>
        private void StopSet()
        {
            switch (SelectedMonitor)
            {
                case 1: SelectedVM.pictureSet.IsActive1 = false; break;
                case 2: SelectedVM.pictureSet.IsActive2 = false; break;
                case 3: SelectedVM.pictureSet.IsActive3 = false; break;
            }
            GM.stopActiveSet(monitor: SelectedMonitor);

            CanActivate = true;       
        }

        /// <summary>
        /// Aktiviert das gewählte Set
        /// </summary>
        private void ActivateSet()
        {
            if (SelectedVM == null) return;
            if(GM == null) GM = DF.GalleryManager;

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

            ((SettingsVM)Settings_MainView.DataContext).settings.IsRunning = false;
            ((SettingsVM)Settings_MainView.DataContext).settings.IsRunning = true;
        }

        /// <summary>
        /// Maximiert das Hauptfenster oder setzt dieses wieder auf Normal zurück, falls es bereits maximiert ist.
        /// </summary>
        private void MaximizeWindow()
        {
            if(_mainWindow.WindowState == WindowState.Normal)
                _mainWindow.WindowState = WindowState.Maximized;
            else
                _mainWindow.WindowState = WindowState.Normal;
        }

        /// <summary>
        /// Helferklasse die beim Ändern der Auswahl in der Listbox aufgerufen wird. <see cref="MainWindow.lbPictures"/>
        /// Überprüft ob das Set Aktiviert werden kann und Benachrichtig alle ViewModels die die <see cref="ObserverNotifyChange"/> implementieren
        /// </summary>
        private void SChange()
        {
            if (SelectedVM == null) return;

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

        /// <summary>
        /// Entfernt die Views beim schließen mit etwas verzögerung.
        /// Gewährleistet das die Animation abgeschlossen ist, bevor die View geändert wird.
        /// </summary>
        /// <param name="animationTime">Dauer der Animation in Sekunden</param>
        /// <returns></returns>
        private async Task ContentCleanup(double animationTime)
        {
            await Task.Delay((int)animationTime * 1000);
            _currentView = null;
        }

        /// <summary>
        /// Helfermethode für das Context Menu
        /// </summary>
        private void ContextSwitch()
        {
            if (CurrentView == ContextPopupView) return;

            SwitchViews(ContextPopupView);
        }

        /// <summary>
        /// Ändernt die Views im Mainwindow <see cref="MainWindow.ContextViews"/>
        /// </summary>
        /// <param name="newView">Neue View die Angezeigt werden soll. null = keine View anzeigen.</param>
        public void SwitchViews(AnimatedBaseView newView)
        {
            if (newView != null && newView != CurrentView)
            {
                newView.AnimateIn();
                CurrentView = newView;
            }
            else if(CurrentView != null)
            {
                CurrentView.AnimateOut();
                Task.Run(() => ContentCleanup(CurrentView.AnimationTime));
            }

            if (CurrentView == ContextPopupView)
                ((ContextPopupVM)ContextPopupView.DataContext).ContentChange(SelectedVM);
            if (CurrentView == AddSetView)
                ((AddSetVM)AddSetView.DataContext).ContentChange(SelectedVM);
        }
        #endregion 
    }

}
