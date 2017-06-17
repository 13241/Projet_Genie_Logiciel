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
            string dateString = date.ToString();
            string data = string.Format("{0},'{1}','{2}','{3}'",client_id,dateString,header_bill,footer_bill);
            database.addElement(tableName, columnNames, data);
            List<List<object>> list = database.readElement("Order_Id", tableName, string.Format("WHERE client_id = {0}", client_id.ToString()));
            int order_id = Convert.ToInt32(list[list.Count - 1][0]);
            List<object> wardrobes = order.Wardrobes;
            //int number_box = wardrobes.Components["Etage"].Count;
            tableName = "rel_catord";
            columnNames = "Order_Id ,Wardrobe_Id,Box_Id,Component_Id";
            data = "";
            for (int i = 1 ; i <= wardrobes.Count ; i++)
            {
                Wardrobe wardrobe = (Wardrobe) wardrobes[i - 1];
                int number_box = wardrobe.Components["Etage"].Count;
                data = string.Format("{0},{1},{2},{3}",order_id.ToString(),i.ToString(),"","");
                //if(.GetType==typeof())
                foreach (KeyValuePair<string, object> cornieres in wardrobe.Components["Cornieres"])
                {
                    Angle angle = (Angle)cornieres.Value;
                    string Id = database.readElement("Id", "catalog", string.Format("WHERE Code='{0}'", angle.Code.ToString()))[0][0].ToString();
                    data = string.Format("'{0}','{1}','{2}','{3}'", order_id.ToString(), i.ToString(), "0", Id);
                    database.addElement(tableName, columnNames, data);
                    //for(int k = 1; <=)
                }
                for (int j = 1 ; j <= number_box; j++ )
                {
                    data = string.Format("{0},{1},{2},{3}", order_id.ToString(), i.ToString(), j.ToString(), "");
                    foreach (KeyValuePair<string, object> kvp in wardrobe.Components["Etage"])
                    {
                        if (Convert.ToInt32(kvp.Key) == j)
                        {
                            Box box = (Box) wardrobe.Components["Etage"][j.ToString()];
                            Dictionary<string, Dictionary<string, Part>> parts = box.Pieces;
                            foreach (KeyValuePair<string, Dictionary<string, Part>> kvp2 in parts)
                            {
                                foreach (KeyValuePair<string, Part> kvp3 in kvp2.Value)
                                {
                                    string Id = database.readElement("Id","catalog",string.Format("WHERE Code='{0}'",kvp3.Value.Code.ToString()))[0][0].ToString();
                                    data = string.Format("{0},{1},{2},{3}", order_id.ToString(), i.ToString(), j.ToString(), Id);
                                    database.addElement(tableName, columnNames,data);
                                }
                            }
                        }
                    }
                    //for(int k = 1; <=)
                }
            }

            //foreach(object component in wardrobes)
            //{
            //    if (component.GetType() == typeof(Box))
            //    {

            //    }
            //}

            //Dictionary<string, Dictionary<string, object>> components = wardrobes.
        }
        static Dictionary<string, object> DbSearchClient(int id_client)
        {
            Dictionary<string, object> order = new Dictionary<string, object>();

            /////////////Use of the Bill dictionary

            Dictionary<string, object> bill;
            bill = textString.bill();

            ////////////Components part list

            //List<string> Parts_list = new List<string>();

            return order;
        }

		public static List<List<object>> DbSearchOrder(string tableName, string columnNames, string condition)
		{
			List<List<object>> result = new List<List<object>>();

			//string columnNames = "DISTINCT Component_Id, Wardrobe_Id, Box_ID";
			//string tableName = "rel_Cat_Ord";
			//string condition = string.Format("WHERE (Order_Id = {0})", order_id);
			BDD database = new BDD("kitbox");

			result = database.readElement(columnNames, tableName, condition);

			return result;
		}
    }
}
