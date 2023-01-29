using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DesktopFox
{
    /// <summary>
    /// Interface für Objecte die auf Antworten einer Messagebox reagieren sollen
    /// </summary>
    public interface IMessageContainer
    {
        /// <summary>
        /// Auswertung des <see cref="MessageBoxResult"/> Ergebnisses
        /// </summary>
        /// <param name="result"></param>
        public void MessageAnswer(MessageBoxResult result);
    }
}
