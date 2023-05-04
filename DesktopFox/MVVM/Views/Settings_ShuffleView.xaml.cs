using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Linq;
using System.Diagnostics;
using System.Windows.Media.Animation;

namespace DesktopFox.MVVM.Views
{
    /// <summary>
    /// Interaktionslogik für Settings_ShuffleView.xaml
    /// </summary>
    public partial class Settings_ShuffleView : AnimatedBaseView
    {

        public Settings_ShuffleView()
        {
            InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            //https://stackoverflow.com/a/5726430
            TextBox textBox = sender as TextBox;
            int iValue = -1;

            if (Int32.TryParse(textBox.Text, out iValue) == false)
            {
                TextChange textChange = e.Changes.ElementAt<TextChange>(0);
                int iAddedLength = textChange.AddedLength;
                int iOffset = textChange.Offset;

                textBox.Text = textBox.Text.Remove(iOffset, iAddedLength);
            }
        }

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Space || 
                !(
                e.Key >= Key.D0 && e.Key <= Key.D9 || 
                e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9 || 
                e.Key == Key.Back || e.Key == Key.Delete ||
                e.Key >= Key.Left && e.Key <= Key.Down)
                ) 
            {
                e.Handled = true;
            }
        }
    }
}
