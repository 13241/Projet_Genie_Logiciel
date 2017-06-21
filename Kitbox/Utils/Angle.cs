
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Kitbox
{
    public class Angle : Part //cornière
    {

        private double cut_height;

        /// <summary>
        /// height of the angle to be cut (if the angle is too high)
        /// </summary>
        public double Cut_height { get => cut_height; set => cut_height = value; }

        /// <summary>
        /// Construct a visual part of the Angle, the 2 views are in front and left
        /// </summary>
        public override void ConstructVisualPart()
        {
            Size scaled = new Size(Convert.ToInt32(Dimensions.X), Convert.ToInt32(Dimensions.Y - Cut_height));
            Visual_part = new VisualPart(scaled,
                new Dictionary<string, Size>()
                {
                        { "front", scaled },
                        { "left", scaled }
                });
            Visual_part.AddPanel("front_" + Reference, new Point(0, 0), scaled, Color);
            Visual_part.AddPanel("left_" + Reference, new Point(0, 0), scaled, Color);
        }
    }
}