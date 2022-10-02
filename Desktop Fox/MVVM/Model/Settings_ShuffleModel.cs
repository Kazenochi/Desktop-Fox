using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopFox.MVVM.Model
{
    public class Settings_ShuffleModel : ObserverNotifyChange
    {
        public int ShuffleTime { get { return _shuffleTime; } set { _shuffleTime = value; RaisePropertyChanged(nameof(_shuffleTime)); } }
        private int _shuffleTime = 0;            //Beginn des Nachtzeitraums Bsp: 20:00 Uhr

        public void SetShuffle(TimeSpan shuffleTime)
        {
            ShuffleTime = shuffleTime.Minutes;
        }
    }
}
