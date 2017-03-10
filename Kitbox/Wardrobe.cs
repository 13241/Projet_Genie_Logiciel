using System;
using System.Collections.Generic;
namespace Kitbox
{
	public class Wardrobe
	{
		protected double height;
		protected List<Box> boxes = new List<Box>();
		protected List<Angle> angles = new List<Angle>();
		protected double depth;
		protected double width;
		protected VisualPart visual = new VisualPart();
		public Wardrobe()
		{
		}
		public double Height
		{
			get;
			set;
		}
		public double Depth
		{
			get;
			set;
		}
		public double Width
		{
			get;
			set;
		}
		public void AddAngle(Angle angle)
		{
			angles.Add(angle);
		}
		public void AddBox(Box box)
		{
			boxes.Add(box);
			height += box.Height;
		}
		public void RemoveBox(int index)
		{
			// on prend la vraie index d'une liste (qui commence a compter a 0)???
			// pas mieux de donner un objet Box en parametre pour faire directement boxes.Remove(box) ??????
			// faire la modification dans un setter ????
			boxes.RemoveAt(index);
		}
		public double Angles
		{
			get;
			// un setter????
		}

		public double Boxes
		{
			get;
			// un setter????
		}

	}
}
