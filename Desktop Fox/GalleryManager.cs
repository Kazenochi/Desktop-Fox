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
        public GalleryManager(Gallery gallery, GalleryShadow shadow, MainWindowVM MainWindowViewModel)
        {
            _gallery = gallery;
            _shadow = shadow; 
            MWVM = MainWindowViewModel;
        }


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

                PictureViewVM newVM = new PictureViewVM(pictureSet);
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

        public void RenameSet(String pictureSet, String newName)
        {
            _shadow.Rename(pictureSet, newName);
        }

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

        public void addCollection(PictureSet pictureSet, Collection nwCollection, bool day)
        {
            if (day)
            {
                pictureSet.DayCol = nwCollection;
            }
            else
            {
                pictureSet.NightCol = nwCollection;
            }
        }

        public void removeCollection(String pictureSet, bool day, bool all=false)
        {
            if (all)
            {
                _shadow.Remove(pictureSet);
                MWVM.SelectedVM = null;
                PictureView? tmpDeleteViews = null;
                PictureViewVM? tmpDeleteVM = null;
                
                foreach(PictureView i in MWVM.MainWindowModel._pictureViews)
                {
                    if (i.pLabel.Content == pictureSet)
                    {
                        tmpDeleteViews = i;
                        tmpDeleteVM = (PictureViewVM)i.DataContext;
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
    }
}