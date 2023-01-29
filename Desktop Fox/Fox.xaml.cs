using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Windows.ApplicationModel.Contacts.Provider;
using DesktopFox.MVVM.Views;
using System.Collections.Generic;
using System.Diagnostics;
using DesktopFox.MVVM.ViewModels;
using System.ComponentModel;
using System;
using System.Linq;
using System.Threading;

namespace DesktopFox
{
    /// <summary>
    /// Programmstart und Hauptklasse für dieses Programm
    /// </summary>
    public partial class Fox : Window
    {
        private readonly FileChecker fileChecker;
        private Gallery gallery;
        private readonly GalleryManager GM;
        private Settings settings;
        private readonly SettingsManager SM;
        private MainWindow? MW;
        private readonly MainWindowVM mainWindowVM;
        private readonly ContextPopupVM contextPopupVM;
        private readonly PreviewVM previewVM;
        private readonly AnimatedWallpaperConfigVM animatedWPConfigVM;
        private readonly AddSetVM addSetVM;
        private readonly SettingsVM settingsVM;
        private readonly GalleryShadow shadow;
        private readonly VirtualDesktop vDesk;
        private WallpaperSaves wallpaperSaves;
        public Shuffler shuffler;
        private bool firstStart = true;

        /// <summary>
        /// Konstruktor
        /// </summary>
        public Fox()
        {
            InitializeComponent();
            this.Hide();
            NotifyIcon notifyIcon = new NotifyIcon(this);
            fileChecker = new FileChecker();          
            LoadFiles();
            shadow = new GalleryShadow(gallery);

            vDesk = new VirtualDesktop(wallpaperSaves: wallpaperSaves);
            SM = new SettingsManager(this, settings, vDesk);
            mainWindowVM = new MainWindowVM(this);
            GM = new GalleryManager(this, SM, gallery, shadow, mainWindowVM);

            addSetVM = new AddSetVM(mainWindowVM, GM);
            settingsVM = new SettingsVM(settings);
            contextPopupVM = new ContextPopupVM(mainWindowVM, GM);
            previewVM = new PreviewVM(this);
            animatedWPConfigVM = new AnimatedWallpaperConfigVM(vDesk);

            shuffler = new Shuffler(mainWindowVM, GM, SM, previewVM, vDesk);

            ReadyPictureVMs();
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
        /// Gibt die gespeicherten Hintergrundbilder zurück
        /// </summary>
        public WallpaperSaves WallpaperSaves { get { return wallpaperSaves; } }

        /// <summary>
        /// Gibt die Instanz des Shufflers zurück
        /// </summary>
        public Shuffler Shuffler { get { return shuffler; } }

        /// <summary>
        /// Gibt die Instanz des Hauptfensters zurück
        /// </summary>
        /// <returns></returns>
        public MainWindow GetMainWindow()
        {
            if(MW != null) return MW;
            
            return null;
        }

        /// <summary>
        /// Erstmalige Initialisierung der View Models für die Einträge in der Galerie und Model zuweisung der PictureSets
        /// </summary>
        public void ReadyPictureVMs()
        {
            foreach (var i in gallery.PictureSetList.Values)
            {
                mainWindowVM.MainWindowModel._pictureViewVMs.Add(new PictureVM(i));
            }
        }

        /// <summary>
        /// Laden der Gallery und Settings JSON Dateien
        /// </summary>
        public void LoadFiles()
        {
            var tmpGal = DF_Json.loadFile(SaveFileType.Gallery);
            if (tmpGal == null)
                gallery = new Gallery();
            else
                gallery = fileChecker.FullCheck((Gallery)tmpGal);

            var tmpSet = DF_Json.loadFile(SaveFileType.Settings);
            if(tmpSet == null)
                settings = new Settings();
            else
                settings = (Settings)tmpSet;

            var tmpWPs = DF_Json.loadFile(SaveFileType.Wallpaper);
            if (tmpWPs != null && ((WallpaperSaves)tmpWPs).wallpapers.Count >= 0)
                wallpaperSaves = (WallpaperSaves)tmpWPs;
        }

        /// <summary>
        /// Erstellt das Hauptfenster neu oder Ruft diese auf.
        /// </summary>
        public void MakeMainWindow()
        {
            //Sicherung falls Fenster Aufgerufen wird bevor der Shuffler initialisiert wurde
            if (shuffler == null) return;

            if(MW == null) 
            { 
                MW ??= new MainWindow();
                shuffler.MWinHandler(MW);
                MW.Closed += MW_Closed;
                MW.DataContext = mainWindowVM;
                
    
                if(firstStart)
                {
                    //Auslagern nach mainWindowModel
                    foreach (var i in mainWindowVM.MainWindowModel._pictureViewVMs)
                    {
                        var tmpView = new PictureView();
                        tmpView.DataContext = i;
                        mainWindowVM.MainWindowModel._pictureViews.Add(tmpView);
                    }
                    
                    mainWindowVM.AddSetView.DataContext = addSetVM;
                    mainWindowVM.Settings_MainView.DataContext = settingsVM;
                    mainWindowVM.ContextPopupView.DataContext = contextPopupVM;
                    mainWindowVM.PreviewView.DataContext = previewVM;
                    animatedWPConfigVM.CheckSavedWallpapers();
                    mainWindowVM.AnimatedWPConfigView.DataContext = animatedWPConfigVM;
                }

            }
            mainWindowVM.SetCurrentMain(MW);
            shuffler.StartPreviewShuffleTimer();

            firstStart = false;
            MW.Show();
        }

        /// <summary>
        /// Funktion für das schließen des Haupfensters. Nicht benötigte Elemente werden entbunden
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MW_Closed(object? sender, System.EventArgs e)
        {
            //MW.Hide();
            if (MW == null) return;

            mainWindowVM.CurrentView = null;
            SaveOnClose(lastClose: false);
            
            foreach (var i in mainWindowVM.MainWindowModel._pictureViews)
            {
                i.DataContext = null; 
            }
            mainWindowVM.MainWindowModel._pictureViews.Clear();
            
            mainWindowVM.SetCurrentMain(null);
            shuffler.MWinHandler(null);
            MW.DataContext = null;
            MW.Closed -= MW_Closed;
            MW = null;
            GC.Collect();
        }

        /// <summary>
        /// Hilfsmethode zum Speichern der Dateien beim Beenden des Programms oder dem Schließen des Hauptfensters
        /// </summary>
        /// <param name="lastClose"></param>
        private void SaveOnClose(bool lastClose = true)
        {

            if (vDesk.getWallpapers != null && vDesk.getWallpapers.Count > 0)
            {
                wallpaperSaves ??= new();
                wallpaperSaves.wallpapers = vDesk.getWallpapers;
                if (DF_Json.saveFile(wallpaperSaves))
                    Debug.WriteLine("App Close. Animierte Wallpaper gespeichert");
                else
                    Debug.WriteLine("App Close. Fehler beim Speichern von Animierten Wallpapern");
            }
            else
            {
                DF_Json.deleteFile(SaveFileType.Wallpaper);
            }

            if (DF_Json.saveFile(gallery) & DF_Json.saveFile(settings))
                Debug.WriteLine("App Close. Speichern der Daten war erfolgreich");
            else
                Debug.WriteLine("App Close. FEHLER. Daten konnten nicht gespeichert werden.");

            if (lastClose)
            {
                vDesk.clearWallpapers(lastClean: lastClose);
            }
        }

        /// <summary>
        /// Cleanupfunktion beim Beenden der Application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Application_Close(object sender, CancelEventArgs e)
        {   if (MW != null)
                MW.ClosingStoryboardFinished(null, null);
            SaveOnClose();
        }
    }
}
