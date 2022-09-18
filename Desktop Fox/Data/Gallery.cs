using CommunityToolkit.Mvvm.ComponentModel;
using Desktop_Fox;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Controls;


namespace Desktop_Fox
{

    public class Gallery
    {
        public int Hello { get; set; }
        public IDictionary<String, PictureSet> PictureSetList { get; set; }

        private MainWindow _mainWindow;
        public List<String> activeSetsList = new List<String>() { "Empty", "Empty", "Empty" };

        public Gallery(MainWindow mainWindow)
        {
            PictureSetList = new Dictionary<String, PictureSet>();
            _mainWindow = mainWindow;
        }
        public void reInitMW(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
        }
    }
}