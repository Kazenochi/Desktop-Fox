using DesktopFox.MVVM.Model;

namespace DesktopFox.MVVM.ViewModels
{
    public class BackgroundWindowVM
    {
        public BackgroundWindowModel BackgroundModel { get; set; } = new ();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="monitor">Auf Welchem Monitor dieses Fenster Angezeigt wird</param>
        /// <param name="monitorHeight"></param>
        /// <param name="monitorWidth"></param>
        public BackgroundWindowVM()
        {
        }
    }
}
