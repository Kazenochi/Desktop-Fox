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
    /// <summary>
    /// ViewModel der <see cref="Views.AddSetView"/> Klasse
    /// </summary>
    public class AddSetVM : ObserverNotifyChange
    {
        public AddSetModel AddSetModel { get; set; }
        private MainWindowVM MWVM { get; set; }
        private GalleryManager GM;
        private List<String> fileList;

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="MainWindowViewModel"></param>
        /// <param name="galleryManager"></param>
        public AddSetVM(MainWindowVM MainWindowViewModel, GalleryManager galleryManager)
        {
            MWVM = MainWindowViewModel;
            GM = galleryManager;
            if(MWVM.SelectedVM != null)
                AddSetModel = new AddSetModel(MWVM.SelectedVM.pictureSet.SetName);
            else
                AddSetModel = new AddSetModel(NewName());         
        }

        /// <summary>
        /// Erhält die Benachtrichtigung falls sich das ausgewählte Objekt in der Liste geändert hat. <see cref="MainWindow.lbPictures"/> 
        /// Aktualisiert den Setnamen in der Anzeige
        /// </summary>
        /// <param name="pictureViewVM">Aktuell ausgewähltes Objekt</param>
        public new void ContentChange(PictureVM pictureViewVM)
        {
            AddSetModel.PictureSetName = pictureViewVM.pictureSet.SetName;
        }

        /// <summary>
        /// Flag ob die Bilder als Tages oder als Nacht Collection hinzugefügt werden sollen.
        /// </summary>
        public bool Day { get { return _day; } set { _day = value; RaisePropertyChanged(nameof(Day)); } }
        private bool _day;

        /// <summary>
        /// Kommando für das öffnen des Ordner Dialogs <see cref="DF_FolderDialog.openFolderDialog"/>
        /// </summary>
        public ICommand OpenFolderDialog { get { return new DelegateCommand(o => OpenFD()); } }

        /// <summary>
        /// Kommando für das hinzufügen eines neuen Sets
        /// </summary>
        public ICommand AddNewSet { get { return new DF_Command.DelegateCommand(o => AddNS()); } }

        /// <summary>
        /// Kommando für das erstellen eines neuen Setnamens
        /// </summary>
        public ICommand GenerateSetName { get { return new DF_Command.DelegateCommand(o => GetSetName()); } }

        /// <summary>
        /// Fügt ein neues Set mit den aktuellen Parametern hinzu.
        /// </summary>
        private void AddNS()
        {
            if (AddSetModel.FolderPath != null && AddSetModel.PictureSetName != "Error")
                GM.addSet(AddSetModel.PictureSetName, GalleryManager.makeCollection(fileList, AddSetModel.FolderPath), Day);
        }

        /// <summary>
        /// Zeigt entweder den Namen des Ausgewählten Sets oder einen neuen Namen in der UI an 
        /// </summary>
        /// <param name="newName">Flag ob ein neuer Name erstellt werden soll.</param>
        private void GetSetName(bool newName = true)
        {
            if(!newName && MWVM.SelectedVM != null)
                AddSetModel.PictureSetName = MWVM.SelectedVM.pictureSet.SetName;
            else
                AddSetModel.PictureSetName = NewName();
        }

        /// <summary>
        /// Ruft den Ordnerdialog auf <see cref="DF_FolderDialog.openFolderDialog"/> 
        /// und aktualisiert den Ordnerpfad in der UI <see cref="Views.AddSetView.label_SelectedFolder"/>
        /// </summary>
        private void OpenFD()
        {
            fileList = DF_FolderDialog.openFolderDialog();
            if(fileList != null)
                AddSetModel.FolderPath = Path.GetDirectoryName(fileList[0]);
        }

        /// <summary>
        /// Generiert den nächsten Set Namen der verfügbar ist und gibt diesen zurück
        /// </summary>
        /// <returns></returns>
        private String NewName()
        {
            String newName;
            bool newNameExists = false;

            for( int i = 1; i < 100; i++)
            {
                newNameExists = false;
                newName = "New Picture Set " + i;
                if(MWVM.MainWindowModel._pictureViewVMs.Count == 0) return newName;

                foreach (var VMs in MWVM.MainWindowModel._pictureViewVMs)
                {
                    if (newName == VMs.pictureSet.SetName)
                        newNameExists = true;
                }
                if (!newNameExists)
                    return newName;
            }
            newName = "Error";
            Debug.WriteLine("Maximaler Anzahl von NewSets erreicht. New Name = " + newName);
            return newName;
        }
    }
}
