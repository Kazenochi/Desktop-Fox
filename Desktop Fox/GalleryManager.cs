using Desktop_Fox;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Desktop_Fox
{
    /// <summary>
    /// Verwaltet Änderungen und Informationsbeschaffung von der Gallery
    /// </summary>
    public class GalleryManager
    {
        /*
        private Gallery _gallery;
        private MainWindow _mainWindow;
        private List<String> files = new List<String>();
        private PictureSet previewSet;
        private SettingsManager SM;
        public GalleryManager(Gallery gallery, MainWindow mainWindow)
        {
            _gallery = gallery;
            _mainWindow = mainWindow;
        }
        public void reInit(Gallery gallery)
        {
            _gallery = gallery;
        }
        public void reInit(SettingsManager settingsManager)
        {
            SM = settingsManager;
        }

        /// <summary>
        /// Fügt ein Neues Set in der Gallery hinzu
        /// </summary>
        /// <param name="nwPictureSet"></param>
        /// <returns></returns>
        public Boolean addSet(PictureSet nwPictureSet)
        {
            try
            {
                _gallery.PictureSetList.Add(nwPictureSet.name, nwPictureSet);

            }
            catch (System.ArgumentException)
            {
                Console.WriteLine("Fehler beim Hinzufügen des Sets. Name ist möglicherweise schon vorhanden");
                return false;
            }

            //galleryChange();
            return true;
        }

        /// <summary>
        /// Entfernt das angegebene Set von der Gallery
        /// </summary>
        /// <param name="set"></param>
        public void removeSet(PictureSet set)
        {
            if (Object.ReferenceEquals(this.previewSet, set))
            {

            }
            _gallery.PictureSetList.Remove(set.name);
            if (_gallery.PictureSetList.Count == 0)
            {
                this.previewSet = null;                     //nicht sicher ob das notwendig ist
                _mainWindow.shuffler.stopPreviewShuffleTimer();
            }
            else
            {
                this.previewSet = _gallery.PictureSetList.ElementAt(0).Value;
            }

            galleryChange();
        }

        /// <summary>
        /// Routine die Aufgerufen wird wenn der Inhalt der Gallery geändert wurde
        /// </summary>
        public void galleryChange()
        {
            _mainWindow.pictureboxFill();
            foreach (PictureView i in _mainWindow.listBoxPreview.Items)
            {
                i.MarkerCheck();
            }
            //_mainWindow.pictureboxMarker();
        }

        /// <summary>
        /// Fügt einem PictureSet eine neue Bilder Collection hinzu
        /// </summary>
        /// <param name="pictureSet"></param>
        /// <param name="collection"></param>
        public void setDayCollection(PictureSet pictureSet, Collection collection)
        {
            _gallery.PictureSetList[pictureSet.name].DayCol = collection;
            galleryChange();
        }

        /// <summary>
        /// Fügt einem PictureSet eine neue Bilder Collection hinzu
        /// </summary>
        /// <param name="pictureSet"></param>
        /// <param name="collection"></param>
        public void setNightCollection(PictureSet pictureSet, Collection collection)
        {
            _gallery.PictureSetList[pictureSet.name].NightCol = collection;
            galleryChange();
        }

        /// <summary>
        /// Gibt die Tag Collection vom Set zurück
        /// </summary>
        /// <param name="pictureSet"></param>
        /// <returns></returns>
        public Collection getDayCollection(PictureSet pictureSet)
        {
            return _gallery.PictureSetList[pictureSet.name].DayCol;
        }

        /// <summary>
        /// Gibt die Nacht Collection vom Set zurück
        /// </summary>
        /// <param name="pictureSet"></param>
        /// <returns></returns>
        public Collection getNightCollection(PictureSet pictureSet)
        {
            return _gallery.PictureSetList[pictureSet.name].NightCol;
        }

        /// <summary>
        /// Setzt das angegebene aktive Pictureset das neu auf dem Desktop angezeigt wird
        /// </summary>
        /// <param name="pictureSet"></param>
        /// <param name="choice">1 = Erstes Set, 2 = Zweites Set, ...</param>
        public void setActiveSet(PictureSet pictureSet, int choice = 1)
        {
            String tmpName;
            if (pictureSet == null)
            {
                tmpName = "Empty";
            }
            else
            {
                tmpName = pictureSet.name;
            }

            _gallery.activeSetsList[choice - 1] = tmpName;
            _mainWindow.pictureboxReMark();

            //Prüfen ob es noch ein Aktives Set gibt und welcher modus aktiv ist, ansonsten Stoppen des Timers
            if (areSetsActive() && SM.getDesktopMode() == "Multi")
                return;
            else if (areSetsActive(number: 1) && SM.getDesktopMode() == "Single")
                return;
            else
                _mainWindow.shuffler.stopDesktopTimer();

            //_gallery.activePictureSet = pictureSet;
            //galleryChange();
        }

        /// <summary>
        /// Gibt das angegebene aktive Pictureset zurück das aktuell auf dem Desktop angezeigt wird.
        /// </summary>
        /// <param name="choice">1 = Erstes Set, 2 = Zweites Set, ...</param>
        /// <returns></returns>
        public PictureSet getActiveSet(int choice = 1, Boolean any = false)
        {
            if (choice > _gallery.PictureSetList.Count)
                choice = 1;

            if (any && _gallery.activeSetsList[choice - 1] == "Empty")
            {
                for (int i = 0; i < _gallery.activeSetsList.Count; i++)
                {
                    if (_gallery.activeSetsList[i] != "Empty")
                        return _gallery.PictureSetList[_gallery.activeSetsList[i]];
                }
            }

            if (_gallery.activeSetsList[choice - 1] == "Empty")
                return null;

            return _gallery.PictureSetList[_gallery.activeSetsList[choice - 1]];
            //return _gallery.activePictureSet;
        }

        /// <summary>
        /// Gibt die Anzahl der Aktiven Sets zurück
        /// </summary>
        /// <returns></returns>
        public int getActiveSetCount()
        {
            return _gallery.activeSetsList.Count;
        }

        /// <summary>
        /// Checked ob es Aktive Sets gibt anhand vom Status des ersten Sets
        /// </summary>
        /// <returns></returns>
        public Boolean areSetsActive(int number = 0)
        {
            switch (number)
            {
                case 0:
                    Boolean result = false;
                    foreach (String i in _gallery.activeSetsList)
                    {
                        if (i != "Empty")
                            result = true;
                    }
                    return result;
                    break;
                case 1:
                    if (_gallery.activeSetsList[0] != "Empty")
                        return true;
                    break;
                case 2:
                    if (_gallery.activeSetsList[1] != "Empty")
                        return true;
                    break;
                case 3:
                    if (_gallery.activeSetsList[2] != "Empty")
                        return true;
                    break;
                default:
                    return false;
                    break;
            }
            return false;
        }

        /// <summary>
        /// Gibt das Set zurück das aktuell als Preview angezeigt wird
        /// </summary>
        /// <returns></returns>
        public PictureSet getPreviewSet()
        {
            if (this.previewSet == null)
            {
                if (_gallery.PictureSetList.Count == 0)
                    return null;
                else
                    return _gallery.PictureSetList.ElementAt(0).Value;
            }
            return this.previewSet;
        }

        /// <summary>
        /// Setzt ein Set als Preview Set das angezeigt werden soll
        /// </summary>
        /// <param name="pictureSet"></param>
        public void setPreviewSet(PictureSet pictureSet)
        {
            this.previewSet = pictureSet;
            //galleryChange();
        }

        /// <summary>
        /// Fügt neue Bilder der mitgegebenen Collection hinzu
        /// ACHTUNG: Funktion Collidiert aktuell noch mit der Windows Shuffle Funktion. es Darf nur einen OrdnerPfad geben
        /// </summary>
        /// <param name="col"></param>
        /// <param name="pictures"></param>
        public void addCollection(Collection col, List<String> pictures, String path, Boolean wait = false)
        {
            //Sicherheitsmechanismus um zu gewährleisten das eine Collection, nur in einem Ordner bilder hat
            if (col.singlePics.Count != 0)
            {
                col.singlePics.Clear();
            }

            col.folderDirectory = path;

            if (col.singlePics.Count != 0)
                pictures = duplicateFileCheck(pictures, col);

            foreach (String i in pictures)
            {
                var tmp = new Picture(i);
                col.singlePics.Add(i, tmp);
            }

            //Anstoßen des Shuffle Timers falls die Collection geändert wurde die gerade aktiv ist
            if (SM.isRunning() && getActiveSet(any: true) != null)
            {
                if (getActiveSet(1) != null && (col.folderDirectory == getActiveSet(1).DayCol.folderDirectory || col.folderDirectory == getActiveSet(1).NightCol.folderDirectory))
                    _mainWindow.shuffler.picShuffleStart();
                else if (getActiveSet(2) != null && (col.folderDirectory == getActiveSet(2).DayCol.folderDirectory || col.folderDirectory == getActiveSet(2).NightCol.folderDirectory))
                    _mainWindow.shuffler.picShuffleStart();
                else if (getActiveSet(3) != null && (col.folderDirectory == getActiveSet(3).DayCol.folderDirectory || col.folderDirectory == getActiveSet(3).NightCol.folderDirectory))
                    _mainWindow.shuffler.picShuffleStart();
            }

            if (wait == false)
                galleryChange();
        }

        /// <summary>
        /// Entfernt alle Bilder aus einer Collection
        /// </summary>
        /// <param name="pictureSet"></param>
        /// <param name="colTime"></param>
        public void removeCollection(Collection collection, Boolean wait = false)
        {
            collection.singlePics.Clear();
            if (wait == false)
                galleryChange();
        }

        /// <summary>
        /// Überprüft ob es das Selbe Bild schon in der Collection gibt und Entfernt diese dann um keine Duplicate zu haben.
        /// Die Bereinigte Liste wird anschließend zurückgegeben
        /// </summary>
        /// <param name="files"></param>
        /// <param name="picCol"></param>
        /// <returns></returns>
        private List<String> duplicateFileCheck(List<String> files, Collection picCol)
        {
            //Entkopplung der Liste um mit foreach die Elemente mit Treffern zu entfernen
            List<String> tmpList = new List<String>();

            foreach (String i in files)
            {
                try
                {
                    if (i == picCol.singlePics.Where(j => j.Value.Path == i).FirstOrDefault().Value.Path)
                    {
                        tmpList.Add(i);
                    }
                }
                catch (System.NullReferenceException)
                {
                    Console.WriteLine("Kein Duplikat Gefunden");
                }
            }

            foreach (var i in tmpList)
            {
                files.Remove(i);
            }
            Console.WriteLine("Beim Einlesen wurden Doppelte Dateien Gefunden. " + files.Count + " Bilder wurden nicht eingelesen");
            return files;
        }

        /// <summary>
        /// Liest die Dateien aus dem Ordner aus und wandelt diese von einem String Array in eine Liste um
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public List<FileInfo> getFiles(String path)
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

        public DirectoryInfo[] getFolders(String path)
        {
            DirectoryInfo dir = new DirectoryInfo(path);
            return dir.GetDirectories();
        }

        /// <summary>
        /// Überprüft alle Einträge in der Gallery. Entfernt fehlende Bilder und fügt neue Bilder im Ordner hinzu
        /// </summary>
        public void galleryRefresh()
        {
            foreach (PictureSet i in _gallery.PictureSetList.Values)
            {
                List<FileInfo> tmpFiles = new List<FileInfo>();
                List<String> tmpList = new List<String>();

                if (Directory.Exists(i.DayCol.folderDirectory))
                {
                    #region "Cleanup für Dateien und Ordner die nicht mehr exixtieren"
                    foreach (Picture j in i.DayCol.singlePics.Values)
                    {
                        if (File.Exists(j.Path) == false)
                        {
                            i.DayCol.singlePics.Remove(j.Path);
                        }
                    }
                    #endregion

                    #region "Einlesen von evtl. Neu hinzugekommenen Bildern"
                    tmpFiles = getFiles(i.DayCol.folderDirectory);
                    if (tmpFiles.Count > 0)
                    {
                        foreach (FileInfo file in tmpFiles)
                        {
                            tmpList.Add(file.FullName);
                        }
                        addCollection(i.DayCol, tmpList, tmpFiles.ElementAt(0).DirectoryName, wait: true);
                    }
                    #endregion
                }
                else
                {
                    removeCollection(i.DayCol, wait: true);
                }

                if (Directory.Exists(i.NightCol.folderDirectory))
                {
                    #region "Cleanup für Dateien und Ordner die nicht mehr exixtieren"
                    foreach (Picture j in i.NightCol.singlePics.Values)
                    {
                        if (File.Exists(j.Path) == false)
                        {
                            i.NightCol.singlePics.Remove(j.Path);
                        }
                    }
                    #endregion

                    #region "Einlesen von evtl. Neu hinzugekommenen Bildern"
                    tmpFiles = getFiles(i.NightCol.folderDirectory);
                    if (tmpFiles.Count > 0)
                    {
                        tmpList.Clear();
                        foreach (FileInfo file in tmpFiles)
                        {
                            tmpList.Add(file.FullName);
                        }
                        addCollection(i.NightCol, tmpList, tmpFiles.ElementAt(0).DirectoryName, wait: true);
                    }
                    #endregion
                }
                else
                {
                    removeCollection(i.NightCol, wait: true);
                }

                if (i.DayCol.singlePics.Count == 0 && i.NightCol.singlePics.Count == 0)
                    removeSet(_gallery.PictureSetList[i.name]);
            }
            galleryChange();
        }

        /// <summary>
        /// Benennt ein vorhandenes Set um. Gibt "false" bei einem Fehler zurück.
        /// </summary>
        /// <param name="oldName"></param>
        /// <param name="newName"></param>
        /// <returns></returns>
        public Boolean renameSet(String oldName, String newName)
        {
            foreach (var i in _gallery.PictureSetList.Values)
            {
                if (i.name == newName)
                    return false;
            }
            if (_gallery.PictureSetList.ContainsKey(oldName) == false)
                return false;

            PictureSet tmpSet = _gallery.PictureSetList[oldName];


            if (this.previewSet.name == tmpSet.name)
                this.previewSet = tmpSet;

            for (int i = 0; i < _gallery.activeSetsList.Count; i++)
            {
                if (_gallery.activeSetsList[i] == tmpSet.name)
                    _gallery.activeSetsList[i] = newName;
            }

            tmpSet.name = newName;
            addSet(tmpSet);

            _gallery.PictureSetList.Remove(oldName);
            galleryChange();
            return true;
        }

        /// <summary>
        /// Gibt das nächste Set in der Liste zurück. Nach dem Letzten Element wird wieder das erste zurückgegeben
        /// Wenn es nicht mehr als ein Set gibt wird des übergebene Set wieder zurück gegeben
        /// </summary>
        /// <param name="pictureSet"></param>
        /// <returns></returns>
        public PictureSet getNextSet(PictureSet pictureSet)
        {
            if (_gallery.PictureSetList.Count > 1)
            {
                List<String> tmpList = new List<string>();
                int tmpCount = 0;
                int match = 0;

                foreach (String i in _gallery.PictureSetList.Keys)
                {
                    tmpList.Add(i);
                    if (pictureSet.name == i)
                        match = tmpCount;
                    else
                        tmpCount++;
                }

                if ((match + 1) >= tmpList.Count)
                    return _gallery.PictureSetList[tmpList.ElementAt(0)];
                else
                    return _gallery.PictureSetList[tmpList.ElementAt(match + 1)];
            }
            else
            {
                return pictureSet;
            }

        }
    }
        */
    }
}