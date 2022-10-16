using DesktopFox.MVVM.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DesktopFox.MVVM.ViewModels
{
    /// <summary>
    /// ViewModel der <see cref="Views.ContextPopupView"/> Klasse
    /// </summary>
    public class ContextPopupVM : ObserverNotifyChange
    {
        private MainWindowVM MWVM;
        private GalleryManager GM;
        public ContextPopupModel contextModel { get; set; }

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="mainWindowVM"></param>
        /// <param name="galleryManager"></param>
        public ContextPopupVM(MainWindowVM mainWindowVM, GalleryManager galleryManager)
        {
            MWVM = mainWindowVM;
            GM = galleryManager;
            contextModel = new ContextPopupModel();
            if (MWVM.SelectedVM != null)
                contextModel.PictureSetName = MWVM.SelectedVM.pictureSet.SetName;

            DeleteLock();
        }

        /// <summary>
        /// Integer Wert für die Auswahl welches Set entfernt werden soll. 
        /// 1 = Tag <see cref="Views.ContextPopupView.radio_Day"/>
        /// 2 = Nacht <see cref="Views.ContextPopupView.radio_Night"/>
        /// 3 = Beide <see cref="Views.ContextPopupView.radio_Both"/>
        /// </summary>
        public int DeletionSelect { get { return _deletionSelect; } set { _deletionSelect = value; DeleteLock();  RaisePropertyChanged(nameof(DeletionSelect)); } }
        private int _deletionSelect = 0;
       
        /// <summary>
        /// Flag ob der Tages Radiobutton auswählbar ist <see cref="Views.ContextPopupView.radio_Day"/>
        /// </summary>
        public bool CanDeleteDay { get { return _canDeleteDay; } set { _canDeleteDay = value; RaisePropertyChanged(nameof(CanDeleteDay)); } }
        private bool _canDeleteDay;

        /// <summary>
        /// Flag ob der Nacht Radiobutton auswählbar ist <see cref="Views.ContextPopupView.radio_Night"/>
        /// </summary>
        public bool CanDeleteNight { get { return _canDeleteNight; } set { _canDeleteNight = value; RaisePropertyChanged(nameof(CanDeleteNight)); } }
        private bool _canDeleteNight;

        /// <summary>
        /// Flag ob der Beide Radiobutton auswählbar ist <see cref="Views.ContextPopupView.radio_Both"/>
        /// </summary>
        public bool CanDeleteBoth { get { return _canDeleteBoth; } set { _canDeleteBoth = value; RaisePropertyChanged(nameof(CanDeleteBoth)); } }
        private bool _canDeleteBoth;

        /// <summary>
        /// Flag ob der Lösch Button Drückbar ist <see cref="Views.ContextPopupView.button_Delete"/>
        /// </summary>
        public bool CanDelete { get { return _canDelete; } set { _canDelete = value; RaisePropertyChanged(nameof(CanDelete)); } }
        private bool _canDelete = false;

        /// <summary>
        /// Funktion die den Löschen Button Sperrt oder Freigibt <see cref="CanDelete"/>
        /// </summary>
        private void DeleteLock()
        {
            if (_deletionSelect != 0)
                CanDelete = true;
            else
                CanDelete = false;
        }

        /// <summary>
        /// Logik für die Radio Buttons. 
        /// Entscheidet anhand der Eigenschaften des Ausgewählten Sets welcher Radio Button auswählbar ist. <see cref="DesktopFox.MainWindowVM.SelectedVM"/>
        /// </summary>
        public void DeleteValidation()
        {
            if (MWVM.SelectedVM == null) return;

            switch(GM.ContainsCollections(MWVM.SelectedVM.pictureSet.SetName))
            {
                case 1: CanDeleteDay = true;
                        CanDeleteNight = false;
                        CanDeleteBoth = false;
                    break;
                case 2: CanDeleteDay = false;
                        CanDeleteNight = true;
                        CanDeleteBoth = false;
                    break;
                case 3: CanDeleteDay = true;
                        CanDeleteNight = true;
                        CanDeleteBoth = true;
                    break;
                case 4: CanDeleteDay = false;
                        CanDeleteNight = false;
                        CanDeleteBoth = true;
                    break;
            }
        }

        /// <summary>
        /// Kommando das das Set Umbenennt
        /// </summary>
        public ICommand RenameSetCommand { get { return new DF_Command.DelegateCommand(o => RenameSet()); } }

        /// <summary>
        /// Kommando das die gewählte Komponente des Sets entfernt
        /// </summary>
        public ICommand RemoveCommand { get { return new DF_Command.DelegateCommand(o => RemoveValue()); } }

        /// <summary>
        /// Entfernt Anhand der Einstellungen eine Collection von dem gewählten Set oder das komplette Set von der Galerie
        /// </summary>
        private void RemoveValue()
        {
            bool tmpDay = false;
            bool deleteAll = false;

            switch (DeletionSelect)
            {
                case 1: tmpDay = true; GM.removeCollection(MWVM.SelectedVM.pictureSet.SetName, true); break;
                case 2: tmpDay = false; GM.removeCollection(MWVM.SelectedVM.pictureSet.SetName, false); break;
                case 3: deleteAll = true; GM.removeCollection(MWVM.SelectedVM.pictureSet.SetName, true, all: true); break;
                case 4: deleteAll = true; GM.removeCollection(MWVM.SelectedVM.pictureSet.SetName, true, all: true); break;
            }
            DeletionSelect = 0;
            DeleteValidation();
        }

        /// <summary>
        /// Benennt das Ausgewählte Set um. Note: Gefährlicher Code, Fehlerfälle werden nicht abgefangen.
        /// </summary>
        private void RenameSet() 
        {
            GM.RenameSet(MWVM.SelectedVM.pictureSet.SetName, contextModel.PictureSetName);            
        }

        /// <summary>
        /// Erhält die Benachtrichtigung falls sich das ausgewählte Objekt in der Liste geändert hat. <see cref="MainWindow.lbPictures"/> 
        /// Aktualisiert den Setnamen in der Anzeige
        /// </summary>
        /// <param name="pictureViewVM">Aktuell ausgewähltes Objekt</param>
        public new void ContentChange(PictureVM pictureViewVM)
        {
            contextModel.PictureSetName = pictureViewVM.pictureSet.SetName;
            DeletionSelect = 0;
            DeleteValidation();
        }

    }
}
