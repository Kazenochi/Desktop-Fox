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
using System.Windows.Media.Animation;
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
        private Point mousePoint;
        private bool mouseDown = false;
        private bool CloseLock = true;

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

        #region Drag & Drop Funktion. Note: Nicht langzeitgetestet aber scheint ohne logikfehler zu laufen

        /// <summary>
        /// Drag Funktionalität für Items in der Listbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListBoxItem_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            ListBoxItem draggedItem;
            PictureView draggedView;
            Point mouselocation;

            //Debug.WriteLine("Mouse Move Event Fire");
            //Spezifizieren, wann es sich um einen Drag handelt und wann nur ein klick erfolgt
            //Verhindert unnötiges aktivieren des Drag&Drops und fehler bei der Auswahl
            if (sender is ListBoxItem && e.LeftButton == MouseButtonState.Pressed)
            {
                if (!mouseDown)
                {
                    mouselocation = e.GetPosition(MainBorder);
                    mouseDown = true;
                }
                if(mouselocation != e.GetPosition(MainBorder))
                {
                    draggedItem = sender as ListBoxItem;
                    DragDrop.DoDragDrop(draggedItem, draggedItem.DataContext, DragDropEffects.Move);
                    draggedItem.IsSelected = true;
                }    
                //((MainWindowVM)this.DataContext).SChange();
            }else if(e.LeftButton == MouseButtonState.Released)
            {
                mouseDown = false;
            }
        }

        /// <summary>
        /// Drop Logik um die Views neu anzuordnen. Ordnet nur während der Laufzeit neu an.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListBoxItem_ListBoxDrop(object sender, DragEventArgs e)
        {
            bool debug = false;
            firecount++;
            Debug.WriteLine("---FireCount: " + firecount);

            PictureView droppedData = e.Data.GetData(typeof(PictureView)) as PictureView;
            PictureView target = ((ListBoxItem)(sender)).DataContext as PictureView;

            int removedIndex = lbPictures.Items.IndexOf(droppedData);
            int targetIndex = lbPictures.Items.IndexOf(target);

            #region Debug Ausgabe
            if (debug)
            {
                Debug.WriteLine(" Before: Anzahl von Views: " + ((MainWindowVM)this.DataContext).MainWindowModel._pictureViews.Count);
                Debug.WriteLine(" Before: Anzahl von VMs: " + ((MainWindowVM)this.DataContext).MainWindowModel._pictureViewVMs.Count);
                Debug.WriteLine("DropInfo: ");
                Debug.WriteLine("Dropped Data Type: " + droppedData.GetType().ToString());
                Debug.WriteLine("Dropped Data Context: " + droppedData.DataContext);
                Debug.WriteLine("Dropped Data Context Name Test: " + ((PictureVM)droppedData.DataContext).pictureSet.SetName);
                Debug.WriteLine("Target Type: " + target.GetType().ToString());
                Debug.WriteLine("Target Data Context Name Test: " + ((PictureVM)target.DataContext).pictureSet.SetName);
            }
            #endregion

            ((MainWindowVM)this.DataContext).DropItemEvent(droppedData, (PictureVM)droppedData.DataContext, targetIndex, removedIndex);
        }

        #endregion

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (CloseLock)
            {
                this.Opacity = 1;

                var sb = new Storyboard();
                var fadeinAnimationX = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(0.7));
                sb.Children.Add(fadeinAnimationX);
                Storyboard.SetTarget(sb, this);
                Storyboard.SetTargetProperty(sb, new PropertyPath(System.Windows.Controls.Control.OpacityProperty));

                sb.Completed += ClosingStoryboardFinished;
                sb.Begin();

                e.Cancel = true;
            }
        }

        private void ClosingStoryboardFinished(object sender, EventArgs e)
        {
            CloseLock = false;
            this.Close();
        }
    }
}
