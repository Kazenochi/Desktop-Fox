using DesktopFox.MVVM.Model;
using System.Windows;
using System.Windows.Input;

namespace DesktopFox.MVVM.ViewModels
{

    public class MessageVM
    {
        public MessageModel Model { get; set; } = new MessageModel();
        private IMessageContainer _container;

        public ICommand YesCommand { get { return new DF_Command.DelegateCommand(o => _container.MessageAnswer(MessageBoxResult.Yes)); } }
        public ICommand NoCommand { get { return new DF_Command.DelegateCommand(o => _container.MessageAnswer(MessageBoxResult.No)); } }

        /// <summary>
        /// Festlegen des Vaterobjekts das auf die Antworten dieser Messagebox reagieren soll. <see cref="IMessageContainer.MessageAnswer(MessageBoxResult)"/>
        /// </summary>
        /// <param name="container"></param>
        public void DefineContainer(IMessageContainer container)
        {
            _container = container;
        }

        /// <summary>
        /// Erstellt anhand des <see cref="MessageType"/> die Nachricht in der Messagebox
        /// </summary>
        /// <param name="messageType"></param>
        public void GenerateMessage(MessageType messageType)
        {
            switch (messageType)
            {
                case MessageType.Error: break;

                case MessageType.Delete:
                    Model.Message = "Do you really want to delete this collection?";
                    break;

                case MessageType.Notification: break;
            }
        }
    }
}