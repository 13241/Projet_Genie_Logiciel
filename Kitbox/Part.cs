using System;
using System.Collections.Generic;
namespace Kitbox
{
	public abstract class Part 
	{
		protected Dictionary<string, object> part_data = new Dictionary<string, object>();
		protected string Color = "";

		public Part()
		{
			part_data.Add("Code"," ");
			part_data.Add("Height", 0.0);
			part_data.Add("Depth", 0.0);
			part_data.Add("Width", 0.0);
			part_data.Add("Color", " ");
			part_data.Add("Stock", 0);
			part_data.Add("MinStock", 0);
			part_data.Add("PriceClient", 0.0);

			//Dictionary that contains every suppliers
			Dictionary<string, object> dicoSuppliers = new Dictionary<string, object>();
			part_data.Add("PriceClient", dicoSuppliers);

			//Dictionary that contains every structure
			Dictionary<string, object> dicoStructure = new Dictionary<string, object>();
			part_data.Add("PriceClient", dicoStructure);

			//Dictionary that contains every Type
			Dictionary<string, object> dicoType = new Dictionary<string, object>();
			part_data.Add("PriceClient", dicoType);
		}

		public bool IsAvailable(int number){
			//A modifier en fonction de la base de donnée
			int stock = 16;
			if (number < stock)
			{
				return false;
			}
			return true;
		}
		public T GetData<T>(string key)
		{
			//A typecast is needed because the type 'object' is not converted implicitly
			return (T)Part_data[key];
		}
		public Dictionary<string,object> Part_data
		{
			get {return this.part_data;}
			set {this.part_data = value;}
		}
	}
}
