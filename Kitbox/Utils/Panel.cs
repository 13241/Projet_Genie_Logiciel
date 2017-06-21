using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Kitbox
{
    public class Panel : Part //panel : panneau / nogging : traverse
    {
        /// <summary>
        /// Construct the visualPart for a panel (which is also a nogging => same behavior for a visualPart)
        /// </summary>
        public override void ConstructVisualPart()
        {
            Size scaled = new Size(Convert.ToInt32(Dimensions.X), Convert.ToInt32(Dimensions.Y));
            Visual_part = new VisualPart(scaled,
                new Dictionary<string, Size>()
                {
                        { "front", scaled }
                });
            Visual_part.AddPanel("front_" + Reference, new Point(0, 0), scaled, Color);
        }
    }
}
