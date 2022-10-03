namespace DesktopFox.MVVM.ViewModels
{
    public class PictureVM
    {
        public PictureSet pictureSet { get; set; }

        public PictureVM(PictureSet picture)
        {
            pictureSet = picture;
        }
    }
}
