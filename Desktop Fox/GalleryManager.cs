using DesktopFox;
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
        private SettingsManager SM;
        public GalleryManager(Gallery gallery)
        {
            _gallery = gallery;
        }
        



        public void reInit(SettingsManager settingsManager)
        {
            SM = settingsManager;
        }
    }

}