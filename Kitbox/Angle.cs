
using System;
using System.Collections.Generic;
namespace Kitbox
{
	class Angle : Part
	{
		string color;
		double height;
		int x; 

		//Coupe la cornière en un nombre x qui est le paramètre 
		//et retourne une liste contenant x nouvelle cornière
		//public Angle(double cut_height = 0.0)
		//{
		//	this.cut_height = cut_height;
		//}
		public Angle(string color, double height) 
		{
			this.color = color;
			this.height = height;
		}

		public List<Angle> Divide(int x)
		{
			//Retrieve the normalized height of the angle from the database that suits the case
			this.height = 17.8;
			this.x = x;
			double new_height = height / x;
			List<Angle> liste = new List<Angle>();
			for (int i = 0;i<x;i++)
			{
				Angle angle = new Angle(this.color,new_height);
				liste.Add(angle);
			}
			return liste;
		}
		//La valeur finale de la cornière après avoir été coupée. 
		//c'est la propriété liée à l'attribut CutHeight
		public double SetCutHeight
		{
			set{ this.height = value / x; }
		}
		public double GetCutheight
		{
			get { return this.height; }
		}
	}
}