using DesktopFox.MVVM.ViewModels;
using DesktopFox.MVVM.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using DragDropEffects = System.Windows.DragDropEffects;
using DragEventArgs = System.Windows.DragEventArgs;
using MouseEventArgs = System.Windows.Input.MouseEventArgs;

namespace DesktopFox
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int firecount = 0;

        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Ermöglicht das Bewegen des Fensters wenn die Linke Maustaste gedrückt wurde.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WindowClickAndDrag(object sender, MouseButtonEventArgs e)
        {
            if(this.WindowState == WindowState.Maximized)
                this.WindowState = WindowState.Normal;

            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        #region Sehr Wackeliges Drag & Drop Verhalten. Funktioniert, Anzeige der Selected Sets ist jedoch oft falsch

        /// <summary>
        /// Drag Funktionalität für Items in der Listbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListBoxItem_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            ListBoxItem draggedItem;
            PictureView draggedView;
            bool dragged = false;

            if (sender is ListBoxItem && e.LeftButton == MouseButtonState.Pressed)
            {
                draggedItem = sender as ListBoxItem;
                DragDrop.DoDragDrop(draggedItem, draggedItem.DataContext, DragDropEffects.Move);
                draggedItem.IsSelected = true;
                dragged = true;
                //((MainWindowVM)this.DataContext).SChange();
            }

            if(dragged && sender is PictureView && e.LeftButton == MouseButtonState.Released)
            {
                draggedView = sender as PictureView;
                //draggedItem.Focus();
                ((MainWindowVM)this.DataContext).SelectedItem = draggedView;
                dragged = false;
            } 
        }

        /// <summary>
        /// Drop Logik um die Views neu anzuordnen. Ordnet nur wärend der Laufzeit neu an, Note: Beim Speichern muss die Galerie angepasst werden
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListBoxItem_ListBoxDrop(object sender, DragEventArgs e)
        {
            bool debug = true;
            firecount++;
            Debug.WriteLine("---FireCount: " + firecount);

            PictureView droppedData = e.Data.GetData(typeof(PictureView)) as PictureView;
            PictureView target = ((ListBoxItem)(sender)).DataContext as PictureView;

            int removedIndex = lbPictures.Items.IndexOf(droppedData);
            int targetIndex = lbPictures.Items.IndexOf(target);

            if (debug)
            {
                Debug.WriteLine(" Before: Anzahl von Views: " + ((MainWindowVM)this.DataContext).MainWindowModel._pictureViews.Count);
                Debug.WriteLine(" Before: Anzahl von VMs: " + ((MainWindowVM)this.DataContext).MainWindowModel._pictureViewVMs.Count);
            }

            if (removedIndex < targetIndex)
            {
                if (debug) Debug.WriteLine("Action Runter Schieben -> Entfernt: " + removedIndex + " | Ziel: " + targetIndex);

                ((MainWindowVM)this.DataContext).MainWindowModel._pictureViews.Insert(targetIndex + 1, droppedData);
                ((MainWindowVM)this.DataContext).MainWindowModel._pictureViewVMs.Insert(targetIndex + 1, (PictureVM)((MainWindowVM)this.DataContext).MainWindowModel._pictureViews[targetIndex].DataContext);
                
                ((MainWindowVM)this.DataContext).MainWindowModel._pictureViews.RemoveAt(removedIndex);
                ((MainWindowVM)this.DataContext).MainWindowModel._pictureViewVMs.RemoveAt(targetIndex);
            }
            else
            {
                
                removedIndex = removedIndex + 1;
                if (debug) Debug.WriteLine("Action Hoch Schieben -> Entfernt: " + removedIndex + " | Ziel: " + targetIndex);

                if (((MainWindowVM)this.DataContext).MainWindowModel._pictureViews.Count + 1 > removedIndex)
                {
                    ((MainWindowVM)this.DataContext).MainWindowModel._pictureViews.Insert(targetIndex, droppedData);
                    ((MainWindowVM)this.DataContext).MainWindowModel._pictureViewVMs.Insert(targetIndex, (PictureVM)((MainWindowVM)this.DataContext).MainWindowModel._pictureViews[targetIndex].DataContext);
                    
                    ((MainWindowVM)this.DataContext).MainWindowModel._pictureViews.RemoveAt(removedIndex);
                    ((MainWindowVM)this.DataContext).MainWindowModel._pictureViewVMs.RemoveAt(removedIndex);
                }
            }

            if (debug)
            {
                Debug.WriteLine(" After: Anzahl von Views: " + ((MainWindowVM)this.DataContext).MainWindowModel._pictureViews.Count);
                Debug.WriteLine(" After: Anzahl von VMs: " + ((MainWindowVM)this.DataContext).MainWindowModel._pictureViewVMs.Count);
            }

            ((MainWindowVM)this.DataContext).getGalleryManager().GallerySort();
        }

        #endregion

    }
}
