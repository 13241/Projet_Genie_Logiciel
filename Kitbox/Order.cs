
using System;
using System.Collections.Generic;
namespace Kitbox
{
	public class Order
	{
		private Person current_client;
		private List<object> wardrobes;
        private Dictionary<string, object> bill;
		private List<string> parts_list;
		private Dictionary<string, string> codes;
        private Double delayed;

        public Order()
        {
            wardrobes = new List<object>();
            bill = new Dictionary<string, object>();
            parts_list = new List<string>();
            codes = new Dictionary<string, string>();
            delayed = 0;
        }
		
		private string AddToBill()
		{
			string bill_return = "";
			bill_return += "Societe Kitbox\nAdresse :";
			return bill_return;

		}
		private List<string> AddToPartsList()
		{
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
		public List<List<object>> GetOrder(int client_id)
		{
			BDD database = new BDD("kitbox");
			List<object> listNumDate = new List<object>();
			List<List<object>> list = new List<List<object>>();

			string columnNames = "Order_Id,Date";
			string tableName = "orders";
			string condition = string.Format("WHERE (Client_Id = '{0}')", client_id);

			list = database.readElement(columnNames, tableName, condition);

			return list;
		}

        /// <summary>
        /// Returns a dictionnary containing all the information about an order
        /// </summary>
        /// <returns>The bill.</returns>
        /// <param name="order_id">Order identifier.</param>
		public Dictionary<string, object> GetBill(int order_id)
		{
            bill = new Dictionary<string, object>();
            Dictionary<string, Dictionary<string, List<object>>> components = new Dictionary<string, Dictionary<string, List<object>>>();
            int wardrobes = components.Count;
            List<List<object>> list = new List<List<object>>();

			string RelTableName = "rel_catord";
			string OrderTableName = "orders";
            string catalog_table_name = "catalog";
			string columnNames;
			string condition = string.Format("WHERE (Order_Id = '{0}')", order_id);

			list = DbOrder.DbSearchOrder(OrderTableName, "Header_Bill", string.Format("WHERE (Order_Id ='{0}' AND Client_Id ='{1}')", order_id, current_client.Id));
			bill.Add("Header", list[0][0]);
			columnNames = "DISTINCT Wardrobe_Id";

            // List of the wardrobe Id's
			List<List<object>> list1 = DbOrder.DbSearchOrder(RelTableName, columnNames, condition);
			bill.Add("Components", new Dictionary<string, Dictionary<string, List<object>>>());

            // Choose a wardrobe
			for (int i = 1; i <= list1.Count; i++)
			{
			    columnNames = "DISTINCT Box_Id";
				condition = string.Format("WHERE (Order_Id = '{0}' AND Wardrobe_Id = '{1}')", order_id, i);

                // Add the wardrobe as a dictionnary key if it doesn't already exist
                if(!components.ContainsKey(String.Format("Wardrobe{0}", i)))
                {
                    components.Add(String.Format("Wardrobe{0}", i), new Dictionary<string, List<object>>());
				}

                // Read all the Box_Id's of the database for this wardrobe and this order id
                List<List<object>>  list2 = DbOrder.DbSearchOrder(RelTableName, columnNames, condition);

				// Choose a box
				for (int j = 1; j <= list2.Count; j++)
                {
					columnNames = "Component_Id";
					condition = string.Format("WHERE (Order_Id = '{0}' AND Wardrobe_Id = '{1}' AND Box_Id = '{2}')", order_id, i, j-1);
					
					List<List<object>> list3 = DbOrder.DbSearchOrder(RelTableName, columnNames, condition);

					// Choose a component(part) of the box
					for (int k = 1; k <= list3.Count; k++)
					{
						columnNames = "Ref, Code, Dimensionscm, Couleur, hauteur, profondeur, largeur, PrixClient";
						condition = string.Format("WHERE Id = '{0}'", Convert.ToString(list3[k-1][0]));

                        // Read all the specifications of a component in the database
                        List<List<object>> list4 = DbOrder.DbSearchOrder(catalog_table_name, columnNames, condition);

                        // Choose the specifications of the component
						foreach (List<object> component in list4)
						{
                            
                            if(!components[String.Format("Wardrobe{0}", i)].ContainsKey(Convert.ToString(component[1])))
                            {
                                components[String.Format("Wardrobe{0}", i)].Add(Convert.ToString(component[1]),new List<object>());
                                List<object> component_code = components[String.Format("Wardrobe{0}", i)][Convert.ToString(component[1])];
                                //Quantity
                                component_code.Add(1);
                                //Price
                                component_code.Add(component[7]);
                                //Reference
                                component_code.Add(component[0]);
                                //Dimensions
                                component_code.Add(component[2]);
                                //Colour
                                component_code.Add(component[3]);
                                //Height
                                component_code.Add(component[4]);
                                //Depth
                                component_code.Add(component[5]);
                                //Width
                                component_code.Add(component[6]);
                            }

                            // In case the component is already in the dictionnary, we only increment the quantity
                            else 
                            {
                                List<object> component_code = components[String.Format("Wardrobe{0}", i)][Convert.ToString(component[1])];
                                int number = (int)component_code[0];
                                number += 1;
                                component_code[0] = number;
                            }
                        }
					}
				}
			}
            bill["Components"] = components;

			bill["Footer"] = DbOrder.DbSearchOrder(OrderTableName, "Footer_Bill", string.Format("WHERE (Order_Id = '{0}' AND Client_Id = '{1}')", order_id, current_client.Id))[0][0];

			return bill;
		}
		public List<string> PartsList { get => parts_list; set => parts_list = value; }

		public Dictionary<string, string> Codes { get => codes; set => codes = value; }

		public Person CurrentClient { get => current_client; set => current_client = value; }

		public List<object> Wardrobes { get => wardrobes; set => wardrobes = value; }
        /// <summary>
        /// Determine the maximum delay for the box
        /// </summary>
        public double Delayed
        {
            get
            {
                foreach(object ersatz in wardrobes)
                {
                    Wardrobe wardrobe = (Wardrobe)ersatz;
                    foreach(string part in wardrobe.Components.Keys)
                    {
                        foreach(string pos in wardrobe.Components[part].Keys)
                        {
                            if(typeof(Box).IsInstanceOfType(wardrobe.Components[part][pos]))
                            {
                                Box thisbox = (Box)wardrobe.Components[part][pos];
                                foreach(string piece in thisbox.Pieces.Keys)
                                {
                                    foreach(string position in thisbox.Pieces[piece].Keys)
                                    {
                                        delayed = Math.Max(delayed, Convert.ToDouble(thisbox.Pieces[piece][position].Delayed));
                                    }
                                }
                            }
                            else
                            {
                                delayed = Math.Max(delayed, Convert.ToDouble(((Part)wardrobe.Components[part][pos]).Delayed));
                            }
                        }
                    }
                }
                return delayed;
            }
        }
    }
}