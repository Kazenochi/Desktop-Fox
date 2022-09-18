using IDesktopWallpaperWrapper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Desktop_Fox
{
    public class Virtual_Desktop
    {
        private DesktopWallpaper wrapper = new DesktopWallpaper();
        private IDictionary<String, Monitor> monitorDict = new Dictionary<String, Monitor>();
        private int monitorCount;
        //private Collection activeCollection;
        private int[] boundary;
        public Virtual_Desktop()
        {

            String[] test = wrapper.GetAllMonitorIDs();
            foreach (String testID in test)
            {
                Console.WriteLine("MonitorIDS: " + testID);
            }
            convertMonitorID(wrapper.GetActiveMonitorIDs());
            this.monitorCount = monitorDict.Count;
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
                    addNewMonitor("Main", monitor[i]);
            }
            foreach (var pair in monitorDict)
                Console.WriteLine($"Monitor: {pair.Key} ----- Name der Adresse: {pair.Value}");

            if (monitor.Length > 1)
            {
                //Zweiten Monitor festlegen
                for (int i = 0; i < monitor.Length; i++)
                {
                    if (wrapper.GetMonitorRECT(monitor[i]).X == monitorDict.ElementAt(0).Value.Width)
                        addNewMonitor("Second", monitor[i]);
                }
            }
            if (monitor.Length > 2)
            {
                //Dritten Monitor festlegen
                for (int i = 0; i < monitor.Length; i++)
                {
                    if (wrapper.GetMonitorRECT(monitor[i]).X > monitorDict["Main"].getWidth() + monitorDict["Second"].getWidth() || wrapper.GetMonitorRECT(monitor[i]).X < 0)
                        addNewMonitor("Third", monitor[i]);
                }
            }
            calcBoundary();
        }

        /// <summary>
        /// Fügt einen neuen Monitor zum Monitor Dictionary hinzu
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="monID"></param>
        private void addNewMonitor(String pos, String monID)
        {
            Monitor monitor = new Monitor(monID, pos);
            //monitor.Name;
            monitorDict.Add(pos, monitor);
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
            this.boundary = bound;
            //Console.WriteLine("Gesamte Desktop Auflösung Breite:" + width + " Höhe: " + hight);
        }

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


        public IDictionary<String, Monitor> getMonitors
        {
            get { return monitorDict; }
        }

        /// <summary>
        /// Gibt den Hauptmonitor des Systems zurück
        /// </summary>
        public Monitor getMainMonitor
        {
            get { return monitorDict["Main"]; }
        }

        /// <summary>
        /// Gibt den Zweiten Monitor des Systems zurück
        /// </summary>
        public Monitor getSecondMonitor
        {
            get { return monitorDict["Second"]; }
        }

        /// <summary>
        /// Gibt den Dritten Monitor des Systems zurück
        /// </summary>
        public Monitor getThirdMonitor
        {
            get { return monitorDict["Third"]; }
        }
    }
}