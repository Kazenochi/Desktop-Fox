using DesktopFox.MVVM.ViewModels;
using DesktopFox.MVVM.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DesktopFox
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MainWindowVM myVM = (MainWindowVM)this.DataContext;
            var item = (ListBox)sender;
            var selected = (PictureView)item.SelectedItem;
            

            if (selected != null)
            {
                var logic = (PictureViewVM)selected.DataContext;
                myVM.SChange(logic);
            }             
        }

        /// <summary>
        /// Ermöglicht das Bewegen des Fensters wenn die Linke Maustaste gedrückt wurde.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WindowClickAndDrag(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Right)
            {
                //contextPopupView.Visibility = Visibility.Hidden;
            }

            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }
    }
}
