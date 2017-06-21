using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Kitbox
{
    public class Cleat : Part //tasseau (avec rainures pour glisser les panneaux)
    {
        /// <summary>
        /// the cleat does not need a viusalPart in this application
        /// </summary>
        public override void ConstructVisualPart()
        {
            //not implemented (this part need not be seen)
        }

    }
}
