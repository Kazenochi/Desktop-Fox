using DesktopFox;
using DesktopFox.Base;
using DesktopFox.MVVM.ViewModels;
using DesktopFox.MVVM.Views;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Media.Imaging;

namespace DesktopFox
{

    /// <summary>
    /// Verwaltet Änderungen und Informationsbeschaffung von der Gallery
    /// </summary>
    public class GalleryManager
    {
        private Gallery _gallery;
        private GalleryShadow _shadow;
        private SettingsManager SM;
        private MainWindowVM MWVM;
        private Settings _settings;

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="gallery">Instanz der Galerie</param>
        /// <param name="shadow">Instanz der Galerie Spiegelklasse</param>
        /// <param name="MainWindowViewModel">Instanz des Hauptfenster Viemodels</param>
        public GalleryManager(Gallery gallery, GalleryShadow shadow, MainWindowVM MainWindowViewModel)
        {
            _gallery = gallery;
            _shadow = shadow; 
            MWVM = MainWindowViewModel;
        }

        /// <summary>
        /// Gibt die Galerie zurück
        /// </summary>
        public Gallery Gallery { get { return _gallery; } }

        /// <summary>
        /// Fügt ein Neues Set in der Gallery hinzu
        /// </summary>
        /// <param name="nwPictureSet"></param>
        /// <returns></returns>
        public void addSet(String SetName, Collection nwCollection, bool day = true)
        {
            if(nwCollection == null) return;
            
            PictureSet pictureSet = new PictureSet(SetName);

            if (_gallery.PictureSetList.ContainsKey(_shadow.GetKey(pictureSet.SetName)))
            {
                addCollection(_gallery.PictureSetList[_shadow.GetKey(pictureSet.SetName)], nwCollection, day);
            }
            else
            {
                addCollection(pictureSet, nwCollection, day);
                _shadow.Add(pictureSet);

                PictureVM newVM = new PictureVM(pictureSet);
                PictureView newView = new PictureView();
                newView.DataContext = newVM;

                MWVM.MainWindowModel._pictureViewVMs.Add(newVM);
                MWVM.MainWindowModel._pictureViews.Add(newView);

            }
            return;
        }

        /// <summary>
        /// Erfrägt welche Collections im Pictureset vorhanden sind <see cref="PictureSet.ContainsCollections"/>
        /// </summary>
        /// <param name="pictureSetName"></param>
        /// <returns>1=Day, 2=Night, 3=Both, 4=Twins</returns>
        public int ContainsCollections(String pictureSetName)
        {
            return _gallery.PictureSetList[_shadow.GetKey(pictureSetName)].ContainsCollections();
        }

        /// <summary>
        /// Brückenmethode: <see cref="GalleryShadow.Rename"/>
        /// </summary>
        /// <param name="pictureSet">Alter Name des Bild Sets</param>
        /// <param name="newName">Neuer Name des Bild Sets</param>
        public void RenameSet(String pictureSet, String newName)
        {
            _shadow.Rename(pictureSet, newName);
        }

        /// <summary>
        /// Erzeugt eine neue Collection //Note: Übergabe des Pfades ist nach aktuellem Stand unnötig
        /// </summary>
        /// <param name="pictures">Liste mit absoluten Pfaden zu den Bildern</param>
        /// <param name="path">Pfad zum Bild Ordner in dem sich die die angegeben Bilder befinden</param>
        /// <returns></returns>
        public static Collection makeCollection(List<String> pictures, String path)
        {
            Collection collection = new Collection();
            collection.folderDirectory = path;

            foreach (String i in pictures)
            {
                var tmp = new Picture(i);
                collection.singlePics.Add(i, tmp);
            }

            return collection;
        }

        /// <summary>
        /// Fügt eine Neue Collection zu einem Bilder Set hinzu
        /// </summary>
        /// <param name="pictureSet">Instanz des Bilder Sets</param>
        /// <param name="nwCollection">Instanz der Collection die hinzugefügt werden soll</param>
        /// <param name="day">Ob die Collection als TagesCollection hinzugefügt werden soll</param>
        public void addCollection(PictureSet pictureSet, Collection nwCollection, bool day)
        {
            if (day)
                pictureSet.DayCol = nwCollection;
            else
                pictureSet.NightCol = nwCollection;
        }

        /// <summary>
        /// Entfernt eine Collection von einem Bilder Set und entfernt dieses falls es anschließend keine Collection mehr beinhaltet.
        /// </summary>
        /// <param name="pictureSet">Name des Bild Sets</param>
        /// <param name="day">Soll die Tages Collection entfernt werden?</param>
        /// <param name="all">Soll das komplette Set entfernt werden?</param>
        public void removeCollection(String pictureSet, bool day, bool all=false)
        {
            if (all)
            {
                _shadow.Remove(pictureSet);
                MWVM.SelectedVM = null;
                PictureView? tmpDeleteViews = null;
                PictureVM? tmpDeleteVM = null;
                
                foreach(PictureView i in MWVM.MainWindowModel._pictureViews)
                {
                    if (i.pLabel.Content == pictureSet)
                    {
                        tmpDeleteViews = i;
                        tmpDeleteVM = (PictureVM)i.DataContext;
                    }  
                }
                if (tmpDeleteViews != null && tmpDeleteVM != null)
                {
                    MWVM.MainWindowModel._pictureViews.Remove(tmpDeleteViews);
                    MWVM.MainWindowModel._pictureViewVMs.Remove(tmpDeleteVM);
                }
                return;
            }

            if (day)
                _gallery.PictureSetList[_shadow.GetKey(pictureSet)].DayCol = null;
            else
                _gallery.PictureSetList[_shadow.GetKey(pictureSet)].NightCol = null;

        }

        /// <summary>
        /// Gibt die Tag Collection vom Set zurück
        /// </summary>
        /// <param name="pictureSet"></param>
        /// <returns></returns>
        public Collection getDayCollection(String pictureSet)
        {
            return _gallery.PictureSetList[_shadow.GetKey(pictureSet)].DayCol;
        }

        /// <summary>
        /// Gibt die Nacht Collection vom Set zurück
        /// </summary>
        /// <param name="pictureSet"></param>
        /// <returns></returns>
        public Collection getNightCollection(String pictureSet)
        {
            return _gallery.PictureSetList[_shadow.GetKey(pictureSet)].NightCol;
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
                        return _gallery.PictureSetList[_shadow.GetKey(_gallery.activeSetsList[i])];
                }
            }

            if (_gallery.activeSetsList[choice - 1] == "Empty")
                return null;

            return _gallery.PictureSetList[_shadow.GetKey(_gallery.activeSetsList[choice - 1])];
        }

        /// <summary>
        /// Gibt das Set zurück das aktuell als Preview angezeigt wird
        /// </summary>
        /// <returns></returns>
        public PictureSet getPreviewSet()
        {
            if (MWVM.SelectedVM == null) return null;

            return MWVM.SelectedVM.pictureSet;
        }

        /// <summary>
        /// Gibt die Anzahl der Aktiven Sets zurück
        /// </summary>
        /// <returns></returns>
        public int getActiveSetCount()
        {
            int tmpCount = 0;
            foreach(var i in _gallery.activeSetsList)
            {
                if (i != "Empty")
                    tmpCount++;
            }
            return tmpCount;
        }

        /// <summary>
        /// Setzt das angegebene aktive Pictureset das neu auf dem Desktop angezeigt wird
        /// </summary>
        /// <param name="pictureSet"></param>
        /// <param name="choice">1 = Erstes Set, 2 = Zweites Set, ...</param>
        public void setActiveSet(String pictureSet, int choice = 1)
        {
            _gallery.activeSetsList[choice - 1] = pictureSet;
        }

        /// <summary>
        /// Entfernt das angegebene Bild Set aus der Liste der Aktiven Sets
        /// </summary>
        /// <param name="pictureSet">Name des Sets das Angehalten werden soll</param>
        /// <param name="choice"></param>
        /// <returns></returns>
        public bool stopActiveSet(String pictureSet, int choice = 1)
        {
            bool isEmpty = true;
            for(int i = 0; i < _gallery.activeSetsList.Count; i++)
            {
                if (_gallery.activeSetsList[i] == pictureSet)
                    _gallery.activeSetsList[i] = "Empty";

                if (_gallery.activeSetsList[i] != "Empty");
                    isEmpty = false;
            }
            return isEmpty;
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
        /// Gibt das nächste Set in der Liste zurück. Nach dem Letzten Element wird wieder das erste zurückgegeben
        /// Wenn es nicht mehr als ein Set gibt wird des übergebene Set wieder zurück gegeben
        /// </summary>
        /// <param name="pictureSet"></param>
        /// <returns></returns>
        public String getNextSet(String pictureSet)
        {
            if (_gallery.PictureSetList.Count > 1)
            {
                List<String> tmpList = new List<string>();
                int tmpCount = 0;
                int match = 0;

                foreach (PictureSet i in _gallery.PictureSetList.Values)
                {
                    tmpList.Add(i.SetName);
                    if (pictureSet == i.SetName)
                        match = tmpCount;
                    else
                        tmpCount++;
                }

                if ((match + 1) >= tmpList.Count)
                    return tmpList.ElementAt(0);
                else
                    return tmpList.ElementAt(match + 1);
            }
            else
            {
                return pictureSet;
            }
        }
    }
}