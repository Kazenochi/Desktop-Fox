using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopFox.Base
{
    /// <summary>
    /// Spiegel Klasse zur Galerie um eine Auflösung von Setnamen zu Schlüsseln zu erleichtern
    /// </summary>
    public class GalleryShadow
    {
        private Gallery _gallery;
        private IDictionary<int, PictureSet> _pictureSetList;
        private Random _random = new Random();

        private IDictionary<String, int> _shadowList;

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="gallery"></param>
        public GalleryShadow(Gallery gallery)
        {
            _pictureSetList = gallery.PictureSetList;
            _shadowList = new Dictionary<String, int>();
            foreach (var i in _pictureSetList.Keys)
            {
                _shadowList.Add(_pictureSetList[i].SetName, i);
            }
        }

        /// <summary>
        /// Gibt den Schlüssel der Galerie des Angegebenen Bild Sets zurück 
        /// </summary>
        /// <param name="pictureSet">Name des Bild Sets</param>
        /// <returns></returns>
        public int GetKey(String pictureSet)
        {
            try
            {
                return _shadowList[pictureSet];
            }
            catch (System.Collections.Generic.KeyNotFoundException)
            {
                return 1001;
            }
        }

        /// <summary>
        /// Fügt ein neues Bilder Set zur Galerie hinzu
        /// </summary>
        /// <param name="pictureSet">Instanz des neuen Bilder Sets</param>
        public void Add(PictureSet pictureSet)
        {
            int tmpKey = GetNewKey();
            _pictureSetList.Add(tmpKey, pictureSet);
            _shadowList.Add(pictureSet.SetName, tmpKey);
        }

        /// <summary>
        /// Entfernt ein Bilder Set von der Galerie
        /// </summary>
        /// <param name="pictureSet">Name des Sets</param>
        public void Remove(String pictureSet)
        {
            _pictureSetList.Remove(_shadowList[pictureSet]);
            _shadowList.Remove(pictureSet);
        }

        /// <summary>
        /// Benennt ein Set um das sich in der Gallerie Befindet
        /// </summary>
        /// <param name="pictureset"></param>
        /// <param name="newName"></param>
        public void Rename(String pictureset, String newName)
        {
            if (_shadowList.ContainsKey(pictureset))
            {
                int tmpKey = _shadowList[pictureset];
                _pictureSetList[tmpKey].SetName = newName;
                _shadowList.Remove(pictureset);
                _shadowList.Add(newName, tmpKey);
                return;
            }
            Debug.WriteLine("Pictureset ist nicht vorhanden");
        }

        /// <summary>
        /// Gibt einen neuen Key zurück der noch nicht existiert.
        /// </summary>
        /// <returns></returns>
        private int GetNewKey()
        {
            int key;
            for(int i = 0; i < 100; i++)
            {
                key = i;
                if(_pictureSetList.ContainsKey(key) == false)
                    return key;
                else
                    key++;
            }
            Debug.WriteLine("Maximale Anzahl an Sets Erreicht");
            return 101;
        }
    }
}
