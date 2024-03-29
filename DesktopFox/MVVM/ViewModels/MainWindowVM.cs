﻿using System.Windows.Input;
using DesktopFox.MVVM.Views;
using DesktopFox.MVVM.ViewModels;
using System.Windows;
using System.Threading.Tasks;
using System.Linq;
using System.Windows.Media;
using System.ComponentModel;
using System.Diagnostics;

namespace DesktopFox
{
    /// <summary>
    /// ViewModel der <see cref="MainWindow"/> Klasse
    /// </summary>
    public class MainWindowVM : ObserverNotifyChange
    {
        public AddSetView AddSetView = new();
        public Settings_MainView Settings_MainView = new();
        public ContextPopupView ContextPopupView = new();
        public PreviewView PreviewView = new();
        public AnimatedWallpaperConfigView AnimatedWPConfigView = new();

        private MainWindow? _mainWindow;
        private readonly Fox DF;
        private GalleryManager GM;

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="desktopFox"></param>
        public MainWindowVM(Fox desktopFox)
        {
            DF = desktopFox;
            MainWindowModel = new MainWindowModel();
            
            MainPanelBlur = PreviewView;

            DF.SettingsManager.Settings.PropertyChanged += Settings_PropertyChanged;
            MainWindowModel.CollectionChangedVM += CollectionChanged_Event_VM;  
            Task.Run(() => CheckMultiMonitor());
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
        /// Gibt die View im Haupfenster an die die Vorschaubilder beinhaltet. <see cref="MainWindow.MainPanelContext"/>
        /// </summary>
        public AnimatedBaseView MainPanel { get { return _mainPanel; } set { _mainPanel = value; RaisePropertyChanged(nameof(MainPanel)); } }
        private AnimatedBaseView _mainPanel;

        /// <summary>
        /// Hauptfenster der Preview das im Falle eines Contextswitches verwaschen werden kann. <see cref="MainWindow.MainPanelContextBlur"/>
        /// </summary>
        public AnimatedBaseView MainPanelBlur { get { return _mainPanelBlur; } set { _mainPanelBlur = value; RaisePropertyChanged(nameof(MainPanelBlur)); } }
        private AnimatedBaseView _mainPanelBlur;

        /// <summary>
        /// Wert für die Anzeige der <see cref="MainWindow.hideClickPane"/>. Wird deaktiviert falls ein Kontextfenster in <see cref="MainPanel"/> angezeigt wird
        /// </summary>
        private bool _clickPaneVisible = true;
        public bool ClickPaneVisible { get { return _clickPaneVisible; } set { _clickPaneVisible = value; RaisePropertyChanged(nameof(ClickPaneVisible)); } }

        /// <summary>
        /// Opacity der <see cref="MainPanelBlur"/> Ebene.
        /// </summary>
        public double BlurOpacity { get { return _blurOpacity; } set { _blurOpacity = value; RaisePropertyChanged(nameof(BlurOpacity)); } }
        private double _blurOpacity = 1;

        /// <summary>
        /// Stärke des Verwaschungseffektes der <see cref="MainPanelBlur"/> Ebene
        /// </summary>
        public int BlurStrength { get { return _blurStrength; } set { _blurStrength = value; RaisePropertyChanged(nameof(BlurStrength)); } }
        private int _blurStrength = 1;

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
        /// Marker ob die Info im Hauptfenster angezeigt werden soll.
        /// </summary>
        public bool EmptyInfo { get { return _emptyInfo; } set { _emptyInfo = value; RaisePropertyChanged(nameof(EmptyInfo)); } }
        private bool _emptyInfo = true;

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

        /// <summary>
        /// Kommando um den Context in <see cref="MainPanel"/> zu ändern
        /// </summary>
        public ICommand AnimatedToggleCommand { get { return new DF_Command.DelegateCommand(o => SwitchMainPanel()); } }

        #endregion

        #region Methoden

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
        /// Event das bei einer Leeren Gallerie Den InfoText Anzeigt
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CollectionChanged_Event_VM(object sender, CollectionChangeEventArgs e)
        {
            if (MainWindowModel._pictureViewVMs.Count == 0)
            {
                EmptyInfo = true;
                //Cleanup der Vorschau beim entfernen des letzten Sets
                if (PreviewView != null && PreviewView.DataContext != null)
                {
                    ((PreviewVM)PreviewView.DataContext).PreviewModel.ForegroundImage = null;
                    ((PreviewVM)PreviewView.DataContext).PreviewModel.BackgroundImage = null;
                }
            }
            else
                EmptyInfo = false;
            return;
        }

        /// <summary>
        /// Aktualisiert die zuweisung des Hauptfensters
        /// </summary>
        /// <param name="mainWindow"></param>
        public void SetCurrentMain(MainWindow? mainWindow)
        {
            _mainWindow = mainWindow;
            ((PreviewVM)PreviewView.DataContext).PreviewModel.ImageStretch = (Stretch)DF.SettingsManager.Settings.PreviewFillMode;
            if (MainWindowModel._pictureViews.Count > 0)
                SelectedItem = MainWindowModel._pictureViews.ElementAt(0);
        }

        /// <summary>
        /// Wählt den nächsten monitor aus. Note: nicht richtig. sollte unterscheiden ob es 2 oder 3 monitore gibt
        /// </summary>
        private void NextMonitor()
        {
            if(SelectedMonitor < DF.VirtualDesktop.getMonitorCount())
                SelectedMonitor++;
            else
                SelectedMonitor = 1;

            MultiMonitorContent = SelectedMonitor.ToString();
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
                MultiMonitorContent = SelectedMonitor.ToString();
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
            GM ??= DF.GalleryManager;

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
            GM ??= DF.GalleryManager;

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

            //DF.SettingsManager.Settings.IsRunning = false;
            //Debug.WriteLine("Nr2. Settings Running True");
            //DF.SettingsManager.Settings.IsRunning = true;
            //((SettingsVM)Settings_MainView.DataContext).settings.IsRunning = false;
            //((SettingsVM)Settings_MainView.DataContext).settings.IsRunning = true;
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
        public void SChange()
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

            ((PreviewVM)PreviewView.DataContext).ContentChange(SelectedVM);
            
        }

        /// <summary>
        /// Wechselt den Inhalt von <see cref="MainPanel"/> und Stößt die Animationen beim Wechsel an
        /// </summary>
        private void SwitchMainPanel()
        {
            SwitchViews(null);

            if (MainPanel == null)
            {
                MainPanel = this.AnimatedWPConfigView;
                BlurOpacity = 0.2;
                BlurStrength = 20;
                MainPanel.AnimateInSoft();
                ((AnimatedWallpaperConfigVM)((AnimatedWallpaperConfigView)MainPanel).DataContext).ShowVideosOnOpen();
                ClickPaneVisible = false;
            }
            else if (MainPanel == this.AnimatedWPConfigView)
            {
                ((AnimatedWallpaperConfigVM)((AnimatedWallpaperConfigView)MainPanel).DataContext).HideVideosOnClose();
                MainPanel.AnimateOut();
                BlurOpacity = 1;
                BlurStrength = 0;
                _mainPanel = null;
                ClickPaneVisible = true;
            }
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
        public void SwitchViews(AnimatedBaseView? newView)
        {
            if (newView != null && newView != CurrentView)
            {
                if(MainPanel != null) SwitchMainPanel();

                newView.AnimateIn();
                CurrentView = newView;
            }
            else if(CurrentView != null)
            {
                CurrentView.AnimateOut();
                _currentView = null;
            }

            if (CurrentView == ContextPopupView)
                ((ContextPopupVM)ContextPopupView.DataContext).ContentChange(SelectedVM);
            if (CurrentView == AddSetView)
                ((AddSetVM)AddSetView.DataContext).ContentChange(SelectedVM);
        }

        /// <summary>
        /// Helfer Methode Die bei einem Drag & Drop Event in <see cref="MainWindow.lbPictures"/> aufgerufen wird. 
        /// Siehe Ebenfalls <see cref="MainWindow.ListBoxItem_ListBoxDrop(object, DragEventArgs)"/>
        /// Ordnet die Gallerie Liste anhand der neuen Anordnung der Views in der Listbox
        /// </summary>
        /// <param name="pv"></param>
        /// <param name="pvm"></param>
        /// <param name="targetIndex"></param>
        /// <param name="removedIndex"></param>
        public void DropItemEvent(PictureView pv, PictureVM pvm, int targetIndex, int removedIndex)
        {
            bool debug = false;

            if (removedIndex < targetIndex)
            {
                if (debug) Debug.WriteLine("Action Runter Schieben -> Entfernt: " + removedIndex + " | Ziel: " + (targetIndex + 1));

                MainWindowModel._pictureViews.Insert(targetIndex + 1, pv);
                MainWindowModel._pictureViewVMs.Insert(targetIndex + 1, pvm);

                #region Debug Ausgabe
                if (debug) Debug.WriteLine("Insert in " + targetIndex + " von View: " + pv.pLabel.Content.ToString());
                if (debug) Debug.WriteLine("Insert in " + targetIndex + " von VM: " + pvm.pictureSet.SetName);
                if (debug)
                {
                    Debug.WriteLine("Inhalt der VMS vor Remove");
                    Debug.Write("View Names: ");
                    foreach (var item in MainWindowModel._pictureViews)
                    {
                        Debug.Write(item.pLabel.Content.ToString() + ", ");
                    }
                    Debug.WriteLine("");
                    Debug.Write("VM Names: ");
                    foreach (var item in MainWindowModel._pictureViewVMs)
                    {
                        Debug.Write(item.pictureSet.SetName + ", ");
                    }
                    Debug.WriteLine("");
                }
                #endregion

                MainWindowModel._pictureViews.RemoveAt(removedIndex);
                MainWindowModel._pictureViewVMs.RemoveAt(removedIndex);

                #region Debug Ausgabe
                if (debug) Debug.WriteLine("Remove from " + (removedIndex + 1) + " von View: " + MainWindowModel._pictureViews[removedIndex].pLabel.Content.ToString());
                if (debug) Debug.WriteLine("Remove from " + (removedIndex + 1) + " von VM: " + MainWindowModel._pictureViewVMs[removedIndex].pictureSet.SetName);
                if (debug)
                {
                    Debug.WriteLine("Inhalt der VMS nach Remove");
                    Debug.Write("View Names: ");
                    foreach (var item in MainWindowModel._pictureViews)
                    {
                        Debug.Write(item.pLabel.Content.ToString() + ", ");
                    }
                    Debug.WriteLine("");
                    Debug.Write("VM Names: ");
                    foreach (var item in MainWindowModel._pictureViewVMs)
                    {
                        Debug.Write(item.pictureSet.SetName + ", ");
                    }
                    Debug.WriteLine("");
                }
                #endregion

            }
            else
            {

                removedIndex = removedIndex + 1;
                if (debug) Debug.WriteLine("Action Hoch Schieben -> Entfernt: " + removedIndex + " | Ziel: " + targetIndex);

                if (MainWindowModel._pictureViews.Count + 1 > removedIndex)
                {
                    MainWindowModel._pictureViews.Insert(targetIndex, pv);
                    MainWindowModel._pictureViewVMs.Insert(targetIndex, pvm);

                    #region Debug Ausgabe
                    if (debug) Debug.WriteLine("Insert in " + targetIndex + " von View: " + pv.pLabel.Content.ToString());
                    if (debug) Debug.WriteLine("Insert in " + targetIndex + " von VM: " + pvm.pictureSet.SetName);
                    if (debug)
                    {
                        Debug.WriteLine("Inhalt der VMS vor Remove");
                        Debug.Write("View Names: ");
                        foreach (var item in MainWindowModel._pictureViews)
                        {
                            Debug.Write(item.pLabel.Content.ToString() + ", ");
                        }
                        Debug.WriteLine("");
                        Debug.Write("VM Names: ");
                        foreach (var item in MainWindowModel._pictureViewVMs)
                        {
                            Debug.Write(item.pictureSet.SetName + ", ");
                        }
                        Debug.WriteLine("");
                    }
                    #endregion

                    MainWindowModel._pictureViews.RemoveAt(removedIndex);
                    MainWindowModel._pictureViewVMs.RemoveAt(removedIndex);

                    #region Debug Ausgabe
                    if (debug) Debug.WriteLine("Remove from " + (removedIndex) + " von View: " + MainWindowModel._pictureViews[removedIndex - 1].pLabel.Content.ToString());
                    if (debug) Debug.WriteLine("Remove from " + (removedIndex) + " von VM: " + MainWindowModel._pictureViewVMs[removedIndex - 1].pictureSet.SetName);
                    if (debug)
                    {
                        Debug.WriteLine("Inhalt der VMS nach Remove");
                        Debug.Write("View Names: ");
                        foreach (var item in MainWindowModel._pictureViews)
                        {
                            Debug.Write(item.pLabel.Content.ToString() + ", ");
                        }
                        Debug.WriteLine("VM Names: ");
                        foreach (var item in MainWindowModel._pictureViewVMs)
                        {
                            Debug.Write(item.pictureSet.SetName + ", ");
                        }
                        Debug.WriteLine("");
                    }
                    #endregion

                }
            }

            #region Debug Ausgabe
            if (debug)
            {
                Debug.WriteLine(" After: Anzahl von Views: " + MainWindowModel._pictureViews.Count);
                Debug.WriteLine(" After: Anzahl von VMs: " + MainWindowModel._pictureViewVMs.Count);


            }
            #endregion

            DF.GalleryManager.GallerySort();
        }

        #endregion 
    }

}
