using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;


namespace Kitbox
{
    static class DbOrder
    {
		/// <summary>
		/// Adds in the database the order given in the parameter, fills 
        /// the tables with the components and links them to the current 
        /// client and his the ordrer. The order must have a current_client
        /// with an existing id
		/// </summary>
		/// <param name="order">Order.</param>
		public static void DbAddOrder(Order order)
        {
            BDD database = new BDD("kitbox");
            Dictionary<string, object> bill = textString.bill();
            Dictionary<string, Dictionary<string, object>> components = new Dictionary<string, Dictionary<string, object>>();
            string header_bill = bill["header"].ToString();
            string footer_bill = bill["footer"].ToString();
            string columnNames = "Client_Id, Date, Header_Bill, Footer_Bill";
            string tableName = "orders";
            string client_id = order.CurrentClient.Id.ToString();
            DateTime date = DateTime.Now;
            date.AddDays(order.Delayed);
            string dateString = date.ToString();
            string data = string.Format("{0},'{1}','{2}','{3}'", client_id, dateString, header_bill, footer_bill);
			// The order id is auto incremented in the orders table and we add 
			// in the orders table : the client id, the date, the header bill and the footer bill
			database.addElement(tableName, columnNames, data);
            List<List<object>> list = database.readElement("Order_Id", tableName, string.Format("WHERE Client_Id = '{0}'", client_id.ToString()));
            int order_id = Convert.ToInt32(list[list.Count - 1][0]);
            List<object> wardrobes = order.Wardrobes;
            tableName = "rel_catord";
            columnNames = "Order_Id ,Wardrobe_Id,Box_Id,Component_Id";
            data = "";
            // Choose a wardrobe in the list of wardrobes
            for (int i = 1 ; i <= wardrobes.Count ; i++)
            {
                Wardrobe wardrobe = (Wardrobe) wardrobes[i - 1];
                int number_box = wardrobe.Components["Etage"].Count;
                data = string.Format("{0},{1},{2},{3}",order_id.ToString(),i.ToString(),"","");
                // Choose an Angle
                foreach (KeyValuePair<string, object> cornieres in wardrobe.Components["Cornieres"])
                {
                    Angle angle = (Angle)cornieres.Value;
                    string Id = database.readElement("Id", "catalog", string.Format("WHERE Code='{0}'", angle.Code.ToString()))[0][0].ToString();
                    data = string.Format("'{0}','{1}','{2}','{3}'", order_id.ToString(), i.ToString(), "0", Id);
                    // Add the corner and its information in the table
                    database.addElement(tableName, columnNames, data);
                }
                // Choose a box of the wardrobe
                for (int j = 1 ; j <= number_box; j++ )
                {
                    data = string.Format("{0},{1},{2},{3}", order_id.ToString(), i.ToString(), j.ToString(), "");
                    // The "Etage" key gives all the boxes
                    foreach (KeyValuePair<string, object> kvp in wardrobe.Components["Etage"])
                    {
                        if (Convert.ToInt32(kvp.Key) == j)
                        {
                            Box box = (Box) wardrobe.Components["Etage"][j.ToString()];
                            Dictionary<string, Dictionary<string, Part>> parts = box.Pieces;
                            foreach (KeyValuePair<string, Dictionary<string, Part>> kvp2 in parts)
                            {
                                // Choose a part
                                foreach (KeyValuePair<string, Part> kvp3 in kvp2.Value)
                                {
                                    string Id = database.readElement("Id","catalog",string.Format("WHERE Code='{0}'",kvp3.Value.Code.ToString()))[0][0].ToString();
                                    data = string.Format("{0},{1},{2},{3}", order_id.ToString(), i.ToString(), j.ToString(), Id);
                                    // Add the part of the box and its information in the database
                                    database.addElement(tableName, columnNames,data);
                                }
                            }
                        }
                    }
                }
            }
        }

        static Dictionary<string, object> DbSearchClient(int id_client)
        {
            Dictionary<string, object> order = new Dictionary<string, object>();
            Dictionary<string, object> bill;
            bill = textString.bill();
            return order;
        }

        /// <summary>
        /// Returns a table containing the information of the table, the columns
        /// and the condition
        /// </summary>
        /// <returns>The search order.</returns>
        /// <param name="tableName">Table name.</param>
        /// <param name="columnNames">Column names.</param>
        /// <param name="condition">Condition.</param>
		public static List<List<object>> DbSearchOrder(string tableName, string columnNames, string condition)
		{
			List<List<object>> result = new List<List<object>>();
			BDD database = new BDD("kitbox");
			result = database.readElement(columnNames, tableName, condition);
			return result;
		}
    }
}
