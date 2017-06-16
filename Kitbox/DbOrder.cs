using System;
using System.Collections.Generic;
<<<<<<< Updated upstream
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
=======
<<<<<<< HEAD
<<<<<<< HEAD
=======
=======
>>>>>>> fe8304e270888cf26cae1d1c28be8fcf5f14532b
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
<<<<<<< HEAD
using System.Data.SqlClient;

>>>>>>> ac6f694f81b607de6717b43523c2fbff11154698
=======

>>>>>>> fe8304e270888cf26cae1d1c28be8fcf5f14532b
namespace Kitbox
{
	public class DbOrder
	{
		public DbOrder()
		{
		}
		public void DbConnectOrder(string[] parameters)
		{
<<<<<<< HEAD
<<<<<<< HEAD
			
=======
=======
>>>>>>> fe8304e270888cf26cae1d1c28be8fcf5f14532b
			// https://www.codeproject.com/Articles/823854/How-to-connect-SQL-Database-to-your-Csharp-program
			SqlConnection conn = new SqlConnection();
			conn.ConnectionString = "connection_string";
			conn.Open();
>>>>>>> Stashed changes

            /////////////Use of the Bill dictionary

            Dictionary<string, object> bill;
            //bill = textString.bill();

<<<<<<< Updated upstream
            ////////////Components part list
=======
			// Create new SqlDataReader object and read data from the command.
			using (SqlDataReader reader = command.ExecuteReader())
			{
				// while there is another record present
				while (reader.Read())
				{
					// write the data on to the screen
					Console.WriteLine(String.Format("{0} \t | {1} \t | {2} \t | {3}",
					// call the objects from their index
					reader[0], reader[1], reader[2], reader[3]));
				}
			}
<<<<<<< HEAD
>>>>>>> ac6f694f81b607de6717b43523c2fbff11154698
=======
>>>>>>> fe8304e270888cf26cae1d1c28be8fcf5f14532b
		}
		public void DbAddOrder(Order order)
		{
>>>>>>> Stashed changes

            //List<string> Parts_list = new List<string>();

            return order;
        }
    }
}
