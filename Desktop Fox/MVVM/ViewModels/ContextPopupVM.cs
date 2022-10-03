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
    public class ContextPopupVM : ObserverNotifyChange
    {
        private MainWindowVM MWVM;
        private GalleryManager GM;
        public ContextPopupModel contextModel { get; set; }

        public ContextPopupVM(MainWindowVM mainWindowVM, GalleryManager galleryManager)
        {
            MWVM = mainWindowVM;
            GM = galleryManager;
            contextModel = new ContextPopupModel();
            if (MWVM.SelectedVM != null)
                contextModel.PictureSetName = MWVM.SelectedVM.pictureSet.SetName;

            DeleteLock();
        }

        
        public int DeletionSelect { get { return _deletionSelect; } set { _deletionSelect = value; DeleteLock();  RaisePropertyChanged(nameof(DeletionSelect)); } }
        private int _deletionSelect = 0;
       
        public bool CanDeleteDay { get { return _canDeleteDay; } set { _canDeleteDay = value; RaisePropertyChanged(nameof(CanDeleteDay)); } }
        private bool _canDeleteDay;
   
        public bool CanDeleteNight { get { return _canDeleteNight; } set { _canDeleteNight = value; RaisePropertyChanged(nameof(CanDeleteNight)); } }
        private bool _canDeleteNight;
       
        public bool CanDeleteBoth { get { return _canDeleteBoth; } set { _canDeleteBoth = value; RaisePropertyChanged(nameof(CanDeleteBoth)); } }
        private bool _canDeleteBoth;
        
        public bool CanDelete { get { return _canDelete; } set { _canDelete = value; RaisePropertyChanged(nameof(CanDelete)); } }
        private bool _canDelete = false;

        private void DeleteLock()
        {
            if (_deletionSelect != 0)
                CanDelete = true;
            else
                CanDelete = false;
        }
        private void DeleteValidation()
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


        public ICommand RenameSetCommand { get { return new DF_Command.DelegateCommand(o => RenameSet()); } }
        public ICommand RemoveCommand { get { return new DF_Command.DelegateCommand(o => RemoveValue()); } }

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
        private void RenameSet() 
        {
            GM.RenameSet(MWVM.SelectedVM.pictureSet.SetName, contextModel.PictureSetName);            
        }

        public new void ContentChange(PictureVM pictureViewVM)
        {
            contextModel.PictureSetName = pictureViewVM.pictureSet.SetName;
            DeletionSelect = 0;
            DeleteValidation();
        }

    }
}
