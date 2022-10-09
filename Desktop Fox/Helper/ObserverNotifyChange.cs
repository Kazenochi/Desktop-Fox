using DesktopFox.MVVM.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopFox
{
    /// <summary>
    /// Erweiterung des INotifyPropertyChangedInterfaces
    /// </summary>
    public class ObserverNotifyChange : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// TemplateMethode um mögliche Änderungen an andere Objekte zu übergeben
        /// </summary>
        /// <param name="pictureView"></param>
        public void ContentChange(PictureVM pictureView) { }
    }
}
