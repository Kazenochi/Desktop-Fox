

namespace DesktopFox.MVVM.Model
{
    public class AnimatedWallpaperModel : ObserverNotifyChange
    {
        private string _sourceUri = "";
        public string SourceUri { get { return _sourceUri;} set { _sourceUri = value; RaisePropertyChanged(nameof(SourceUri)); } }

        private bool _isMuted = true;
        public bool IsMuted { get { return _isMuted; } set { _isMuted = value;RaisePropertyChanged(nameof(IsMuted));} }

        private int _rotation = 0;
        public int Rotation { get { return _rotation;} set { _rotation = value;RaisePropertyChanged(nameof(Rotation)); } }
    }
}
