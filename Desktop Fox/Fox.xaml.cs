using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Windows.ApplicationModel.Contacts.Provider;
using DesktopFox.MVVM.Views;
using System.Collections.Generic;
using System.Diagnostics;
using DesktopFox.MVVM.ViewModels;

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

        /// <summary>
        /// Konstruktor
        /// </summary>
        public Fox()
        {
            InitializeComponent();
            this.Hide();
            notifyIcon = new NotifyIcon(this);

            loadFiles();

            mainWindowVM = new MainWindowVM();
            addSetVM = new AddSetVM(mainWindowVM);
            settingsVM = new SettingsVM(settings);


            GM = new GalleryManager(gallery);
            SM = new SettingsManager(settings);

            readyPictureVMs();
        }

        public MainWindow GetMainWindow()
        {
            if(MW != null)
                return MW;
            return null;
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
            //Note: ACHTUNG
            //Sollte beim Schließen des Fensters ausgefürht werden
            mainWindowVM.MainWindowModel._pictureViews.Clear();
            //Platzhalter

            MW ??= new MainWindow();
            MW.DataContext = mainWindowVM;
            MW.lbPictures.ItemsSource = mainWindowVM.MainWindowModel._pictureViews;

            foreach (var i in mainWindowVM.MainWindowModel._pictureViewVMs)
            {
                var tmpView = new PictureView();
                tmpView.DataContext = i;
                mainWindowVM.MainWindowModel._pictureViews.Add(tmpView);
            }
            MW.lbPictures.Items.Refresh();

            MW.button_Add.DataContext = addSetVM;
            MW.addSetView.DataContext = addSetVM;

            MW.button_Settings.DataContext = settingsVM;
            MW.settingsMainView.DataContext = settingsVM;

            MW.Show();
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            pictureSet.DayImage = ImageHandler.load("F:\\DesktopFoxTestPicture\\HQ\\Day\\79238609_p1.jpg");
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            pictureSet.DayImage = ImageHandler.load("F:\\DesktopFoxTestPicture\\HQ\\Day\\900540.jpg");
        }


        private void Button_Click_3(object sender, RoutedEventArgs e)
        {

        }
    }
}
