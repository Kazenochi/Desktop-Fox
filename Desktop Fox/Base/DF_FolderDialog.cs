using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows;
using System.Windows.Shapes;

namespace DesktopFox
{
    public class DF_FolderDialog
    {
        public static List<String> openFolderDialog()
        {
            FolderBrowserDialog folderDialog = new System.Windows.Forms.FolderBrowserDialog();

            if (folderDialog.ShowDialog() != System.Windows.Forms.DialogResult.OK) 
                return null;
            
            return getList(getFileInfo(folderDialog.SelectedPath)); 
        }

        private static List<String> getList(List<FileInfo> fileInfos)
        {
            List<String> list = new List<String>();
            foreach(FileInfo fileInfo in fileInfos)
            {
                list.Add(fileInfo.FullName);
            }
            return list;
        }


        private static List<FileInfo> getFileInfo(String path)
        {
            DirectoryInfo dir = new DirectoryInfo(path);
            FileInfo[] filesArray = dir.GetFiles();
            List<FileInfo> picFiles = new List<FileInfo>();
            foreach (FileInfo file in filesArray)
            {
                String ext = System.IO.Path.GetExtension(file.FullName).ToUpper();
                Console.WriteLine("Erweiterung der Dateien: " + ext);
                if (ext == ".PNG" || ext == ".JPG" || ext == ".JPEG" || ext == ".BMP")
                    picFiles.Add(file);
            }
            return picFiles;
        }

    }
}
