using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Media.Media3D;

namespace Kitbox
{
    public class Door : Part //porte
    {
        private Knop knop;
        private string knop_position;
        public Door()
        {
            knop_position = "";
        }

        public Knop Knop
        {
            get { return knop; }
        }

        /// <summary>
        /// Construct the visualPart for the Door, 
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
            int invert;
            int take;
            if (knop_position == "left")
            {
                invert = -1;
                take = 0;
            }
            else if (knop_position == "right")
            {
                invert = 1;
                take = 1;
            }
            else
            {
                return;
            }
            Visual_part.AddVisualPart(knop.Reference+"*"+Knop.Position, knop.Visual_part,
                    new Dictionary<string, string>()
                    {
                        { "front", "front_" + Reference }
                    },
                    new Dictionary<string, Point>()
                    {
                        { "front", new Point(take * (scaled.Width - Convert.ToInt32(knop.Dimensions.X)) - invert * Convert.ToInt32(knop.Location.X), Convert.ToInt32((scaled.Height - Convert.ToInt32(knop.Dimensions.Y)) / 2)) }
                    });
        }

        /// <summary>
        /// add a knop for the door, at the position left or right
        /// </summary>
        /// <param name="knop"></param>
        /// <param name="x_location"></param>
        /// <param name="knop_position"></param>
        public void SetKnop(Knop knop, int x_location, string knop_position = "left/right")
        {
            if(this.knop != null)
            {
                Selling_price -= this.knop.Selling_price;
            }
            this.knop_position = knop_position.Split('/')[0];
            this.knop = knop;
            this.knop.Location = new Point3D(x_location, 0, 0);
            Selling_price += this.knop.Selling_price;
        }
    }
}
