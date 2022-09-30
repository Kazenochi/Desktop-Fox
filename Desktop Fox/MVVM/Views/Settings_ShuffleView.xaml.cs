﻿using System;
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
    public partial class Settings_ShuffleView : UserControl
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
    }
}
