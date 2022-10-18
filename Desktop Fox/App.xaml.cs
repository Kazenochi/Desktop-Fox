using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace DesktopFox
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static Mutex onlyOneMutex;
        /// <summary>
        /// Verhindert das mehrere Instancen des programms gestartet werden können.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnStartup(StartupEventArgs e)
        {
            Boolean isNew = false;
            onlyOneMutex = new Mutex(true, "Desktop Fox", out isNew);
            if(!isNew)
            {
                //MessageBox.Show("Eine Instance der Appliction läuft bereits");
                Debug.WriteLine("Eine Instance der Application läuft bereits. App wird geschlossen");
                App.Current.Shutdown();
            }
            base.OnStartup(e);
        }
    }
}
