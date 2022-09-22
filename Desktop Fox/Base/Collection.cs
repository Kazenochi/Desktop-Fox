using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Imaging;

namespace DesktopFox
{
    public class Picture
    {
        public String Name { get; set; }
        public String Path { get; set; }    //Speicherort des Bildes

        public Picture(string path)
        {
            this.Path = path;
            this.Name = System.IO.Path.GetFileName(path);
        }
    }

    public class Collection
    {
        public IDictionary<String, Picture> singlePics { get; set; }                //Note: Dictionary evtl. unnötig. List<Picture> müsste ausreichen 

        public String folderDirectory;

        public Collection()
        {
            singlePics = new Dictionary<String, Picture>();
        }

        public void addSinglePic(String Path)
        {
            var tmp = new Picture(Path);
            this.singlePics.Add(Path, tmp);
        }

        public BitmapImage getPreview()
        {
            if (singlePics.Count > 0)
                return ImageHandler.load(singlePics.ElementAt(0).Key);
            else
                return ImageHandler.dummy();
        }
    }
}