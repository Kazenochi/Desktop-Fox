using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopFox.MVVM.Model
{
    public class ContextPopupModel : ObserverNotifyChange
    {
        private string _pictureSetName;
        public String PictureSetName { get { return _pictureSetName; } set { _pictureSetName = value; RaisePropertyChanged(nameof(PictureSetName)); } }

        private int _deletionSelect;
        public int DeletionSelect { get { return _deletionSelect; } set { _deletionSelect = value; RaisePropertyChanged(nameof(DeletionSelect)); } }
    }
}
