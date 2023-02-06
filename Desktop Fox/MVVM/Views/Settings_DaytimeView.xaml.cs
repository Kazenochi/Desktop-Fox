﻿using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace DesktopFox.MVVM.Views
{
    /// <summary>
    /// Interaktionslogik für Settings_DaytimeView.xaml
    /// </summary>
    public partial class Settings_DaytimeView : AnimatedBaseView
    {

        public Settings_DaytimeView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Textbox Eingabebeschränkung. Es werden lediglich Zahlen, Zurück, Entfernen und Pfeiltasten akzeptiert
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space ||
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
