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
        private PictureSet pictureSet;
        private PictureViewVM pictureViewVM;
        private AddSetVM addSetVM;
        private SettingsVM settingsVM;
        private GalleryShadow shadow;
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
            mainWindowVM = new MainWindowVM();

            GM = new GalleryManager(gallery, shadow, mainWindowVM);
            SM = new SettingsManager(settings);

            addSetVM = new AddSetVM(mainWindowVM, GM);
            settingsVM = new SettingsVM(settings);

            readyPictureVMs();
        }

        public MainWindow GetMainWindow()
        {
            if(MW != null)
                return MW;
            return null;
        }

        public Gallery GetGallery()
        {
            return gallery;
        }

        public void readyPictureVMs()
        {
            foreach (var i in gallery.PictureSetList.Values)
            {
                mainWindowVM.MainWindowModel._pictureViewVMs.Add(new PictureViewVM(i));
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


        public void makeMainWindow()
        {
            if(MW == null) 
            { 
                MW ??= new MainWindow();
                //MW.Closing += MW_Closed;
                MW.DataContext = mainWindowVM;
                MW.lbPictures.ItemsSource = mainWindowVM.MainWindowModel._pictureViews;

                foreach (var i in mainWindowVM.MainWindowModel._pictureViewVMs)
                {
                    var tmpView = new PictureView();
                    tmpView.DataContext = i;
                    mainWindowVM.MainWindowModel._pictureViews.Add(tmpView);
                }
                MW.lbPictures.Items.Refresh();

                mainWindowVM.AddSetView.DataContext = addSetVM;
                mainWindowVM.Settings_MainView.DataContext = settingsVM;             
            }
            mainWindowVM.SetCurrentMain(MW);
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
