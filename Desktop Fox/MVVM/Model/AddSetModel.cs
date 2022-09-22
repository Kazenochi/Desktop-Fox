using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopFox.MVVM.Model
{
    internal class AddSetModel : INotifyPropertyChanged
    {
        private string _pictureSetName;
        public String PictureSetName { get { return _pictureSetName; } 
            set 
            {
                _pictureSetName = value; 
                RaisePropertyChanged(nameof(PictureSetName));
            } 
        }

        private string _folderPath;
        public String FolderPath { get { return _folderPath; }
        set
            {
                _folderPath = value;
                RaisePropertyChanged(nameof(FolderPath));
            }
        }


        public event PropertyChangedEventHandler? PropertyChanged;
        public void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
