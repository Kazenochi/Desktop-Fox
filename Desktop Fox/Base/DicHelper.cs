using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation.Collections;

namespace Desktop_Fox.Base
{
    /// <summary>
    /// Beinhaltet ein Dictionary das eine gegenteilige Relation zwischen Key und Name, der PictureSetList beinhaltet.
    /// </summary>
    internal class DicHelper
    {
        IDictionary<String, PictureSet> PSL;
        IDictionary<String, String> PSLShadow;
        public DicHelper(IDictionary<String, PictureSet> PictureSetList)
        {

        }
    }
}
