using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopFox
{
    public static class CheckDaySwitch
    {
        public static DateTime Check(DateTime now, DateTime nextSwitch)
        {
            if (now < nextSwitch && Math.Abs((nextSwitch - now).Days) < 1) return nextSwitch;

            int AddDay = 0;
            if(now.TimeOfDay > nextSwitch.TimeOfDay) 
                AddDay = 1;

            nextSwitch = now.Date.Add(TimeSpan.FromDays(AddDay).Add(nextSwitch.TimeOfDay));
            return nextSwitch;
        }
    }
}
