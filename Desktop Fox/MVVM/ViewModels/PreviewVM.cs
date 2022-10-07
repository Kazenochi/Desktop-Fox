using DesktopFox.MVVM.Model;
using DesktopFox.MVVM.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace DesktopFox.MVVM.ViewModels
{
    public class PreviewVM : ObserverNotifyChange
    {
        public PreviewModel PreviewModel { get; set; } = new PreviewModel();
        private Fox DF;
        public PreviewVM(Fox desktopFox)
        {
            DF = desktopFox;
            PreviewModel.PropertyChanged += PreviewModel_PropertyChanged;
        }



        public ICommand PictureForwardCommand { get { return new DF_Command.DelegateCommand(o => DF.shuffler.previewForward()); } }
        public ICommand PictureBackwardCommand { get { return new DF_Command.DelegateCommand(o => DF.shuffler.previewBackward()); } }
        public ICommand FaderFinishCommand { get { return new DF_Command.DelegateCommand(o => dummy()); } }



        public void PreviewTransition()
        {
            PreviewModel.AnimationStart = true;
        }

        public new void ContentChange(PictureVM pictureVM) 
        {
            DF.shuffler.previewRefresh();
        }
        public void dummy() { }

        private void PreviewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.ToString() != "Day") return;

            DF.shuffler.previewRefresh();
        }
    }
}
