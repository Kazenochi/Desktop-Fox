using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DesktopFox
{
    public class TimeRule : ValidationRule
    {
        public int Min { get; set; }
        public int Max { get; set; }

        public TimeRule()
        {
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            int time = 0;

            try
            {
                if (((string)value).Length > 0)
                    time = Int32.Parse((String)value);
            }
            catch (Exception e)
            {
                return new ValidationResult(false, $"Illegal characters or {e.Message}");
            }

            if ((time < Min) || (time > Max))
            {
                return new ValidationResult(false,
                  $"Please enter a valid time: {Min}-{Max}.");
            }
            return ValidationResult.ValidResult;
        }
    }
}
