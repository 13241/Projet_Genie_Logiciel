using System;
using System.Collections.Generic;
using System.Collections;
using System.Windows.Media.Media3D;
using System.Drawing;
using System.Reflection;

namespace Kitbox
{
	public abstract class Part 
	{
        private VisualPart visual_part;
        private string reference;
        private string position;
        private string code;
        private Size3D dimensions;
        private Point3D location;
        private Color color;
        private int stock;
        private int min_stock;
        private double selling_price;


        public Part()
		{
            Code = "";
            Dimensions = new Size3D(0, 0, 0);
            Location = new Point3D(0, 0, 0);
            Color = Color.Empty;
            Stock = 0;
            Min_stock = 0;
            Selling_price = 0;
            visual_part = null;

            //MODIF???
			//Dictionary that contains every suppliers

			//Dictionary that contains every structure

			//Dictionary that contains every Type


		}

        public string Code { get => code; set => code = value; }
        public string Reference { get => reference; set => reference = value; }
        public Size3D Dimensions { get => dimensions; set => dimensions = value; }
        public Point3D Location { get => location; set => location = value; }
        public Color Color { get => color; set => color = value; }
        public int Stock { get => stock; set => stock = value; }
        public int Min_stock { get => min_stock; set => min_stock = value; }
        public double Selling_price { get => selling_price; set => selling_price = value; }
        public VisualPart Visual_part { get => visual_part; set => visual_part = value; }
        public string Position { get => position; set => position = value; }

        public bool IsAvailable(int number)
        {
            if (number < Stock)
            {
                return false;
            }
            return true;
        }

        public abstract void ConstructVisualPart();

        public void SetData(Dictionary<string, object> data)//MODIF FROM DATABASE
        {
            foreach(string key in data.Keys)
            {
                try
                {
                    GetType().GetProperty(key,
                    BindingFlags.FlattenHierarchy |
                    BindingFlags.IgnoreCase |
                    BindingFlags.Public |
                    BindingFlags.Instance).SetValue(this, data[key]);
                }
                catch
                {
                    throw new Exception("the requested attribute : " + key + " does not exist for this part");
                }
            }
        }
    }
}
