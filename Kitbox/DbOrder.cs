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
        static void DbAddOrder(Order order)
        {
            string columnNames = "Order_Id, Client_Id, Date, Header_Bill, Footer_Bill";
            string tableName = "order_informations";
            string Client_Id = order.GetCurrentClient.GetId.ToString();
            BDD database = new BDD("kitbox");
            DateTime date = DateTime.Now;
            string dateString = date.ToString();
            string data = "1," + Client_Id + ","+dateString+",Header_Bill,Footer_Bill";
            database.addElement(tableName, columnNames, data);
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
    }
}
