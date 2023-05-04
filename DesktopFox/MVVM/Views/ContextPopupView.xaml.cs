using DesktopFox.MVVM.ViewModels;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DesktopFox.MVVM.Views
{
    /// <summary>
    /// Interaktionslogik für ContextPopupView.xaml
    /// </summary>
    public partial class ContextPopupView : AnimatedBaseView
    {
        public ContextPopupView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Helfer Funktion die auf die Enter Taste Reagiert und den Rename Befehl ausführt
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AnimatedBaseView_KeyDown(object sender, KeyEventArgs e)
        {
            //return;
            if (e.Key == Key.Enter)
            {
                this.button_Rename.Focus();
                ((ContextPopupVM)this.DataContext).RenameSetCommand.Execute(null);
            }              
        }
    }
}
