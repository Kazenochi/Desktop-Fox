using DesktopFox.MVVM.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static DesktopFox.DF_Command;

namespace DesktopFox.MVVM.ViewModels
{
    public class AddSetVM : ObserverNotifyChange
    {
        public AddSetModel AddSetModel { get; set; }
        private MainWindowVM MWVM { get; set; }
        private GalleryManager GM;
        private List<String> fileList;

        public AddSetVM(MainWindowVM MainWindowViewModel, GalleryManager galleryManager)
        {
            MWVM = MainWindowViewModel;
            GM = galleryManager;
            if(MWVM.SelectedVM != null)
                AddSetModel = new AddSetModel(MWVM.SelectedVM.pictureSet.SetName);
            else
                AddSetModel = new AddSetModel(NewName());         
        }

        public new void ContentChange(PictureVM pictureViewVM)
        {
            AddSetModel.PictureSetName = pictureViewVM.pictureSet.SetName;
        }

        private bool _day;
        public bool Day { get { return _day; } set { _day = value; RaisePropertyChanged(nameof(Day)); } }
        public ICommand OpenFolderDialog { get { return new DelegateCommand(o => OpenFD()); } }
        public ICommand AddNewSet { get { return new DF_Command.DelegateCommand(o => AddNS()); } }
        public ICommand GenerateSetName { get { return new DF_Command.DelegateCommand(o => GetSetName()); } }

        private void AddNS()
        {
            if (AddSetModel.FolderPath != null && AddSetModel.PictureSetName != "Error")
                GM.addSet(AddSetModel.PictureSetName, GalleryManager.makeCollection(fileList, AddSetModel.FolderPath), Day);
  
        }

        private void GetSetName(bool newName = true)
        {
            if(!newName && MWVM.SelectedVM != null)
                AddSetModel.PictureSetName = MWVM.SelectedVM.pictureSet.SetName;
            else
                AddSetModel.PictureSetName = NewName();
        }

        private void OpenFD()
        {
            fileList = DF_FolderDialog.openFolderDialog();
            if(fileList != null)
                AddSetModel.FolderPath = Path.GetDirectoryName(fileList[0]);
        }

        private String NewName()
        {
            String newName;
            bool newNameExists = false;

            for( int i = 1; i < 100; i++)
            {
                newNameExists = false;
                newName = "New Picture Set " + i;
                if(MWVM.MainWindowModel._pictureViewVMs.Count == 0) return newName;

                foreach (var VMs in MWVM.MainWindowModel._pictureViewVMs)
                {
                    if (newName == VMs.pictureSet.SetName)
                        newNameExists = true;
                }
                if (!newNameExists)
                    return newName;
            }
            newName = "Error";
            Debug.WriteLine("Maximaler Anzahl von NewSets erreicht. New Name = " + newName);
            return newName;
        }

        private void NewPS()
        {

        }

    }
}
