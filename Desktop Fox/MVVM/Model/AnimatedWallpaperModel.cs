using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopFox.MVVM.Model
{
    public class AnimatedWallpaperModel : ObserverNotifyChange
    {
        private string _sourceUri = "";
        public string SourceUri { get { return _sourceUri;} set { _sourceUri = value; RaisePropertyChanged(nameof(SourceUri)); } }

        private bool _isMuted = true;
        public bool IsMuted { get { return _isMuted; } set { _isMuted = value;RaisePropertyChanged(nameof(IsMuted));} }
    }
}
