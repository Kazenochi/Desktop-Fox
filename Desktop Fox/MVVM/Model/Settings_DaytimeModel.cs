using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopFox.MVVM.Model
{
    public class Settings_DaytimeModel : ObserverNotifyChange
    {    
        public int DayStartHours { get { return _dayStartHours; } set { _dayStartHours = value; RaisePropertyChanged(nameof(DayStartHours)); } }
        private int _dayStartHours = 8;               //Beginn des Tageszeitraums Bsp: 08:00 Uhr
        public int NightStartHours { get { return _nightStartHours; } set { _nightStartHours = value; RaisePropertyChanged(nameof(NightStartHours)); } }
        private int _nightStartHours = 20;      //Beginn des Nachtzeitraums Bsp: 20:00 Uhr
        public int DayStartMinutes { get { return _dayStartMinutes; } set { _dayStartMinutes = value; RaisePropertyChanged(nameof(DayStartMinutes)); } }
        private int _dayStartMinutes = 0;               //Beginn des Tageszeitraums Bsp: 08:00 Uhr
        public int NightStartMinutes { get { return _nightStartMinutes; } set { _nightStartMinutes = value; RaisePropertyChanged(nameof(NightStartMinutes)); } }
        private int _nightStartMinutes = 0;            //Beginn des Nachtzeitraums Bsp: 20:00 Uhr

        public void SetDaySwitch(TimeSpan dayStart, TimeSpan nightStart)
        {
            DayStartHours = dayStart.Hours;
            NightStartHours = nightStart.Hours;
            DayStartMinutes = dayStart.Minutes;
            NightStartMinutes = nightStart.Minutes;
        }
        
        public List<TimeSpan> SaveValues()
        {
            TimeSpan dayStart = new TimeSpan();
            TimeSpan nightStart = new TimeSpan();

            if (DayStartHours < 0 || (DayStartHours > 24 && DayStartMinutes > 0))
                DayStartHours = 8;
            if(DayStartMinutes < 0 || DayStartMinutes > 59)
                DayStartMinutes = 0;
            if(NightStartHours < 0 || (NightStartHours > 24 && NightStartMinutes > 0))
                NightStartHours = 20;
            if(NightStartMinutes < 0 || NightStartMinutes > 59)
                NightStartMinutes = 0;

            dayStart = TimeSpan.FromHours(DayStartHours) + TimeSpan.FromMinutes(DayStartMinutes);
            nightStart = TimeSpan.FromHours(NightStartHours) + TimeSpan.FromMinutes(NightStartMinutes);

            return new List<TimeSpan> { dayStart, nightStart };
        }

    }
}
