using System.Windows;
using System.Windows.Data;


namespace DesktopFox.MVVM.Views
{
    /// <summary>
    /// Interaktionslogik für AnimatedWallpaperConfigView.xaml
    /// </summary>
    public partial class AnimatedWallpaperConfigView : AnimatedBaseView
    {
        public AnimatedWallpaperConfigView()
        {
            InitializeComponent();
            Monitor1_ToggleButton.Checked += Monitor1_ToggleButton_Checked;
            Monitor2_ToggleButton.Checked += Monitor2_ToggleButton_Checked;
            Monitor3_ToggleButton.Checked += Monitor3_ToggleButton_Checked;
        }

        private void Monitor1_ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            Video1TargetUpdated(null, null);
        }

        private void Monitor2_ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            Video2TargetUpdated(null, null);
        }

        private void Monitor3_ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            Video3TargetUpdated(null, null);
        }

        private void Video1TargetUpdated(object sender, DataTransferEventArgs e)
        {
            if(this.DataContext == null) return; 

            if(this.Monitor1_ToggleButton.IsChecked == true)
                this.WallpaperMediaMonitor1.Play();
            else
                this.WallpaperMediaMonitor1.Stop();
        }

        private void Video2TargetUpdated(object sender, DataTransferEventArgs e)
        {
            if (this.DataContext == null) return;

            if (this.Monitor2_ToggleButton.IsChecked == true)
                this.WallpaperMediaMonitor2.Play();
            else
                this.WallpaperMediaMonitor2.Stop();
        }

        private void Video3TargetUpdated(object sender, DataTransferEventArgs e)
        {
            if (this.DataContext == null) return;

            if (this.Monitor3_ToggleButton.IsChecked == true)
                this.WallpaperMediaMonitor3.Play();
            else
                this.WallpaperMediaMonitor3.Stop();
        }
    }
}
