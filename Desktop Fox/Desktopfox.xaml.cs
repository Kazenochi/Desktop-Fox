using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Windows.ApplicationModel.Contacts.Provider;
using Desktop_Fox.MVVM.Views;
using System.Collections.Generic;

namespace Desktop_Fox
{
    /// <summary>
    /// Programmstart und Hauptklasse für dieses Programm
    /// </summary>
    public partial class Desktopfox : Window
    {
        //Settings settings;
        Gallery gallery;
        NotifyIcon notifyIcon;
        // VirtuelDesktop virtuelDesktop;
        //Monitor monitor;
        //Shuffler shuffler;

        public MainWindow MW;
        public MainWindowVM MWVM;
        public PictureSet pictureSet;
        public PictureViewVM pictureViewVM = new PictureViewVM();
        public PictureView pictureView;
        public List<PictureView> ViewList = new List<PictureView>();

        /// <summary>
        /// Konstruktor
        /// </summary>
        public Desktopfox()
        {
            InitializeComponent();
            DataContext = this;

            pictureSet = new PictureSet("Placeholder");
            pictureViewVM.reInit(pictureSet);


            pictureView = new PictureView();
            pictureView.DataContext = pictureViewVM;
            PictureContainer.Children.Add(pictureView);          
            
           // DataContext = pictureViewVM;
            LBox.ItemsSource = ViewList;

           // PictureContainer





            gallery = new Gallery();       
        }




        private void Button_Click(object sender, RoutedEventArgs e)
        {
            for(int i = 0; i < 5; i++)
            {
                pictureViewVM = new PictureViewVM();
                pictureSet = new PictureSet("Placeholder");
                pictureViewVM.reInit(pictureSet);

                pictureView = new PictureView();
                pictureView.DataContext = pictureViewVM;
                ViewList.Add(pictureView);
            }
            
            LBox.Items.Refresh();
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
            PictureViewVM tmpVM = (PictureViewVM)((PictureView)LBox.SelectedItem).DataContext;
            pictureSet = tmpVM.pictureSet;
        }
    }
}
