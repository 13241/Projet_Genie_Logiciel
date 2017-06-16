
using System;
using System.Collections.Generic;
namespace Kitbox
{
	public class Order
	{
		protected Person current_client;
		protected List<object> wardrobes = new List<object>();
        protected Dictionary<string, object> bill = new Dictionary<string, object>();
        protected List<string> parts_list = new List<string>();
        protected Dictionary<string, string> codes = new Dictionary<string, string>();
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
            //Dictionary<string, Dictionary<string, List<Dictionary<string, List<object>>>>> bill = new Dictionary<string, Dictionary<string, List<Dictionary<string, List<object>>>>>();
            List<List<object>> list = new List<List<object>>();

            string RelTableName = "rel_catord";
            string OrderTableName = "orders";
            string columnNames;
            string condition = string.Format("WHERE (Order_Id = {0})", order_id);
            double total_price = 0;

             
            list = DbOrder.DbSearchOrder(OrderTableName, "Header_Bill", string.Format("WHERE (Order_Id ='{0}' AND Client_Id ='{1}')", order_id, current_client.Id));
            bill.Add("Header",list[0][0]);
			columnNames = "DISTINCT Wardrobe_Id";
			list = DbOrder.DbSearchOrder(RelTableName, columnNames, condition);
            bill.Add("Components","");
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
                    //Pas celui la
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
            //Ici
			bill["Footer"] = DbOrder.DbSearchOrder(OrderTableName, "Footer_Bill", string.Format("WHERE (Order_Id = {0} AND Client_Id = {1})", order_id, current_client.Id));

			return bill;
		}
		public List<string> PartsList{ get => parts_list; set => parts_list = value; }
		
		public Dictionary<string, string> Codes{ get => codes; set => codes = value; }
		
		public Person CurrentClient { get => current_client; set => current_client = value; }

		public List<object> Wardrobes{ get => wardrobes; set => wardrobes = value; }
		
	}
}
