using DesktopFox;
using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace DesktopFox
{
    public class Gallery
    {
        public IDictionary<String, PictureSet> PictureSetList { get; set; }
        public List<String> activeSetsList = new List<String>() { "Empty", "Empty", "Empty" };

        public Gallery()
        {
            PictureSetList = new Dictionary<String, PictureSet>();
        }
    }
}