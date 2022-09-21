using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Media.Imaging;
using static System.Windows.Forms.DataFormats;

namespace Desktop_Fox
{
    
    public class PictureSet
    {
        public String Name { get; set; }
        public BitmapImage DayImage;
        public BitmapImage NightImage;

        public PictureSet(string name)
        {
            Name = name;
            DayImage = ImageHandler.load("F:\\DesktopFoxTestPicture\\Normal\\1.jpg");
            NightImage = ImageHandler.load("F:\\DesktopFoxTestPicture\\Normal\\2.jpg");
        }
    }
}