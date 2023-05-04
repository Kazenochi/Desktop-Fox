using System;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Input;

namespace DesktopFox.MVVM.Views
{
    /// <summary>
    /// InfoBox für Zweitstart der Applikation 
    /// </summary>
    public partial class StartupError : Window
    {
        private readonly Timer _timer;
        private readonly double _shutdownTimeSeconds = 10;

        public StartupError()
        {
            InitializeComponent();
            _timer = new Timer(_shutdownTimeSeconds * 1000);
            _timer.Elapsed += Timer_Elapsed;
            _timer.Enabled= true;
        }

        /// <summary>
        /// Sicherheits Timer um Unnötigen Prozess nach abgelaufener Zeit zu beenden
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Timer_Elapsed(object? sender, ElapsedEventArgs e)
        {
            this.Dispatcher.Invoke(new Action(() => { App.Current.Shutdown(); }));
        }

        /// <summary>
        /// Bestätigung zum Beenden der Info Box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Shutdown_Click(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown();
        }

        /// <summary>
        /// Ermöglicht das Bewegen des Fensters wenn die Linke Maustaste gedrückt wurde.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WindowClickAndDrag(object sender, MouseButtonEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
                this.WindowState = WindowState.Normal;

            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
    }
}
