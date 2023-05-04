using System;


namespace DesktopFox.MVVM.Model
{
    /// <summary>
    /// Model der <see cref="ViewModels.AddSetVM"/> Klasse
    /// </summary>
    public class AddSetModel : ObserverNotifyChange
    {
        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="Name"></param>
        public AddSetModel(String Name)
        {
            PictureSetName = Name;
        }

        #region Binding Variablen
        /// <summary>
        /// Name des Bildsets das in der UI angezeigt werden soll <see cref="Views.AddSetView.label_ActiveSet"/>
        /// </summary>
        public String PictureSetName { get { return _pictureSetName; } set { _pictureSetName = value; RaisePropertyChanged(nameof(PictureSetName)); } }
        private string _pictureSetName;
        
        /// <summary>
        /// OrdnerPad des Bildsets das in der UI angezeigt werden soll <see cref="Views.AddSetView.label_SelectedFolder"/>
        /// </summary>
        public String FolderPath { get { return _folderPath; } set { _folderPath = value; RaisePropertyChanged(nameof(FolderPath)); } }
        private string _folderPath;
        #endregion
    }
}
