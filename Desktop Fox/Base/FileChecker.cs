using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopFox
{
    /// <summary>
    /// Bietet unterschiedliche Methoden um Daten in der Galerie auf Fehler zu überprüfen und zu korrigieren
    /// </summary>
    internal class FileChecker
    {
        private GalleryShadow _shadowHelper;
        private Gallery _galleryLoaded;

        /// <summary>
        /// Überpüft ob es eine Differenz zwischen den Daten in der Gallery und dem Rechner gibt.
        /// </summary>
        /// <param name="gallery">Bereinigte Galerie</param>
        /// <returns></returns>
        public Gallery FullCheck(Gallery gallery)
        {
            _shadowHelper = new GalleryShadow(gallery);
            _galleryLoaded = gallery;

            MissingCheck();
            NewCheck();

            return _galleryLoaded;
        }

        /// <summary>
        /// Überprüft ob die Verzeichnisse oder Bilder in der Gallerie noch vorhandne sind und entfernd diese andernfalls
        /// </summary>
        /// <param name="gallery"></param>
        private void MissingCheck()
        {
            foreach(var i in _galleryLoaded.PictureSetList.Values)
            {
                if(Directory.Exists(i.DayCol.folderDirectory))
                {
                    foreach(var j in i.DayCol.singlePics.Values)
                    {
                        if (!File.Exists(j.Path))
                        {
                            i.DayCol.singlePics.Remove(j.Path);
                        } 
                    }
                }
                else
                {
                    i.DayCol = null;
                }

                if (Directory.Exists(i.NightCol.folderDirectory))
                {
                    foreach (var j in i.NightCol.singlePics.Values)
                    {
                        if (!File.Exists(j.Path))
                        {
                            i.NightCol.singlePics.Remove(j.Path);
                        }
                    }
                }
                else
                {
                    i.NightCol = null;
                }

                if (i.DayCol == null && i.DayCol == null)
                    _shadowHelper.Remove(i.SetName);
            }
        }

        /// <summary>
        /// Überprüft ob es neue Bilder in den Ordnern der Kollektionen gibt
        /// </summary>
        private void NewCheck()
        {          
            foreach(var i in _galleryLoaded.PictureSetList.Values)
            {
                var tmpFiles = DF_FolderDialog.getList(DF_FolderDialog.getFileInfo(i.DayCol.folderDirectory));
                foreach (var j in tmpFiles)
                {
                    if (!i.DayCol.singlePics.ContainsKey(j))
                        i.DayCol.singlePics.Add(j, new Picture(j));
                }
         
                tmpFiles = DF_FolderDialog.getList(DF_FolderDialog.getFileInfo(i.NightCol.folderDirectory));
                foreach (var j in tmpFiles)
                {
                    if (!i.NightCol.singlePics.ContainsKey(j))
                        i.NightCol.singlePics.Add(j, new Picture(j));
                }
                tmpFiles.Clear();
            }
        }

    }
}
