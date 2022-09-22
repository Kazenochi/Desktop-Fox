
namespace DesktopFox
{
    public class MainWindowVM
    {
        public MainWindowModel MainWindowModel { get; set; }
        public MainWindowVM()
        {
            MainWindowModel = new MainWindowModel();
        }  
    }
}
