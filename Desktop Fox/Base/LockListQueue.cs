using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DesktopFox
{
    /// <summary>
    /// Klasse die für die Verwaltung einer Bilderliste, beim Shuffeln verantwortlich ist
    /// </summary>
    public class LockListQueue
    {
        public int _pictureCount { get; private set; } = 0;
        private int lastItem = 0;
        private int half = 0;
        private int lockAmount = 0;
        private int newItemIndex = 0;
        private bool linear = false;

        private Random random = new();
        private Queue<int> LockList = new();
        private List<int> WhiteList = new();

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="pictureCount">Anzahl der Bilder die Verwaltet werden sollen</param>
        public LockListQueue(int pictureCount)
        {
            _pictureCount = pictureCount;
            half = pictureCount / 2;
            lockAmount = (int)(pictureCount * 0.4);

            ResetPictureCount();
        }

        /// <summary>
        /// Gibt einen neuen zufälligen Index zurück
        /// </summary>
        /// <returns></returns>
        public int GetNewRandomItem()
        {
            linear = false;

            if (lastItem < half)
                newItemIndex = NewRandomNumber(half, WhiteList.Count);
            else
                newItemIndex = NewRandomNumber(0, half);

            lastItem = WhiteList.ElementAt(newItemIndex);
            
            if(LockList.Count >= lockAmount)
            {
                WhiteList.Add(LockList.Dequeue());
                WhiteList.Sort();
            }
           
            LockList.Enqueue(lastItem);
            WhiteList.Remove(lastItem);

            half = (int)(WhiteList.Count / 2);

            if (!LockList.Contains(lastItem)) return -1;
            if (WhiteList.Contains(lastItem)) return -1;
            
            Debug.WriteLine("Last before return.");
            return lastItem;
        }

        /// <summary>
        /// Gibt den nächsten Wert in der Schlange zurück
        /// </summary>
        /// <returns></returns>
        public int GetNextItem()
        {
            if (linear)
            {
                lastItem++;
                if(lastItem > _pictureCount-1)
                    lastItem = 0;
                return lastItem;               
            }
           
            linear = true;
            ResetPictureCount();               
            lastItem = 0;
            return WhiteList.ElementAt(lastItem);
        }

        /// <summary>
        /// Gibt den zuletzt gewählten Rückgabewert aus
        /// </summary>
        /// <returns></returns>
        public int GetLastMatch()
        {
            return lastItem;
        }

        /// <summary>
        /// Gibt eine neue Zufallszahl zurück
        /// </summary>
        /// <param name="min">Inclusiver Minimalwert</param>
        /// <param name="max">Exclusiver Maximalwert</param>
        /// <returns></returns>
        private int NewRandomNumber(int min, int max)
        {
            return random.Next(min, max);
        }

        /// <summary>
        /// Setzt die Klasse zurück
        /// </summary>
        /// <param name="newPictureCount"></param>
        public void ResetPictureCount(int newPictureCount = 0) 
        { 
            if(newPictureCount != 0)
            {
                _pictureCount = newPictureCount;
                half = _pictureCount / 2;
                lockAmount = (int)(_pictureCount * 0.4);
            }               

            LockList.Clear();
            WhiteList.Clear();
            for (int i = 0; i < _pictureCount; i++)
            {
                WhiteList.Add(i);
            }
        }

    }
}
