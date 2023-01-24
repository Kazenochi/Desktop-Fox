using DesktopFox.Helper;
using DesktopFox;
using DesktopFox.MVVM.Model;
using LibVLCSharp.Shared;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;


namespace DesktopFox.MVVM.Views
{
    /// <summary>
    /// Interaktionslogik für AnimatedWallpaperView.xaml
    /// </summary>
    public partial class AnimatedWallpaperView : UserControl
    {
        LibVLC _libVLC;
        LibVLCSharp.Shared.MediaPlayer _mediaPlayer;
        private bool _playLock = true;
        private string[] _vlcCommands;
        private string _mediaUri;

        public AnimatedWallpaperView() 
        {
            //InitializeComponent();
        }

        public AnimatedWallpaperView(Wallpaper wallpaper)
        {
            InitializeComponent();
            
            _mediaUri = wallpaper.myMediaUri;
            _vlcCommands = VLCCommandBuilder.BuildCommands(wallpaper);

            this.VideoView.Loaded += VideoView_Loaded;
            Unloaded += Controls_Unloaded;
            wallpaper.PropertyChanged += Wallpaper_PropertyChanged;
        }

        private void Wallpaper_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(_mediaPlayer == null) return;

            switch (e.PropertyName)
            {
                case nameof(Wallpaper.Volume):
                    _mediaPlayer.Volume = (int)((Wallpaper)sender).Volume;
                    break;

                case nameof(Wallpaper.PlayPause):
                    if(((Wallpaper)sender).PlayPause == VLCState.Playing)
                    {
                        _mediaPlayer.Play();
                    }
                    else
                    {
                        _mediaPlayer.Pause();
                    }             
                    break;
            }
        }

        private void VideoView_Loaded(object sender, RoutedEventArgs e)
        {
            _playLock = false;
            BuildMedia();
        }

        private void BuildMedia()
        {
            if (_playLock) return;

            _libVLC = new LibVLC(_vlcCommands);
            
            _mediaPlayer = new MediaPlayer(_libVLC)
            {
                Volume = 0
            };

            this.VideoView.MediaPlayer = _mediaPlayer;
            _mediaPlayer.CropGeometry = "16:9"; 
            using var media = new Media(_libVLC, new Uri(_mediaUri));
            this.VideoView.MediaPlayer.Play(media);
        }

        private void Controls_Unloaded(object sender, RoutedEventArgs e)
        {
            try
            {
                _mediaPlayer.Stop();
                _mediaPlayer.Dispose();
                _libVLC.Dispose();
            }
            catch (System.AccessViolationException)
            {
                Debug.WriteLine("Fehler beim Entladen des Mediaplayer Objekts in AnimatedWallpaperView");
            }
        }
    }
}
