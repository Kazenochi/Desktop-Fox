using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DesktopFox.MVVM.Model
{
    internal class AddSetModel : ObserverNotifyChange
    {
        public AddSetModel(String Name)
        {
            PictureSetName = Name;
        }

        private string _pictureSetName;
        public String PictureSetName { get { return _pictureSetName; } set { _pictureSetName = value; RaisePropertyChanged(nameof(PictureSetName)); } }

        private string _folderPath;
        public String FolderPath { get { return _folderPath; } set { _folderPath = value; RaisePropertyChanged(nameof(FolderPath)); } }

    }
}
