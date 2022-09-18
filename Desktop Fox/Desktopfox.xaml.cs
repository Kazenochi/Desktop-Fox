using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows;


namespace Desktop_Fox
{
    /// <summary>
    /// Programmstart und Hauptklasse für dieses Programm
    /// </summary>
    public partial class Desktopfox : Window
    {
        MainWindow MW;

        /// <summary>
        /// Konstruktor
        /// </summary>
        public Desktopfox()
        {
            InitializeComponent();
            this.Hide();
            MainWindow mainWindow = new MainWindow();
            NotifyIcon notifyIcon = new NotifyIcon(mainWindow);
        }

    }
}
