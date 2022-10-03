using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace DesktopFox.MVVM.Model
{
    public class PreviewModel : ObserverNotifyChange
    {

        public BitmapImage ForegroundImage { get { return _foregroundImage; } set { _foregroundImage = value; RaisePropertyChanged(nameof(ForegroundImage)); } }
        private BitmapImage _foregroundImage;

        public BitmapImage BackgroundImage { get { return _backgroundImage; } set { _backgroundImage = value; RaisePropertyChanged(nameof(BackgroundImage)); } }
        private BitmapImage _backgroundImage;

        public Stretch ImageStretch { get { return _imageStretch; } set { _imageStretch = value; RaisePropertyChanged(nameof(ImageStretch)); } }
        private Stretch _imageStretch = Stretch.UniformToFill;

        public bool Day { get { return _day; } set { _day = value; RaisePropertyChanged(nameof(Day)); } }
        private bool _day = true;
    }
}
