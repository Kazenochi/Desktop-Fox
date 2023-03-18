using DesktopFox.MVVM.ViewModels;
using DesktopFox.MVVM.Views;
using IDesktopWallpaperWrapper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;

namespace DesktopFox
{
    public class VirtualDesktop
    {
        private readonly DesktopWallpaper wrapper = new();
        private readonly IDictionary<MonitorEnum, Monitor> monitorDict = new Dictionary<MonitorEnum, Monitor>();
        private readonly int monitorCount;
        private List<Wallpaper> wallpapers;
        private IntPtr progman, workerw;
        private readonly bool debug = true;

        /// <summary>
        /// Konstruktor
        /// </summary>
        public VirtualDesktop(WallpaperSaves wallpaperSaves = null)
        {
            workerw = IntPtr.Zero;
            String[] test = wrapper.GetAllMonitorIDs();
            foreach (String testID in test)
            {
                Console.WriteLine("MonitorIDS: " + testID);
            }
            convertMonitorID(wrapper.GetActiveMonitorIDs());
            this.monitorCount = monitorDict.Count;

            if(wallpaperSaves != null)
            {
                this.wallpapers = wallpaperSaves.wallpapers;
                buildDesktop();
            }
        }

        /// <summary>
        /// Wandelt das Array aus Monitor IDs in die Monitorbenennungen um. Main-, Second-, ThirdMonitor 
        /// </summary>
        /// <param name="monitor"></param>
        private void convertMonitorID(String[] monitor)
        {
            //Addressieren der Monitor ID's  

            //Hauptmonitor bestimmen
            for (int i = 0; i < monitor.Length; i++)
            {
                if (wrapper.GetMonitorRECT(monitor[i]).X == 0)
                    addNewMonitor(monitor[i], MonitorEnum.MainMonitor);
            }
            foreach (var pair in monitorDict)
                Console.WriteLine($"Monitor: {pair.Key} ----- Name der Adresse: {pair.Value}");

            if (monitor.Length > 1)
            {
                //Zweiten Monitor festlegen
                for (int i = 0; i < monitor.Length; i++)
                {
                    if (wrapper.GetMonitorRECT(monitor[i]).X == monitorDict.ElementAt(0).Value.Width)
                        addNewMonitor(monitor[i], MonitorEnum.SecondMonitor);
                }
            }
            if (monitor.Length > 2)
            {
                //Dritten Monitor festlegen
                for (int i = 0; i < monitor.Length; i++)
                {
                    if (wrapper.GetMonitorRECT(monitor[i]).X > monitorDict[MonitorEnum.MainMonitor].Width + monitorDict[MonitorEnum.SecondMonitor].Width || wrapper.GetMonitorRECT(monitor[i]).X < 0)
                        addNewMonitor(monitor[i], MonitorEnum.ThirdMonitor);
                }
            }
            calcBoundary();
        }

        /// <summary>
        /// Fügt einen neuen Monitor zum Monitor Dictionary hinzu
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="monID"></param>
        private void addNewMonitor(String monID, MonitorEnum monPos)
        {
            Monitor monitor = new Monitor(monID, monPos, wrapper.GetMonitorRECT(monID).Height, wrapper.GetMonitorRECT(monID).Width);
            //monitor.Name;
            monitorDict.Add(monPos, monitor);
        }

        /// <summary>
        /// Funktion die die Maximalwerte der Virtuellen Destopfläche in XY berechnet.
        /// </summary>
        private void calcBoundary()
        {
            int[] bound = new int[2];
            int width = 0;
            int hight = 0;
            foreach (Monitor monitor in monitorDict.Values)
            {
                width += monitor.Width;
                //Es wird geprüft was der Größte Wert in der Y Achse. 
                if (monitor.Height > hight)
                {
                    bound[1] = monitor.Height;
                }
            }
            bound[0] = width;
            //this.boundary = bound;
            //Console.WriteLine("Gesamte Desktop Auflösung Breite:" + width + " Höhe: " + hight);
        }

        #region FensterMethoden für Custom / Animierte Hintergründe

        /// <summary>
        /// Erstellt neue Animierte Hintergrundbilder
        /// </summary>
        /// <param name="monitors">Liste aus Monitor Nummern, auf denen das Hintergrundbild angezeigt werden soll</param>
        /// <param name="mediaUri">Pfad zur Datei die auf den Wallpaper angezeigt werden soll</param>
        /// <param name="imageRotation">Rotationswert den das Wallpaper haben soll</param>
        /// <param name="muted">Ob die Hintergrundbilder eine Tonausgabe haben sollen</param>
        public void newAnimatedWPs(List<int> monitors, Wallpaper wallpaperBluePrint)
        {
            wallpapers ??= new List<Wallpaper>();

            if (wallpapers.Count != 0)
                clearWallpapers();

            //Sicherheitsabfrage um zu gewährliesten das nur von einem Video die Audio abgespielt wird.
            for(int i = 0; i < monitors.Count;i++)
            {
                if(i == 0)
                    wallpapers.Add(WallpaperBuilder.makeWallpaper(this, monitors[i], wallpaperBluePrint.myMediaUri, wallpaperBluePrint.myRotation, wallpaperBluePrint.Volume));
                else
                    wallpapers.Add(WallpaperBuilder.makeWallpaper(this, monitors[i], wallpaperBluePrint.myMediaUri, wallpaperBluePrint.myRotation, VLCVolume.Mute));
            }
            buildDesktop();
        }

        /// <summary>
        /// Fängt den Windows Worker und holt sich die Adresse.
        /// Thanks to Gerald Degeneve who discovered this Solution
        /// https://www.codeproject.com/Articles/856020/Draw-Behind-Desktop-Icons-in-Windows-plus
        /// And rockdanister for providing the Source Code of an implementation of it
        /// https://github.com/rocksdanister/lively
        /// </summary>
        /// <returns></returns>
        private IntPtr windowCatch()
        {
            // Fetch the Progman window
            progman = NativeMethods.FindWindow("Progman", null);

            IntPtr result = IntPtr.Zero;

            // Send 0x052C to Progman. This message directs Progman to spawn a 
            // WorkerW behind the desktop icons. If it is already there, nothing 
            // happens.
            NativeMethods.SendMessageTimeout(progman,
                                   0x052C,
                                   new IntPtr(0xD),
                                   new IntPtr(0x1),
                                   NativeMethods.SendMessageTimeoutFlags.SMTO_NORMAL,
                                   1000,
                                   out result);

            workerw = IntPtr.Zero;

            NativeMethods.EnumWindows(new NativeMethods.EnumWindowsProc((tophandle, topparamhandle) =>
            {
                IntPtr p = NativeMethods.FindWindowEx(tophandle,
                                            IntPtr.Zero,
                                            "SHELLDLL_DefView",
                                            IntPtr.Zero);

                if (p != IntPtr.Zero)
                {
                    // Gets the WorkerW Window after the current one.
                    workerw = NativeMethods.FindWindowEx(IntPtr.Zero,
                                               tophandle,
                                               "WorkerW",
                                               IntPtr.Zero);
                }

                return true;
            }), IntPtr.Zero);

            return workerw;
        }

        /// <summary>
        /// Bauen des Hintergrundes, zwischen Desktop Icons und Hintergrundbild in Windows 
        /// </summary>
        private void buildDesktop()
        {
            if (wallpapers.Count <= 0) return;
            if (workerw == IntPtr.Zero) windowCatch();

            double widthFaktor = 1;
            
            foreach (Wallpaper wallpaper in wallpapers)
            {
                Screen s = Screen.AllScreens[(int)wallpaper.myMonitor.Name-1];
                BackgroundWindow backgroundWindow = new BackgroundWindow();

                if (wallpaper.myMonitor.Name > MonitorEnum.MainMonitor)
                {
                    widthFaktor = (SystemParameters.PrimaryScreenWidth / wallpaper.myMonitor.Width);
                }

                backgroundWindow.Top = s.Bounds.Top;
                backgroundWindow.Left = s.Bounds.Left * widthFaktor;

                backgroundWindow.Width = SystemParameters.PrimaryScreenWidth;
                backgroundWindow.Height = SystemParameters.PrimaryScreenHeight;

                if (debug)
                {
                    Debug.WriteLine("Monitor: \t" + (int)wallpaper.myMonitor.Name);
                    Debug.WriteLine("Actual Monitor Height: \t" + wallpaper.myMonitor.Height);
                    Debug.WriteLine("Actual Monitor Width: \t" + wallpaper.myMonitor.Width);
                    Debug.WriteLine("Wallpaper Monitor \t" + (int)wallpaper.myMonitor.Name);
                    Debug.WriteLine("BackWindow Top: \t" + s.Bounds.Top);
                    Debug.WriteLine("BackWindow Left: \t" + s.Bounds.Left);
                    Debug.WriteLine("BackWindow Width: \t" + backgroundWindow.Width);
                    Debug.WriteLine("BackWindow Height: \t" + backgroundWindow.Height);
                }

                BackgroundWindowVM backgroundWindowVM = new BackgroundWindowVM();
                backgroundWindow.DataContext = backgroundWindowVM;
                backgroundWindow.Show();
                wallpaper.myHandler = new WindowInteropHelper(backgroundWindow).Handle;
                NativeMethods.SetParent(wallpaper.myHandler, workerw);

                /*
                backgroundWindow.Width = wallpaper.myMonitor.Width;
                backgroundWindow.Height = wallpaper.myMonitor.Height;
                */

                AnimatedWallpaperVM animatedWPVM = new AnimatedWallpaperVM();
                AnimatedWallpaperView animatedWPView = new AnimatedWallpaperView(wallpaper);
                animatedWPView.DataContext = animatedWPVM;

                backgroundWindowVM.BackgroundModel.Wallpaper = animatedWPView;

                //BackgroundWindow tmpWin = (BackgroundWindow)HwndSource.FromHwnd(wallpaper.myHandler).RootVisual;
                //((BackgroundWindowVM)tmpWin.DataContext).BackgroundModel.Wallpaper.Play();

            }
        }

        /// <summary>
        /// Aufräumfunktion für die Animierten Hintergrundbilder
        /// </summary>
        /// <param name="lastClean">Ob es sich bei diesem Aufruf um den Letzten handelt bevor die Applikation beendet wird</param>
        public void clearWallpapers(bool lastClean = false)
        {
            if (wallpapers == null || wallpapers.Count <= 0) return;

            foreach(Wallpaper wallpaper in wallpapers)
            {
                //NativeMethods.CloseHandle(wallpaper.myHandler);
                ((BackgroundWindow)HwndSource.FromHwnd(wallpaper.myHandler).RootVisual).Close();
                wrapper.SetWallpaper(wallpaper.myMonitor.ID, wrapper.GetWallpaper(wallpaper.myMonitor.ID));
            }
            wallpapers.Clear();

            if(lastClean) wallpapers = null;
            
            Debug.WriteLine("Alle WPs wurden geschlossen");
        }

        #endregion

        #region Getter / Setter

        /// <summary>
        /// Gibt die Anzahl der Monitore zurück
        /// </summary>
        /// <returns></returns>
        public int getMonitorCount()
        {
            return monitorCount;
        }

        /// <summary>
        /// Gibt die Wrapper Klasse IDesktopWallpaperWrapper zurück
        /// </summary>
        public DesktopWallpaper getWrapper
        {
            get { return wrapper; }
        }

        /// <summary>
        /// Gibt alle Monitore zurück
        /// </summary>
        public IDictionary<MonitorEnum, Monitor> getMonitors { get { return monitorDict; } }

        /// <summary>
        /// Gibt den verlangten System Monitor zurück
        /// </summary>
        /// <param name="number">Nummer des Monitors: 1=Main, 2=Second, 3=Third</param>
        /// <returns></returns>
        public Monitor GetMonitor(int number)
        {
            switch (number)
            {
                case 1: return monitorDict[MonitorEnum.MainMonitor]; 
                case 2: return monitorDict[MonitorEnum.SecondMonitor]; 
                case 3: return monitorDict[MonitorEnum.ThirdMonitor];
                default: Debug.WriteLine("Kein Monitor mit diesem Wert verfügbar"); return null;            
            }
        }
        /// <summary>
        /// Gibt den Hauptmonitor des Systems zurück
        /// </summary>
        public Monitor getMainMonitor { get { return monitorDict[MonitorEnum.MainMonitor]; } }

        /// <summary>
        /// Gibt den Zweiten Monitor des Systems zurück
        /// </summary>
        public Monitor getSecondMonitor { get { return monitorDict[MonitorEnum.SecondMonitor]; } }

        /// <summary>
        /// Gibt den Dritten Monitor des Systems zurück
        /// </summary>
        public Monitor getThirdMonitor { get { return monitorDict[MonitorEnum.ThirdMonitor]; } }

        /// <summary>
        /// Gibt die Liste mit Hintergrundbildern zurück. <see cref="wallpapers"/>
        /// </summary>
        public List<Wallpaper> getWallpapers { get { return wallpapers; } }

        #endregion
    }
}