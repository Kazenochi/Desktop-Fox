using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Media.Imaging;
using Windows.ApplicationModel.Contacts.Provider;

namespace Desktop_Fox
{
    public class PictureViewVM
    {
        public PictureSet pictureSet { get; set; }

        public PictureViewVM()
        {
            pictureSet = new PictureSet("Placeholder");
        }
        public void reInit(PictureSet picture)
        {
            pictureSet = picture;
        }
    }
}
