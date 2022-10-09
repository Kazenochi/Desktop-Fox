using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace DesktopFox.MVVM.Model
{
    /// <summary>
    /// Model der <see cref="ViewModels.PreviewVM"/> Klasse
    /// </summary>
    public class PreviewModel : ObserverNotifyChange
    {
        #region Binding Variablen
        /// <summary>
        /// Bild das im Vordergrund der Vorschau angezeigt werden soll <see cref="Views.PreviewView.ForegroundPreview"/>
        /// </summary>
        public BitmapImage ForegroundImage { get { return _foregroundImage; } set { _foregroundImage = value; RaisePropertyChanged(nameof(ForegroundImage)); } }
        private BitmapImage _foregroundImage;

        /// <summary>
        /// Bild das im Hintergrund der Vorschau angezeigt werden soll <see cref="Views.PreviewView.BackgroundPreview"/>
        /// </summary>
        public BitmapImage BackgroundImage { get { return _backgroundImage; } set { _backgroundImage = value; RaisePropertyChanged(nameof(BackgroundImage)); } }
        private BitmapImage _backgroundImage;

        /// <summary>
        /// Anzeigeart des Vorschaubildes/>
        /// </summary>
        public Stretch ImageStretch { get { return _imageStretch; } set { _imageStretch = value; RaisePropertyChanged(nameof(ImageStretch)); } }
        private Stretch _imageStretch = Stretch.UniformToFill;

        /// <summary>
        /// Flag ob in der Vorschau die Tag oder Nachtbilder der Collection angezeigt werden soll
        /// </summary>
        public bool Day { get { return _day; } set { _day = value; RaisePropertyChanged(nameof(Day)); } }
        private bool _day = true;

        public bool FaderLock { get { return _faderLock; } set { _faderLock = value; RaisePropertyChanged(nameof(FaderLock)); } }
        private bool _faderLock = true;

        public bool AnimationStart { get { return _animationStart; } set { _animationStart = value; RaisePropertyChanged(nameof(AnimationStart)); } }
        private bool _animationStart = false;
        #endregion
    }
}
