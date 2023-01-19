using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DesktopFox.MVVM.Views
{
    /// <summary>
    /// Interaktionslogik für AnimatedWallpaperView.xaml
    /// </summary>
    public partial class AnimatedWallpaperView : UserControl
    {
        public AnimatedWallpaperView()
        {
            InitializeComponent();
            this.WallpaperMedia.Play();
        }

        private void WallpaperMedia_MediaEnded(object sender, RoutedEventArgs e)
        {
            ((MediaElement)sender).Stop();
            ((MediaElement)sender).Play();
        }
    }
}
