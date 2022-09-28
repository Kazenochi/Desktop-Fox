namespace DesktopFox.MVVM.ViewModels
{
    public class PictureViewVM
    {
        public PictureSet pictureSet { get; set; }

        public PictureViewVM(PictureSet picture)
        {
            pictureSet = picture;
        }
    }
}
