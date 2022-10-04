using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DesktopFox.MVVM.Model
{
    public class AddSetModel : ObserverNotifyChange
    {
        public AddSetModel(String Name)
        {
            PictureSetName = Name;
        }

        public String PictureSetName { get { return _pictureSetName; } set { _pictureSetName = value; RaisePropertyChanged(nameof(PictureSetName)); } }
        private string _pictureSetName;
        
        public String FolderPath { get { return _folderPath; } set { _folderPath = value; RaisePropertyChanged(nameof(FolderPath)); } }
        private string _folderPath;
    }
}
