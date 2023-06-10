using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;


namespace DesktopFox
{
    /// <summary>
    /// Klasse für Ordnerdialog aufrufe
    /// </summary>
    public class DF_FolderDialog
    {
        /// <summary>
        /// Öffnet einen neuen Ordnerdialog
        /// </summary>
        /// <returns>String Liste mit Dateinamen</returns>
        public static List<String> openFolderDialog()
        {
            FolderBrowserDialog folderDialog = new System.Windows.Forms.FolderBrowserDialog();

            if (folderDialog.ShowDialog() != System.Windows.Forms.DialogResult.OK) 
                return null;

            return getList(getFileInfo(folderDialog.SelectedPath)); 
        }

        /// <summary>
        /// Öffnet einen <see cref="OpenFileDialog"/> um eine Video Datei Auszuwählen
        /// </summary>
        /// <returns>Pfad zur ausgewählten Datei</returns>
        public static string openSingleFileDialog()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Video files (*.mp4,*.mkv,etc.)|*.mp4;*.mkv;*.webm;*.avi;*.flv;*.mov;*.mkv;|All files (*.*)|*.*";
            openFileDialog.Multiselect = false;
            openFileDialog.CheckFileExists = true;
            openFileDialog.CheckPathExists = true;

            string filePath; 
            if(openFileDialog.ShowDialog() == DialogResult.OK)
            {
                filePath = openFileDialog.FileName;
                return filePath;
            }
            return null;
        }

        /// <summary>
        /// Wandelt die Fileinfo Liste in eine Stringliste um
        /// </summary>
        /// <param name="fileInfos">Liste von Datei Informationen</param>
        /// <returns>String Liste mit Dateinamen</returns>
        public static List<String> getList(List<FileInfo> fileInfos)
        {
            List<String> list = new List<String>();
            foreach(FileInfo fileInfo in fileInfos)
            {
                list.Add(fileInfo.FullName);
            }
            return list;
        }

        /// <summary>
        /// Extrahiert alle Bilddateien aus einem Ordner
        /// </summary>
        /// <param name="path">Absoluter Pfad zum Bildordner</param>
        /// <returns>Liste mit Datei Informationen</returns>
        public static List<FileInfo> getFileInfo(String path)
        {
            DirectoryInfo dir = new DirectoryInfo(path);
            FileInfo[] filesArray = dir.GetFiles();
            List<FileInfo> picFiles = new List<FileInfo>();
            foreach (FileInfo file in filesArray)
            {
                String ext = System.IO.Path.GetExtension(file.FullName).ToUpper();
                if (ext == ".PNG" || ext == ".JPG" || ext == ".JPEG" || ext == ".BMP")
                    picFiles.Add(file);
            }
            return picFiles;
        }

    }
}
