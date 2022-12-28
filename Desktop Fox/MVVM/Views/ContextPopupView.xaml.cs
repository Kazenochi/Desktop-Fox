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
