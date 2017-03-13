using System;
using System.Collections.Generic;
namespace Kitbox
{
	public class Order
	{
		protected Person current_client;
		protected List<Wardrobe> wardrobes;
		protected Dictionary<string, object> bill;
		protected List<string> parts_list;
		protected Dictionary<string, string> codes;
		public Order()
		{
		}

		private string AddToBill()
		{
			string bill_return = "";
			bill_return += "Societe Kitbox\nAdresse :";
			return bill_return;

		}
		private List<string> AddToPartsList()
		{
			// acceder a la liste de box pour recuperer chaque piece de chaque box?
			parts_list.Add(wardrobes[-1]);
			return parts_list;
		}
		public void PrintAll()
		{

		}
		public void AddWardrobe(int index)
		{
			AddToBill();
			AddToPartsList();
		}
		public void RemoveWardrobe(int index)
		{

		}
		public Dictionary<string, object> GetBill()
		{

			// on renvoie juste le dico ???
			return bill;
		}
		public List<string> GetPartsList()
		{
			// on renvoie juste la liste ???
			return parts_list;
		}
		public Dictionary<string, string> GetCodes()
		{
			// on renvoie juste le dico???
			return codes;
		}
		public Person GetCurrentClient
		{
			get { return this.current_client;}
		}
		public List<Wardrobe> GetWardrobes
		{
			get { return this.wardrobes;}
		}
	}
}
