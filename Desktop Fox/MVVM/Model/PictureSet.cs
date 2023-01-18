using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using static System.Windows.Forms.DataFormats;

namespace DesktopFox
{
    /// <summary>
    /// Basisobjekt für die Bilder Sets und Model für die <see cref="MVVM.ViewModels.PictureVM"/> Klasse
    /// </summary>
    public class PictureSet : ObserverNotifyChange
    {
        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="name"></param>
        public PictureSet(string name)
        {
            SetName = name;
            //DayImage = ImageHandler.load("F:\\DesktopFoxTestPicture\\Normal\\1.jpg");
            //NightImage = ImageHandler.load("F:\\DesktopFoxTestPicture\\Normal\\2.jpg");
        }

        #region Binding Variablen

        /// <summary>
        /// Name des Bilder Sets
        /// </summary>
        public String SetName { get { return _setName; } set { _setName = value; RaisePropertyChanged(nameof(SetName)); } }
        private String _setName;
        
        /// <summary>
        /// Gibt an ob das Bilder Set aktuell ausgewählt ist. Bindepunkt für Markierung
        /// </summary>        
        public bool IsSelectedDF { get { return _isSelectedDF; } set { _isSelectedDF = value; RaisePropertyChanged(nameof(IsSelectedDF)); } }
        private bool _isSelectedDF = false;
        
        /// <summary>
        /// Gibt an ob das Bilder Set aktuell auf Monitor 1 aktiv ist. Bindepunkt für Markierung
        /// </summary>
        public bool IsActive1 { get { return _isActive1; } set { _isActive1 = value; RaisePropertyChanged(nameof(IsActive1)); } }
        private bool _isActive1 = false;
        
        /// <summary>
        /// Gibt an ob das Bilder Set aktuell auf Monitor 2 aktiv ist. Bindepunkt für Markierung
        /// </summary>
        public bool IsActive2 { get { return _isActive2; } set { _isActive2 = value; RaisePropertyChanged(nameof(IsActive2)); } }
        private bool _isActive2 = false;
        
        /// <summary>
        /// Gibt an ob das Bilder Set aktuell auf Monitor 3 aktiv ist. Bindepunkt für Markierung
        /// </summary>
        public bool IsActive3 { get { return _isActive3; } set { _isActive3 = value; RaisePropertyChanged(nameof(IsActive3)); } }
        private bool _isActive3 = false;

        #endregion

        #region Binding Images

        /// <summary>
        /// Erstes Bild in der Tag Collection. Bindepunkt für SetVorschau in der Listbox
        /// </summary>
        [JsonIgnore]
        public BitmapImage DayImage { 
            get { return DayCol.getPreview() ?? NightCol.getPreview(); }
            set
            {
                _dayImage = value;
                RaisePropertyChanged(nameof(DayImage));
            } 
        }
        private BitmapImage _dayImage;

        /// <summary>
        /// Erstes Bild in der Nacht Collection. Bindepunkt für SetVorschau in der Listbox
        /// </summary>
        [JsonIgnore]
        public BitmapImage NightImage {
            get { return NightCol.getPreview() ?? DayCol.getPreview(); }
            set 
            {
                _nightImage = value;
                RaisePropertyChanged(nameof(NightImage));
            } 
        }
        private BitmapImage _nightImage;

        #endregion

        #region Binding Collections

        /// <summary>
        /// Sammlung von Bilder die wärend des Tages angezeigt werden
        /// </summary>
        public Collection DayCol { get { return _dayCol ?? _nightCol; } 
            set 
            { 
                _dayCol = value; 
                if (value != null) { DayImage = value.getPreview(); } 
                RaisePropertyChanged(nameof(DayCol)); 
            } 
        }
        private Collection _dayCol;

        /// <summary>
        /// Sammlung von Bilder die wärend der Nacht angezeigt werden
        /// </summary>
        public Collection NightCol { get { return _nightCol ?? _dayCol; } 
            set 
            { 
                _nightCol = value; 
                if (value != null) { NightImage = value.getPreview(); } 
                RaisePropertyChanged(nameof(NightCol)); 
            } 
        }
        private Collection _nightCol;

        #endregion

        /// <summary>
        /// Gibt zurück welche Collections in dem Pictureset vorhanden sind.
        /// </summary>
        /// <returns>1=Day, 2=Night, 3=Both, 4=Twins</returns>
        public int ContainsCollections()
        {
            bool hasDay = false;
            bool hasNight = false;

            if (_dayCol != null)
                hasDay = true;
            if (_nightCol != null)
                hasNight = true;

            if (DayCol.folderDirectory == NightCol.folderDirectory)
                return 4;
            if (hasDay && hasNight)
                return 3;
            if (hasNight)
                return 2;
            if (hasDay)
                return 1;

            return 0;
        }

    }
}