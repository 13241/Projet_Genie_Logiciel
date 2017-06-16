using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kitbox
{
	static class DbConnect
	{
        static Person DbConnectClient(int id, string password)
		{
			BDD database = new BDD("kitbox");
			string tableName = "client";
            string columnNames = textString.columnNames(tableName);
            string condtion = string.Format("WHERE (Client_Id = {0} AND Password = {1})", id, password);
			List<List<object>> list = new List<List<object>>();
			list = database.readElement(columnNames, tableName, condtion);
            Person person = new Person();
            person.Id = id;
            person.Password = password;
            person.FirstName = Convert.ToString(list[0][1]);
            person.LastName = Convert.ToString(list[0][2]);
            person.PhoneNumber = Convert.ToInt16(list[0][3]);
            person.Email = Convert.ToString(list[0][4]);
            String[] split = Convert.ToString(list[0][6]).Split(';');
            person.Address["Street"] = split[0];
            person.Address["Street number"] = split[1];
            person.Address["Postal code"] = split[2];

            return person;
		}
		public static bool DbConnectEmployee(int seller_Id, string password)
		{
            return false;
		}
		// args is a string collection, example : "1", "PrixClient=4"
		// The first element of the collection is always the Id number of the item. 
		// In case of the example above, we're changing the values of the first item.
		public static void DbModifyStock(params string[] args)
		{
			BDD database = new BDD("kitbox");
			int size = args.Count();
			string number_Id = args[0];
			// Skip the first element of the string
			var array = args.Skip(1).ToArray();
			string data = string.Join(",", array);
			database.modifyElement("catalog", data, number_Id);
		}
		public static void DbAddClient(Person person)
		{
			BDD database = new BDD("kitbox");
			string firstname = person.FirstName;
			string lastname = person.LastName;
			int phonenumber = person.PhoneNumber;
			string email = person.Email;
			int id = person.Id;
			string password = person.Password;
			Dictionary<string, object> address = person.Address;
			string data = firstname + "," + lastname + "," + phonenumber + "," + email + "," + password + "," + address["Street"] + ";" + address["Street number"] + ";" + address["Postal code"];
			database.addElement("client_table", textString.columnNames("client_table"), data);
		}
		static bool DblsCLient(int id, string password)
		{
            BDD database = new BDD("kitbox");
			string tableName = "client";
			string columnNames = "Password";
            string condtion = string.Format("WHERE (Client_Id = {0})", id);
            List<List<object>> list = new List<List<object>>();
            list = database.readElement(columnNames, tableName, condtion);
            if(list.Count == 0)
            {
                return false;
            }
            else
            {
                if (Convert.ToString(list[0][0]) == password)
                {
                    return true;
                }
            }
            return false;
		}
		static bool DblsEmployee(int id, string password)
		{
			BDD database = new BDD("kitbox");
			List<List<object>> result = database.readElement("Seller_Id, Password", "seller", "WHERE Seller_Id=" + seller_Id);
			if (result.Count == 0)
			{
				return false;
			}
			else
			{
				if (Convert.ToString(result[0][0]) == password)
				{
					return true;
				}
			}
			return false;
		}
	}
}