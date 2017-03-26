
using System;
using System.Collections.Generic;
namespace Kitbox
{
<<<<<<< HEAD
	public class Angle
	{
		protected double cut_height;

		//Coupe la cornière en un nombre x qui est le paramètre 
		//et retourne une liste contenant x nouvelle cornière
		public Divide (int cut_height) 
		{
			return List<Angle>;
		}

		//La valeur finale de la cornière après avoir été coupée. 
		//c'est la propriété liée à l'attribut CutHeight
		public void SetCutHeight(double cut_height)
		{
		}
		public double GetCutheight
		{
			get {return this.cut_height; }
=======
	class Angle : Part
	{
		string color;
		double height;
		int x; 

		//Coupe la cornière en un nombre x qui est le paramètre 
		//et retourne une liste contenant x nouvelle cornière
		//The height of the angle is taken form the database earlier
		public Angle(string color, double height) 
		{
			this.color = color;
			this.height = height;
		}

		public List<Angle> Divide(int x)
		{
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
>>>>>>> gDan15-master
		}
	}
}