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
        public AddSetVM(MainWindowVM MainWindowViewModel)
        {
            MWVM = MainWindowViewModel;
            if(MWVM.SelectedItem != null)
                AddSetModel = new AddSetModel(MWVM.SelectedItem.Name);
            else
                AddSetModel = new AddSetModel(NewName());         
        }

        private Boolean _addSetVisible = false;
        public Boolean AddSetVisible { get { return _addSetVisible; } set { _addSetVisible = value; GetSetName(); RaisePropertyChanged(nameof(AddSetVisible)); } }
        public ICommand ToggleAddSetCommand { get { return new DF_Command.DelegateCommand(o => this.AddSetVisible = !this.AddSetVisible); } }
        public ICommand OpenFolderDialog { get { return new DelegateCommand(o => OpenFD()); } }
        public ICommand AddNewSet { get { return new DF_Command.DelegateCommand(o => AddNS()); } }

        private void AddNS()
        {

        }
        private void GetSetName()
        {
            if(MWVM.SelectedItem != null)
                AddSetModel.PictureSetName = MWVM.SelectedItem.Name;
            else
                AddSetModel.PictureSetName = NewName();
        }

        private void OpenFD()
        {
            List<String> fileList = DF_FolderDialog.openFolderDialog();
            if(fileList != null)
                AddSetModel.FolderPath = Path.GetDirectoryName(fileList[0]);
        }

        private String NewName()
        {
            String newName;
            for( int i = 1; i < 100; i++)
            {
                newName = "New Picture Set " + i;
                foreach (var VMs in MWVM.MainWindowModel._pictureViewVMs)
                {
                    if (newName == VMs.pictureSet.Name)
                        continue;
                    else
                        return newName;
                }
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
