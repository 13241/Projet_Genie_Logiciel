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
			
		}
		private List<string> AddToPartsList()
		{
			
		}
		public void PrintAll()
		{
			
		}
	}
}
