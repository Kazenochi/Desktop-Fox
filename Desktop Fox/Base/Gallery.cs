using DesktopFox;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows.Controls;

namespace DesktopFox
{
    public class Gallery
    {
        public IDictionary<int, PictureSet> PictureSetList { get; set; }
        
        /// <summary>
        /// Liste mit dem Picturesets die auf den monitoren Aktiv sind. Dient nur Information und ist nicht teil der Logik
        /// </summary>
        public List<String> activeSetsList = new List<String>() { "Empty", "Empty", "Empty" };

        public Gallery()
        {
            PictureSetList = new Dictionary<int, PictureSet>();
        }
    }
}