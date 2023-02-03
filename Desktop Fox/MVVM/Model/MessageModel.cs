

namespace DesktopFox.MVVM.Model
{
    public class MessageModel : ObserverNotifyChange
    {
        /// <summary>
        /// Nachricht die in der Messagebox angezeigt werden soll
        /// </summary>
        public string Message { get { return _message; } set { _message = value; RaisePropertyChanged(nameof(Message)); } }
        private string _message = "";
    }
}
