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
		public Dictionary<string, object> GetBill(int order_id)
		{
			

			List<List<object>> list = new List<List<object>>();
			string RelTableName = "rel_Cat_Ord";
			string OrderTableName = "orders";
			string columnNames;
			string condition = string.Format("WHERE (Order_Id = {0})", order_id);
			double total_price = 0;

			bill["Header"] = DbOrder.DbSearchOrder(OrderTableName, "Header_Bill", string.Format("WHERE (Order_Id = {0} AND Client_Id = {1})", order_id, current_client));

			columnNames = "DISTINCT Wardrobe_Id";
			list = DbOrder.DbSearchOrder(RelTableName, columnNames, condition);
            for (int i = 1; i <= list.Count; i++)
            {
                bill["Components"] += string.Format("Wardrobe {0} :\n\t", i);
                columnNames = "DISTINCT Box_Id";
                condition = string.Format("WHERE (Order_Id = {0} AND Wardrobe_Id = {1})", order_id, i);

                list = DbOrder.DbSearchOrder(RelTableName, columnNames, condition);
                for (int j = 1; j <= list.Count; j++)
                {
                    bill["Components"] += string.Format("Box {0} :\n\t\t", i);
                    columnNames = "Component_Id";
                    condition = string.Format("WHERE (Order_Id = {0} AND Wardrobe_Id = {1} AND Box_Id = {2})", order_id, i, j);
                    list = DbOrder.DbSearchOrder(RelTableName, columnNames, condition);

                    for (int k = 1; k <= list.Count; k++)
                    {
                        //condition = Convert.ToString(list[k-1][0]);
                        columnNames = "Réf, Code, Dimensions, Couleur, hauteur, profondeur, largeur, Prix-Client";
                        condition = string.Format("WHERE Component_Id = {0}", Convert.ToString(list[k - 1][0]));
                        list = DbOrder.DbSearchOrder(OrderTableName, columnNames, condition);
                        foreach (List<object> component in list)
                        {
                            for (int spec_nbr = 0; spec_nbr < list.Count; spec_nbr++)
                            {
                                // In case it is the price
                                if (spec_nbr == list.Count - 1)
                                {
                                    bill["Components"] += string.Format("{0} €", list[spec_nbr]);
                                    total_price += Convert.ToDouble(component[spec_nbr]);
								}
								else if (spec_nbr == 0)
								{
									bill["Components"] += string.Format("\n\t\t{0}", component[spec_nbr]);
								}
								else
								{
									bill["Components"] += string.Format("{0} \t", component[spec_nbr]);
								}
							}
						}
					}
				}
			}

			bill["Footer"] = DbOrder.DbSearchOrder(OrderTableName, "Footer_Bill", string.Format("WHERE (Order_Id = {0} AND Client_Id = {1})", order_id, current_client));

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
