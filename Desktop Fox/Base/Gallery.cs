﻿using System;
using System.Collections.Generic;


namespace DesktopFox
{
    public class Gallery
    {
        /// <summary>
        /// Collection die alle Bilder Sets beinhaltet
        /// </summary>
        public IDictionary<int, PictureSet> PictureSetList { get; set; }
        
        /// <summary>
        /// Liste mit dem Picturesets die auf den monitoren Aktiv sind. Dient nur informativ und ist nicht teil der Logik
        /// </summary>
        public List<String> activeSetsList = new List<String>() { "Empty", "Empty", "Empty" };

        /// <summary>
        /// Konstruktor
        /// </summary>
        public Gallery()
        {
            PictureSetList = new Dictionary<int, PictureSet>();
        }
    }
}