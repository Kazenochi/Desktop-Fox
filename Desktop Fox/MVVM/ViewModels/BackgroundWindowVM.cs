using DesktopFox.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopFox.MVVM.ViewModels
{
    class BackgroundWindowVM
    {
        public BackgroundWindowModel BackgroundModel = new BackgroundWindowModel();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="monitor">Auf Welchem Monitor dieses Fenster Angezeigt wird</param>
        /// <param name="monitorHeight"></param>
        /// <param name="monitorWidth"></param>
        public BackgroundWindowVM(int monitor, int monitorHeight, int monitorWidth)
        {
            BackgroundModel.Height = monitorHeight;
            BackgroundModel.Width = monitorWidth;
            BackgroundModel.Monitor = monitor;
        }
    }
}
