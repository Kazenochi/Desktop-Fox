using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Windows.ApplicationModel.Contacts.Provider;
using DesktopFox.MVVM.Views;
using System.Collections.Generic;
using System.Diagnostics;
using DesktopFox.MVVM.ViewModels;
using DesktopFox.Base;
using System.ComponentModel;
using System;
using System.Linq;

namespace DesktopFox
{
    /// <summary>
    /// Programmstart und Hauptklasse für dieses Programm
    /// </summary>
    public partial class Fox : Window
    {
        private Gallery gallery;
        private GalleryManager GM;
        private Settings settings;
        private SettingsManager SM;
        private NotifyIcon notifyIcon;
        private MainWindow MW;
        private MainWindowVM mainWindowVM;
        private ContextPopupVM contextPopupVM;
        private PreviewVM previewVM;
        private AddSetVM addSetVM;
        private SettingsVM settingsVM;
        private GalleryShadow shadow;
        private VirtualDesktop vDesk;
        public Shuffler shuffler;

        /// <summary>
        /// Konstruktor
        /// </summary>
        public Fox()
        {
            InitializeComponent();
            this.Hide();
            notifyIcon = new NotifyIcon(this);

            loadFiles();
            shadow = new GalleryShadow(gallery);
            
            vDesk = new VirtualDesktop();
            SM = new SettingsManager(this, settings, vDesk);
            mainWindowVM = new MainWindowVM(this);
            GM = new GalleryManager(this, SM, gallery, shadow, mainWindowVM);         
            
            addSetVM = new AddSetVM(mainWindowVM, GM);
            settingsVM = new SettingsVM(settings);
            contextPopupVM = new ContextPopupVM(mainWindowVM, GM);
            previewVM = new PreviewVM(this);

            shuffler = new Shuffler(mainWindowVM, GM, SM, previewVM, vDesk);

            readyPictureVMs();
        }

        /// <summary>
        /// Gibt die Instanz des Settings Manager zurück
        /// </summary>
        public SettingsManager SettingsManager { get { return SM; } }

        /// <summary>
        /// Gibt die Instanz des Gallery Managers zurück
        /// </summary>
        public GalleryManager GalleryManager { get { return GM; } }

        /// <summary>
        /// Gibt die Instanz des Virtuellen Desktops zurück
        /// </summary>
        public VirtualDesktop VirtualDesktop { get { return vDesk; } }

        /// <summary>
        /// Gibt die Instanz des Shufflers zurück
        /// </summary>
        public Shuffler Shuffler { get { return shuffler; } }

        public MainWindow GetMainWindow()
        {
            if(MW != null)
                return MW;
            return null;
        }

        /// <summary>
        /// Erstmalige Initialisierung der View Models für die Einträge in der Galerie und Model zuweisung der PictureSets
        /// </summary>
        public void readyPictureVMs()
        {
            foreach (var i in gallery.PictureSetList.Values)
            {
                mainWindowVM.MainWindowModel._pictureViewVMs.Add(new PictureVM(i));
            }
        }

        /// <summary>
        /// Laden der Gallery und Settings JSON Dateien
        /// </summary>
        public void loadFiles()
        {
            var tmpGal = DF_Json.loadGallery();
            if (tmpGal == null)
                gallery = new Gallery();
            else
                gallery = tmpGal;

            var tmpSet = DF_Json.loadSettings();
            if(tmpSet == null)
                settings = new Settings();
            else
                settings = tmpSet;
        }

        /// <summary>
        /// Erstellt das Hauptfenster neu oder Ruft diese auf.
        /// </summary>
        public void makeMainWindow()
        {
            if(MW == null) 
            { 
                MW ??= new MainWindow();
                shuffler.mWinHandler(MW);
                //MW.Closing += MW_Closed;
                MW.DataContext = mainWindowVM;
                MW.lbPictures.ItemsSource = mainWindowVM.MainWindowModel._pictureViews;


                //Auslagern nach mainWindowModel
                foreach (var i in mainWindowVM.MainWindowModel._pictureViewVMs)
                {
                    var tmpView = new PictureView();
                    tmpView.DataContext = i;
                    mainWindowVM.MainWindowModel._pictureViews.Add(tmpView);
                }
                MW.lbPictures.Items.Refresh();

                mainWindowVM.AddSetView.DataContext = addSetVM;
                mainWindowVM.Settings_MainView.DataContext = settingsVM;
                mainWindowVM.ContextPopupView.DataContext = contextPopupVM;
                mainWindowVM.PreviewView.DataContext = previewVM;
            }
            mainWindowVM.SetCurrentMain(MW);
            shuffler.startPreviewShuffleTimer();
            if(mainWindowVM.MainWindowModel._pictureViews.Count > 0)
                mainWindowVM.SelectedItem = mainWindowVM.MainWindowModel._pictureViews.ElementAt(0);

            MW.Show();
        }

        private void MW_Closed(object? sender, System.EventArgs e)
        {
            MW.Hide();
            /*
            foreach (var i in mainWindowVM.MainWindowModel._pictureViews)
            {
                i.DataContext = null; 
            }
            mainWindowVM.MainWindowModel._pictureViews.Clear();
            MW.DataContext = null;
            MW.Closed -= MW_Closed;
            MW = null;
            GC.Collect();
            */
        }

        /// <summary>
        /// Cleanupfunktion beim Beenden der Application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Application_Close(object sender, CancelEventArgs e)
        {
            //shuffler.ShufflerStopCleanup();
            //notifyIcon.Dispose();
            //Debugnachrichten werden nicht auf der Konsole ausgegeben
            if (DF_Json.saveFile(gallery) & DF_Json.saveFile(settings))
                Debug.WriteLine("Programm wurde Beendet. Speichern der Daten war erfolgreich");
            else
                Debug.WriteLine("Program wurde Beendet. FEHLER. Daten konnten nicht gespeichert werden.");
        }
    }
}
