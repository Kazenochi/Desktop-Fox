using DesktopFox.MVVM.Model;
using LibVLCSharp.Shared;
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
        LibVLC _libVLC;
        LibVLCSharp.Shared.MediaPlayer _mediaPlayer;
        string myUri;
        int myRotation = 0;

        public AnimatedWallpaperView(string mediaUri, int rotation, AnimatedWallpaperModel myModel)
        {        
            myUri = mediaUri;
            myRotation = rotation;
            InitializeComponent();
            this.VideoView.Loaded += VideoView_Loaded;
            Unloaded += Controls_Unloaded;
            myModel.PropertyChanged += MyModel_PropertyChanged;
        }

        private void MyModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            _mediaPlayer.Volume = ((AnimatedWallpaperModel)sender).Volume; 
        }

        private void WallpaperMedia_MediaEnded(object sender, RoutedEventArgs e)
        {    
            ((MediaElement)sender).Play();
        }

        private void VideoView_Loaded(object sender, RoutedEventArgs e)
        {
            if(myRotation == 90) 
            {
                _libVLC = new LibVLC(enableDebugLogs: true, VLCCommands.Transform, VLCCommands.Rotate90, "--crop=9:16");
            }
            else if (myRotation == 180)
            {
                _libVLC = new LibVLC(enableDebugLogs: true, VLCCommands.Transform, VLCCommands.Rotate180);
            }
            else
            {
                _libVLC = new LibVLC(enableDebugLogs: true, "--aspect-ratio=16:9");
            }
            
            _mediaPlayer = new LibVLCSharp.Shared.MediaPlayer(_libVLC);
            _mediaPlayer.Volume = 0;
            

            this.VideoView.MediaPlayer = _mediaPlayer;
            
            using (var media = new Media(_libVLC, new Uri(myUri)))
                this.VideoView.MediaPlayer.Play(media);
        }


        private void Controls_Unloaded(object sender, RoutedEventArgs e)
        {
            _mediaPlayer.Stop();
            _mediaPlayer.Dispose();
            _libVLC.Dispose();
        }
    }
}
