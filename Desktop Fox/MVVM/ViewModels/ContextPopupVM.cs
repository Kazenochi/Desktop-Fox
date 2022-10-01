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
                   
        }

        public ICommand VisibilitySwitchCommand { get { return new DF_Command.DelegateCommand(o => dummy()); } }
        private void dummy() { }

        public new void ContentChange(PictureViewVM pictureViewVM)
        {
            contextModel.PictureSetName = pictureViewVM.pictureSet.SetName;
        }
    }
}
