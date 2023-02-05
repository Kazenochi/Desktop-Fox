using DesktopFox;
using System.Diagnostics;

namespace TestProject
{
    [TestClass]
    public class NextDaySwitch
    {
        [TestMethod]
        public void NowCheckBefore()
        {
            DateTime DateNow = DateTime.Now;
            DateTime NextSwitch = DateTime.Now.AddHours(23);

            DateTime returnValue = CheckDaySwitch.Check(DateNow, NextSwitch);
            Debug.WriteLine($"Current Time: {DateNow} | Time Until Switch: {returnValue - DateNow}");
            Debug.WriteLine($"Next Switch: {NextSwitch} | Return Value: {returnValue}");
            Assert.AreEqual(NextSwitch, returnValue);           
        }

        [TestMethod]
        public void NowCheckAfter()
        {
            DateTime DateNow = DateTime.Now.AddHours(26);
            DateTime NextSwitch = DateTime.Now.AddDays(1);

            DateTime returnValue = CheckDaySwitch.Check(DateNow, NextSwitch);
            Debug.WriteLine($"Current Time: {DateNow} | Time Until Switch: {returnValue - DateNow}");
            Debug.WriteLine($"Next Switch: {NextSwitch} | Return Value: {returnValue}");
            Assert.AreEqual(NextSwitch.AddDays(1), returnValue);
        }

        [TestMethod]
        public void LinearCheck()
        {
            DateTime DateNow = DateTime.Now;
            TimeSpan DayStart = TimeSpan.FromHours(8);
            DateTime NextSwitch = DateNow.Subtract(DateNow.TimeOfDay).Add(DayStart).AddDays(2);
            int AddDay = 0;
            bool Error = false;
            for (int i = 0; i < 100; i++)
            {
                AddDay = 0;
                DateNow = DateTime.Now.AddHours(i);
                DateTime returnValue = CheckDaySwitch.Check(DateNow, NextSwitch);
                Debug.WriteLine($"Count: {i}");
                Debug.WriteLine($"Current Time: {DateNow} | Time Until Switch: {returnValue-DateNow}");
                Debug.WriteLine($"Next Switch: {NextSwitch} | Return Value: {returnValue}");

                if (DateNow.TimeOfDay > DayStart)
                    AddDay = 1;                   

                if (DateNow < NextSwitch && Math.Abs((DateNow - NextSwitch).Days) > 0)
                {
                    Debug.WriteLine("Very Smaller");
                    if(DateNow.Subtract(DateNow.TimeOfDay).Add(DayStart).AddDays(AddDay) != returnValue)
                    {
                        Error = true;
                        Debug.WriteLine($"Error bei: {i} - {DateNow.Subtract(DateNow.TimeOfDay).Add(DayStart).AddDays(AddDay)} nicht gleich {returnValue}");
                    }            
                }
                else if (DateNow < NextSwitch)
                {
                    Debug.WriteLine("Smaller");
                    if(NextSwitch != returnValue)
                    {
                        Error = true;
                        Debug.WriteLine($"Error bei: {i} - {NextSwitch} nicht gleich {returnValue}");
                    }      
                }
                else
                {
                    Debug.WriteLine("Taller");
                    if (DateNow.Subtract(DateNow.TimeOfDay).Add(DayStart).AddDays(AddDay) != returnValue)
                    {
                        Error = true;
                        Debug.WriteLine($"Minus: {DateNow.Subtract(DateNow.TimeOfDay)} | Plus DayStart: {DateNow.Subtract(DateNow.TimeOfDay).Add(DayStart)}");
                        Debug.WriteLine($"Error bei: {i} - {DateNow.Subtract(DateNow.TimeOfDay).Add(DayStart).AddDays(AddDay)} nicht gleich {returnValue}");
                    }
                } 
                Debug.WriteLine("");
            }
            Assert.AreEqual(false, Error);
        }

        [TestMethod]
        public void LinearMinutenCheck()
        {
            DateTime DateNow = DateTime.Now;
            TimeSpan DayStart = TimeSpan.FromHours(8);
            DateTime NextSwitch = DateNow.Subtract(DateNow.TimeOfDay).Add(DayStart).AddDays(2);
            int AddDay = 0;
            bool Error = false;
            for (int i = 0; i < 10000; i++)
            {
                DateNow = DateTime.Now.AddMinutes(i);
                //DateNow = DateTime.Now.AddDays(i);
                DateTime returnValue = CheckDaySwitch.Check(DateNow, NextSwitch);
                Debug.WriteLine($"Count: {i}");
                Debug.WriteLine($"Current Time: {DateNow} | Time Until Switch: {returnValue - DateNow}");
                Debug.WriteLine($"Next Switch: {NextSwitch} | Return Value: {returnValue}");

                if (DateNow.TimeOfDay > DayStart)
                {
                    AddDay = 1;
                }
                else
                {
                    AddDay = 0;
                }

                if (DateNow < NextSwitch && Math.Abs((DateNow - NextSwitch).Days) > 0)
                {
                    Debug.WriteLine("Very Smaller");
                    if (DateNow.Subtract(DateNow.TimeOfDay).Add(DayStart).AddDays(AddDay) != returnValue)
                    {
                        Error = true;
                        Debug.WriteLine($"Error bei: {i} - {DateNow.Subtract(DateNow.TimeOfDay).Add(DayStart).AddDays(AddDay)} nicht gleich {returnValue}");
                    }

                }
                else if (DateNow < NextSwitch)
                {
                    Debug.WriteLine("Smaller");
                    if (NextSwitch != returnValue)
                    {
                        Error = true;
                        Debug.WriteLine($"Error bei: {i} - {NextSwitch} nicht gleich {returnValue}");
                    }

                }
                else
                {
                    Debug.WriteLine("Taller");
                    if (DateNow.Subtract(DateNow.TimeOfDay).Add(DayStart).AddDays(AddDay) != returnValue)
                    {
                        Error = true;
                        Debug.WriteLine($"Minus: {DateNow.Subtract(DateNow.TimeOfDay)} | Plus DayStart: {DateNow.Subtract(DateNow.TimeOfDay).Add(DayStart)}");
                        Debug.WriteLine($"Error bei: {i} - {DateNow.Subtract(DateNow.TimeOfDay).Add(DayStart).AddDays(AddDay)} nicht gleich {returnValue}");
                    }

                }

                Debug.WriteLine("");
                //Assert.AreEqual(NextSwitch, NextSwitch.AddDays(1));
            }
            Assert.AreEqual(false, Error);
        }

        [TestMethod]
        public void RandomCheck()
        {
            DateTime DateNow = DateTime.Now;
            TimeSpan DayStart = TimeSpan.FromHours(8);
            DateTime NextSwitch = DateNow.Subtract(DateNow.TimeOfDay).Add(DayStart).AddDays(5);
            int AddDay = 0;
            bool Error = false;
            for (int i = 0; i < 100; i++)
            {
                AddDay = 0;
                Random r = new Random();
                int TestValues = r.Next(0, 10000);
                DateNow = DateTime.Now.AddHours(TestValues);
                DateTime returnValue = CheckDaySwitch.Check(DateNow, NextSwitch);
                Debug.WriteLine($"Count: {TestValues}");
                Debug.WriteLine($"Current Time: {DateNow} | Time Until Switch: {returnValue - DateNow}");
                Debug.WriteLine($"Next Switch: {NextSwitch} | Return Value: {returnValue}");

                if (DateNow.TimeOfDay > DayStart)
                    AddDay = 1;

                if (DateNow < NextSwitch && Math.Abs((DateNow - NextSwitch).Days) > 0)
                {
                    Debug.WriteLine("Very Smaller");
                    if (DateNow.Subtract(DateNow.TimeOfDay).Add(DayStart).AddDays(AddDay) != returnValue)
                    {
                        Error = true;
                        Debug.WriteLine($"Error bei: {TestValues} - {DateNow.Subtract(DateNow.TimeOfDay).Add(DayStart).AddDays(AddDay)} nicht gleich {returnValue}");
                    }
                }
                else if (DateNow < NextSwitch)
                {
                    Debug.WriteLine("Smaller");
                    if (NextSwitch != returnValue)
                    {
                        Error = true;
                        Debug.WriteLine($"Error bei: {TestValues} - {NextSwitch} nicht gleich {returnValue}");
                    }
                }
                else
                {
                    Debug.WriteLine("Taller");
                    if (DateNow.Subtract(DateNow.TimeOfDay).Add(DayStart).AddDays(AddDay) != returnValue)
                    {
                        Error = true;
                        Debug.WriteLine($"Minus: {DateNow.Subtract(DateNow.TimeOfDay)} | Plus DayStart: {DateNow.Subtract(DateNow.TimeOfDay).Add(DayStart)}");
                        Debug.WriteLine($"Error bei: {TestValues} - {DateNow.Subtract(DateNow.TimeOfDay).Add(DayStart).AddDays(AddDay)} nicht gleich {returnValue}");
                    }
                }
                Debug.WriteLine("");
            }
            Assert.AreEqual(false, Error);
        }
    }
}
