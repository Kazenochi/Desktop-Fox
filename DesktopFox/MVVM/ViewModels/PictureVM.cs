using System;
using System.Collections.Generic;
using System.Windows.Documents;

namespace DesktopFox.MVVM.ViewModels
{
    /// <summary>
    /// ViewModel der <see cref="Views.PictureView"/> Klasse
    /// </summary>
    public class PictureVM
    {
        /// <summary>
        /// Bilder Set das Tag und Nacht Collection beinhaltet und das Model dieser Klasse ist
        /// </summary>
        public PictureSet pictureSet { get; set; }

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="picture">Bilder Set das als Model verwendet werden soll</param>
        public PictureVM(PictureSet picture)
        {
            pictureSet = picture;
        }

        /// <summary>
        /// Aktualisiert die Anzeige der BilderSets nach dem ändern des Aktiven Sets
        /// </summary>
        /// <param name="activeSet"></param>
        /// <param name="monitor"></param>
        public void ActiveSetChanged(String activeSet, int monitor)
        {
            if(activeSet == pictureSet.SetName)
            {
                switch (monitor)
                {
                    case 1: pictureSet.IsActive1 = true; break;
                    case 2: pictureSet.IsActive2 = true; break;
                    case 3: pictureSet.IsActive3 = true; break;
                }
            }
            else
            {
                switch (monitor)
                {
                    case 1: pictureSet.IsActive1 = false; break;
                    case 2: pictureSet.IsActive2 = false; break;
                    case 3: pictureSet.IsActive3 = false; break;
                }
            }
        }
    }
}
