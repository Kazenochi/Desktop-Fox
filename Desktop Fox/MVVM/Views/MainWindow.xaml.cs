using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Desktop_Fox
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Pictures = new List<PictureView>();
        }

        public int setCount = 0;
  

        public List<PictureView> Pictures;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            PictureSet PS = new PictureSet("Picture Set " + setCount);
            setCount++;

            PictureSetListBox.ItemsSource = Pictures;
            PictureSetListBox.Items.Refresh();
        }

        private void Button_Make_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
