namespace DesktopFox.MVVM.ViewModels
{
    /// <summary>
    /// ViewModel der <see cref="Views.PictureView"/> Klasse
    /// </summary>
    public class PictureVM
    {
        /// <summary>
        /// Bilder Set das Tag und Nacht Collection beinhaltet und das Model dieser Klasse ist
        /// </summary>
        public PictureSet pictureSet { get; set; }

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="picture">Bilder Set das als Model verwendet werden soll</param>
        public PictureVM(PictureSet picture)
        {
            pictureSet = picture;
        }
    }
}
