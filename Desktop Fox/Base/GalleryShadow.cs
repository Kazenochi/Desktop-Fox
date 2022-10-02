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

        private IDictionary<String, int> _shadow;


        public GalleryShadow(Gallery gallery)
        {
            _pictureSetList = gallery.PictureSetList;
            _shadow = new Dictionary<String, int>();

            foreach (var i in _pictureSetList.Keys)
            {
                _shadow.Add(_pictureSetList[i].SetName, i);
            }
        }

        public int GetKey(PictureSet pictureSet)
        {
            try
            {
                return _shadow[pictureSet.SetName];
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
            _shadow.Add(pictureSet.SetName, tmpKey);
        }

        public void Remove(PictureSet pictureSet)
        {
            int tmpKey = _shadow[pictureSet.SetName];
            _pictureSetList.Remove(tmpKey);
            _shadow.Remove(pictureSet.SetName);
        }

        public void Rename(String pictureset, String newName)
        {
            if (_shadow.ContainsKey(pictureset))
            {
                int tmpKey = _shadow[pictureset];
                _pictureSetList[tmpKey].SetName = newName;
                _shadow.Remove(pictureset);
                _shadow.Add(newName, tmpKey);
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
