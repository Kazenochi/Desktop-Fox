using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopFox.MVVM.Model
{
    /// <summary>
    /// Model der <see cref="ViewModels.ContextPopupVM"/> Klasse
    /// </summary>
    public class ContextPopupModel : ObserverNotifyChange
    {
        /// <summary>
        /// Name des Bilder Sets das in der UI angezeigt werden soll <see cref="Views.ContextPopupView.pictureSetName"/>
        /// </summary>
        public String PictureSetName { get { return _pictureSetName; } set { _pictureSetName = value; RaisePropertyChanged(nameof(PictureSetName)); } }
        private string _pictureSetName;

    }
}
