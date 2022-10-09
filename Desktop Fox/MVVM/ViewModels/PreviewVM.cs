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
    /// <summary>
    /// ViewModel der <see cref="Views.PreviewView"/> Klasse
    /// </summary>
    public class PreviewVM : ObserverNotifyChange
    {
        public PreviewModel PreviewModel { get; set; } = new PreviewModel();
        private Fox DF;

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="desktopFox"></param>
        public PreviewVM(Fox desktopFox)
        {
            DF = desktopFox;
            PreviewModel.PropertyChanged += PreviewModel_PropertyChanged;
        }


        #region Kommandos
        /// <summary>
        /// Kommando das das nächste Bild in der Vorschau anzeigt
        /// </summary>
        public ICommand PictureForwardCommand { get { return new DF_Command.DelegateCommand(o => DF.shuffler.previewForward()); } }

        /// <summary>
        /// Kommando das das vorherige Bild in der Vorschau anzeigt
        /// </summary>
        public ICommand PictureBackwardCommand { get { return new DF_Command.DelegateCommand(o => DF.shuffler.previewBackward()); } }

        public ICommand FaderFinishCommand { get { return new DF_Command.DelegateCommand(o => dummy()); } }
        #endregion


        public void PreviewTransition()
        {
            PreviewModel.AnimationStart = true;
        }

        /// <summary>
        /// Note: Auslagern in den Shuffler evtl. Möglich
        /// </summary>
        /// <param name="pictureVM"></param>
        public new void ContentChange(PictureVM pictureVM) 
        {
            DF.shuffler.previewRefresh();
        }
        public void dummy() { }

        /// <summary>
        /// Listener der beim Ändern des Anzeigebildes ausgeführt wird. Aktualisiert das Anzeigebild in der Vorschau. 
        /// Note: Auslagern in den Shuffler evtl. Möglich
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PreviewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.ToString() != "Day") return;

            DF.shuffler.previewRefresh();
        }
    }
}
