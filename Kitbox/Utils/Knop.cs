using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using System.Drawing;

namespace Kitbox
{
    public class Knop : Part //coupelle
    {
        /// <summary>
        /// construct the visualpart for the knop
        /// </summary>
        public override void ConstructVisualPart()
        {
            Size scaled = new Size(Convert.ToInt32(Dimensions.X), Convert.ToInt32(Dimensions.Y));
            Visual_part = new VisualPart(scaled,
                new Dictionary<string, Size>()
                {
                        { "front", scaled }
                });
            Visual_part.AddPanel("front_" + Reference, new Point(0, 0), scaled, Color, true);
        }
    }
}