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
        /// Setname in der Preview
        /// </summary>
        public string SetName { get { return _setName; } set { _setName = value; RaisePropertyChanged(nameof(SetName)); } }
        private string _setName = "";

        /// <summary>
        /// Zeigt die Menge an bilder der aktuellen Preview an. 
        /// </summary>
        public int PictureCountMax { get { return _pictureCountMax; } set { _pictureCountMax = value; RaisePropertyChanged(nameof(PictureCountMax)); } }
        private int _pictureCountMax = 0;

        /// <summary>
        /// Zeigt das wievielte bild in der Collection gerade angezeigt wird.
        /// </summary>
        public int PictureCountCurrent { get { return _pictureCountCurrent; } set { _pictureCountCurrent = value; RaisePropertyChanged(nameof(PictureCountCurrent)); } }
        private int _pictureCountCurrent = 0;

        /// <summary>
        /// Flag ob in der Vorschau die Tag oder Nachtbilder der Collection angezeigt werden soll
        /// </summary>
        public bool Day 
        { 
            get { return _day; } 
            set 
            { 
                if(_day == value) return;

                _day = value; 
                RaisePropertyChanged(nameof(Day)); 
            } 
        }
        private bool _day = true;

        public bool FaderLock { get { return _faderLock; } set { _faderLock = value; RaisePropertyChanged(nameof(FaderLock)); } }
        private bool _faderLock = false;

        #endregion
    }
}
