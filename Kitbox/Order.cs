
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
            Dictionary<string, Dictionary<string, List<object>>> components = new Dictionary<string, Dictionary<string, List<object>>>();
            //!!!!!!!!!!!!!! We can just make a components.Count to  know how many wardrobes there are
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
            // list of the wardrobe Ids
			List<List<object>> list1 = DbOrder.DbSearchOrder(RelTableName, columnNames, condition);
			bill.Add("Components", new Dictionary<string, Dictionary<string, List<object>>>());
			for (int i = 1; i <= list1.Count; i++)
			{
                //components.Add(String.Format("Wardrobe{0}", i));
				//bill["Components"] += string.Format("Wardrobe {0} :\n\t", i);
				columnNames = "DISTINCT Box_Id";
				condition = string.Format("WHERE (Order_Id = '{0}' AND Wardrobe_Id = '{1}')", order_id, i);
                if(!components.ContainsKey(String.Format("Wardrobe{0}", i)))
                {
                    //components[String.Format("Wardrobe{0}", i)] = new Dictionary<string, List<object>>();
                    components.Add(String.Format("Wardrobe{0}", i), new Dictionary<string, List<object>>());
				}
                // list of the box Id depending on Order_Id and Wardrobe_Id
                List<List<object>>  list2 = DbOrder.DbSearchOrder(RelTableName, columnNames, condition);
				for (int j = 1; j <= list2.Count; j++)
				{
                    
					//bill["Bill"] += string.Format("Box {0} :\n\t\t", i);
					columnNames = "Component_Id";
					condition = string.Format("WHERE (Order_Id = '{0}' AND Wardrobe_Id = '{1}' AND Box_Id = '{2}')", order_id, i, j);
					
					List<List<object>> list3 = DbOrder.DbSearchOrder(RelTableName, columnNames, condition);

					for (int k = 1; k <= list3.Count; k++)
					{
                        //components.Add(String.Format("Wardrobe{0}", i));
						//condition = Convert.ToString(list[k-1][0]);
						columnNames = "Ref, Code, Dimensionscm, Couleur, hauteur, profondeur, largeur, PrixClient";
						condition = string.Format("WHERE Id = '{0}'", Convert.ToString(list3[k-1][0]));
                        List<List<object>> list4 = DbOrder.DbSearchOrder(catalog_table_name, columnNames, condition);
						foreach (List<object> component in list4)
						{
							//for (int spec_nbr = 0; spec_nbr < list.Count; spec_nbr++)
							//{
                            
                                if(!components[String.Format("Wardrobe{0}", i)].ContainsKey(Convert.ToString(component[1])))
                                {
                                    components[String.Format("Wardrobe{0}", i)].Add(Convert.ToString(component[1]),new List<object>());
                                    List<object> component_code = components[String.Format("Wardrobe{0}", i)][Convert.ToString(component[1])];
                                    //component_code = new List<object>();
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
                                else 
                                {
                                    List<object> component_code = components[String.Format("Wardrobe{0}", i)][Convert.ToString(component[1])];
                                    int number = (int)component_code[0];
                                    number += 1;
                                    component_code[0] = number;
                            }

                            ////Price
                            //component_code[1] = component[7];
                            ////Reference
                            //component_code[2] = component[0];
                            ////Dimensions
                            //component_code[3] = component[2];
                            ////Colour
                            //component_code[4] = component[3];
                            ////Height
                            //component_code[5] = component[4];
                            ////Depth
                            //component_code[6] = component[5];
                            ////Width
                            //component_code[7] = component[6];

                            // In case it is the price
                            //if (spec_nbr == list.Count - 1)
                            //{
                            //	bill["Components"] += string.Format("{0} €", list[spec_nbr]);
                            //	total_price += Convert.ToDouble(component[spec_nbr]);
                            //}
                            //else if (spec_nbr == 0)
                            //{
                            //	bill["Components"] += string.Format("\n\t\t{0}", component[spec_nbr]);
                            //}
                            //else
                            //{
                            //	bill["Components"] += string.Format("{0} \t", component[spec_nbr]);
                            //}
                            //}
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

	}
}