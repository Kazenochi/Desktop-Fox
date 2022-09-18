using System;
using System.Collections.Generic;


namespace Desktop_Fox
{
    public class Picture
    {

        public String Name { get; set; }
        public String Path { get; set; }    //Speicherort des Bildes
        public String Size { get; set; }    //Bildgröße
        public String visual;               //Anzeigeart wie es auf dem Desktop angezeigt werden soll. Normal/Tiled/Stretch
        public int brightness;              //Helligkeit des Bildes um die Automatischen Rotation zu ermöglichen 
        public Picture(string path)
        {
            this.Path = path;
            this.Name = System.IO.Path.GetFileName(path);
            this.Size = "Test";
        }
    }

    public class Collection
    {

        public IDictionary<String, Picture> singlePics { get; set; }                //Note: Dictionary evtl. unnötig. List<Picture> müsste ausreichen 
        public IDictionary<String, object> multiPics { get; set; }
        public String folderDirectory;
        public Collection()
        {
            singlePics = new Dictionary<String, Picture>();
            multiPics = new Dictionary<String, object>();
        }

        public void addSinglePic(String Path)
        {
            var tmp = new Picture(Path);
            this.singlePics.Add(Path, tmp);
        }
    }
}