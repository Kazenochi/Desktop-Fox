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
    internal class AddSetVM : INotifyPropertyChanged
    {
        public AddSetModel AddSetModel { get; set; }
        private MainWindowVM MWVM { get; set; }
        private GalleryManager GM;
        private List<String> fileList;

        public AddSetVM(MainWindowVM MainWindowViewModel, GalleryManager galleryManager)
        {
            MWVM = MainWindowViewModel;
            GM = galleryManager;
            if(MWVM.SelectedItem != null)
                AddSetModel = new AddSetModel(MWVM.SelectedVM.pictureSet.SetName);
            else
                AddSetModel = new AddSetModel(NewName());         
        }

        private Boolean _addSetVisible = false;
        public Boolean AddSetVisible { get { return _addSetVisible; } set { _addSetVisible = value; GetSetName(newName: false); RaisePropertyChanged(nameof(AddSetVisible)); } }

        private Boolean _day;
        public Boolean Day { get { return _day; } set { _day = value; RaisePropertyChanged(nameof(Day)); } }

        public ICommand ToggleAddSetCommand { get { return new DF_Command.DelegateCommand(o => this.AddSetVisible = !this.AddSetVisible); } }
        public ICommand OpenFolderDialog { get { return new DelegateCommand(o => OpenFD()); } }
        public ICommand AddNewSet { get { return new DF_Command.DelegateCommand(o => AddNS()); } }
        public ICommand GenerateSetName { get { return new DF_Command.DelegateCommand(o => GetSetName()); } }

        private void AddNS()
        {
            if (AddSetModel.FolderPath != null && AddSetModel.PictureSetName != "Error")
                GM.addSet(AddSetModel.PictureSetName, GalleryManager.makeCollection(fileList, AddSetModel.FolderPath), Day);

            
        }

        private void GetSetName(Boolean newName = true)
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
            Boolean newNameExists = false;

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

        public event PropertyChangedEventHandler? PropertyChanged;

        public void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
