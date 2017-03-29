using System;
using System.Collections.Generic;
<<<<<<< HEAD
=======
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

>>>>>>> ac6f694f81b607de6717b43523c2fbff11154698
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
			
=======
			// https://www.codeproject.com/Articles/823854/How-to-connect-SQL-Database-to-your-Csharp-program
			SqlConnection conn = new SqlConnection();
			conn.ConnectionString = "connection_string";
			conn.Open();

			// use the connection here

			conn.Close();
			conn.Dipose();
			SqlCommand command = new SqlCommand("SELECT * FROM TableName", conn);
			SqlCommand command = new SqlCommand("SELECT * FROM TableName WHERE FirstColumn = @0", conn);

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
>>>>>>> ac6f694f81b607de6717b43523c2fbff11154698
		}
		public void DbAddOrder(Order order)
		{

		}
		public Dictionary<string, object> DbSearchClient(int id_client)
		{

		}
	}
}
