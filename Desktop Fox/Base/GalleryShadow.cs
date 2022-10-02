using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopFox.Base
{
    public class GalleryShadow
    {
        private Gallery _gallery;
        private IDictionary<int, PictureSet> _pictureSetList;
        private Random _random = new Random();

        private IDictionary<String, int> _shadowList;


        public GalleryShadow(Gallery gallery)
        {
            _pictureSetList = gallery.PictureSetList;
            _shadowList = new Dictionary<String, int>();

            foreach (var i in _pictureSetList.Keys)
            {
                _shadowList.Add(_pictureSetList[i].SetName, i);
            }
        }

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

        public void Add(PictureSet pictureSet)
        {
            int tmpKey = GetNewKey();
            _pictureSetList.Add(tmpKey, pictureSet);
            _shadowList.Add(pictureSet.SetName, tmpKey);
        }

        public void Remove(String pictureSet)
        {
            _pictureSetList.Remove(_shadowList[pictureSet]);
            _shadowList.Remove(pictureSet);
        }

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

        private int GetNewKey()
        {
            int key;
            for(int i = 0; i < 1000; i++)
            {
                key = i;
                if(_pictureSetList.ContainsKey(key) == false)
                    return key;
                else
                    key++;
            }
            Debug.WriteLine("Maximale Anzahl an Sets Erreicht");
            return 1001;
        }
    }
}
