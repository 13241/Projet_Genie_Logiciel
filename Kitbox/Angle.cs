
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Kitbox
{
    public class Angle : Part //cornière
    {

        //Coupe la cornière en un nombre x qui est le paramètre 
        //et retourne une liste contenant x nouvelle cornière
        //The height of the angle is taken form the database earlier
        //MODIF
        
        //La valeur finale de la cornière après avoir été coupée. 
        //c'est la propriété liée à l'attribut CutHeight
        //MODIF

        public override void ConstructVisualPart()
        {
            Size scaled = new Size(Convert.ToInt32(Dimensions.X), Convert.ToInt32(Dimensions.Y));
            Visual_part = new VisualPart(scaled,
                new Dictionary<string, Size>()
                {
                        { "front", scaled },
                        { "left", scaled}
                });
            Visual_part.AddPanel("front_" + Reference, new Point(0, 0), scaled, Color);
            Visual_part.AddPanel("left_" + Reference, new Point(0, 0), scaled, Color);
        }
    }
}