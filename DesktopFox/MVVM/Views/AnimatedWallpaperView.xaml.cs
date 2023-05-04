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
        private int _volume;
        private Wallpaper _wallpaper;

        public AnimatedWallpaperView() 
        {
            //InitializeComponent();
        }

        public AnimatedWallpaperView(Wallpaper wallpaper)
        {
            InitializeComponent();
            
            _mediaUri = wallpaper.myMediaUri;
            _vlcCommands = VLCCommandBuilder.BuildCommands(wallpaper);
            _volume = (int)wallpaper.Volume;
            _wallpaper = wallpaper;
            this.VideoView.Loaded += VideoView_Loaded;
            Unloaded += Controls_Unloaded;
            _wallpaper.PropertyChanged += Wallpaper_PropertyChanged;
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
                    try
                    {
                        if (((Wallpaper)sender).PlayPause == VLCState.Playing)
                        {
                            _mediaPlayer.Play();
                        }
                        else if(((Wallpaper)sender).PlayPause == VLCState.Paused)
                        {
                            _mediaPlayer.Pause();
                        }
                        else if(((Wallpaper)sender).PlayPause == VLCState.Stopped) 
                        {
                            _mediaPlayer.Stop();
                        }
                    }
                    catch (System.AccessViolationException)
                    {
                        Debug.WriteLine("Fehler beim Entladen des Mediaplayer Objekts in AnimatedWallpaperView");
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
                Volume = 0,
                CropGeometry = "16:9"
            };

            this.VideoView.MediaPlayer = _mediaPlayer;     
            using var media = new Media(_libVLC, new Uri(_mediaUri));
            this.VideoView.MediaPlayer.Play(media);
            _mediaPlayer.Volume = _volume;
        }

        public void Controls_Unloaded(object sender, RoutedEventArgs e)
        {
            _wallpaper.PropertyChanged -= Wallpaper_PropertyChanged;
            if (_mediaPlayer == null) return;
            
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
