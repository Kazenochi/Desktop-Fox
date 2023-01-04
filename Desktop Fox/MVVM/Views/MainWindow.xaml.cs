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
        private Point mousePoint;
        private bool mouseDown = false;

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
            bool debug = true;
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

            if (removedIndex < targetIndex)
            {
                if (debug) Debug.WriteLine("Action Runter Schieben -> Entfernt: " + removedIndex + " | Ziel: " + (targetIndex + 1));

                ((MainWindowVM)this.DataContext).MainWindowModel._pictureViews.Insert(targetIndex + 1, droppedData);
                ((MainWindowVM)this.DataContext).MainWindowModel._pictureViewVMs.Insert(targetIndex + 1, (PictureVM)droppedData.DataContext);
                #region Debug Ausgabe
                if (debug) Debug.WriteLine("Insert in " + targetIndex + " von View: " + ((PictureView)droppedData).pLabel.Content.ToString());
                if (debug) Debug.WriteLine("Insert in " + targetIndex + " von VM: " + ((PictureVM)droppedData.DataContext).pictureSet.SetName);
                if (debug)
                {
                    Debug.WriteLine("Inhalt der VMS vor Remove");
                    Debug.Write("View Names: ");
                    foreach (var item in ((MainWindowVM)this.DataContext).MainWindowModel._pictureViews)
                    {
                        Debug.Write(item.pLabel.Content.ToString() + ", ");
                    }
                    Debug.WriteLine("");
                    Debug.Write("VM Names: ");
                    foreach (var item in ((MainWindowVM)this.DataContext).MainWindowModel._pictureViewVMs)
                    {
                        Debug.Write(item.pictureSet.SetName + ", ");
                    }
                    Debug.WriteLine("");
                }
                #endregion
                ((MainWindowVM)this.DataContext).MainWindowModel._pictureViews.RemoveAt(removedIndex);
                ((MainWindowVM)this.DataContext).MainWindowModel._pictureViewVMs.RemoveAt(removedIndex);
                #region Debug Ausgabe
                if (debug) Debug.WriteLine("Remove from " + (removedIndex + 1) + " von View: " + ((MainWindowVM)this.DataContext).MainWindowModel._pictureViews[removedIndex].pLabel.Content.ToString());
                if (debug) Debug.WriteLine("Remove from " + (removedIndex + 1) + " von VM: " + ((MainWindowVM)this.DataContext).MainWindowModel._pictureViewVMs[removedIndex].pictureSet.SetName);
                if (debug)
                {
                    Debug.WriteLine("Inhalt der VMS nach Remove");
                    Debug.Write("View Names: ");
                    foreach(var item in ((MainWindowVM)this.DataContext).MainWindowModel._pictureViews)
                    {
                        Debug.Write(item.pLabel.Content.ToString() + ", ");
                    }
                    Debug.WriteLine("");
                    Debug.Write("VM Names: ");
                    foreach(var item in ((MainWindowVM)this.DataContext).MainWindowModel._pictureViewVMs)
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

                if (((MainWindowVM)this.DataContext).MainWindowModel._pictureViews.Count + 1 > removedIndex)
                {
                    ((MainWindowVM)this.DataContext).MainWindowModel._pictureViews.Insert(targetIndex, droppedData);
                    ((MainWindowVM)this.DataContext).MainWindowModel._pictureViewVMs.Insert(targetIndex, (PictureVM)droppedData.DataContext);
                    #region Debug Ausgabe
                    if (debug) Debug.WriteLine("Insert in " + targetIndex + " von View: " + ((PictureView)droppedData).pLabel.Content.ToString());
                    if (debug) Debug.WriteLine("Insert in " + targetIndex + " von VM: " + ((PictureVM)droppedData.DataContext).pictureSet.SetName);
                    if (debug)
                    {
                        Debug.WriteLine("Inhalt der VMS vor Remove");
                        Debug.Write("View Names: ");
                        foreach (var item in ((MainWindowVM)this.DataContext).MainWindowModel._pictureViews)
                        {
                            Debug.Write(item.pLabel.Content.ToString() + ", ");
                        }
                        Debug.WriteLine("");
                        Debug.Write("VM Names: ");
                        foreach (var item in ((MainWindowVM)this.DataContext).MainWindowModel._pictureViewVMs)
                        {
                            Debug.Write(item.pictureSet.SetName + ", ");
                        }
                        Debug.WriteLine("");
                    }
                    #endregion
                    ((MainWindowVM)this.DataContext).MainWindowModel._pictureViews.RemoveAt(removedIndex);
                    ((MainWindowVM)this.DataContext).MainWindowModel._pictureViewVMs.RemoveAt(removedIndex);
                    #region Debug Ausgabe
                    if (debug) Debug.WriteLine("Remove from " + (removedIndex) + " von View: " + ((MainWindowVM)this.DataContext).MainWindowModel._pictureViews[removedIndex-1].pLabel.Content.ToString());
                    if (debug) Debug.WriteLine("Remove from " + (removedIndex) + " von VM: " + ((MainWindowVM)this.DataContext).MainWindowModel._pictureViewVMs[removedIndex-1].pictureSet.SetName);
                    if (debug)
                    {
                        Debug.WriteLine("Inhalt der VMS nach Remove");
                        Debug.Write("View Names: ");
                        foreach (var item in ((MainWindowVM)this.DataContext).MainWindowModel._pictureViews)
                        {
                            Debug.Write(item.pLabel.Content.ToString() + ", ");
                        }
                        Debug.WriteLine("VM Names: ");
                        foreach (var item in ((MainWindowVM)this.DataContext).MainWindowModel._pictureViewVMs)
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
                Debug.WriteLine(" After: Anzahl von Views: " + ((MainWindowVM)this.DataContext).MainWindowModel._pictureViews.Count);
                Debug.WriteLine(" After: Anzahl von VMs: " + ((MainWindowVM)this.DataContext).MainWindowModel._pictureViewVMs.Count);


            }
            #endregion

            ((MainWindowVM)this.DataContext).getGalleryManager().GallerySort();
        }

        #endregion

    }
}
