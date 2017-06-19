using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kitbox
{
	static class DbConnect
	{
        public static Person DbConnectClient(int id, string password)
		{
			BDD database = new BDD("kitbox");
			string tableName = "client";
            string columnNames = "*";
            string condtion = string.Format("WHERE (Client_Id ='{0}' AND Password ='{1}')", id.ToString(), password);
			List<List<object>> list = new List<List<object>>();
			list = database.readElement(columnNames, tableName, condtion);
            Person person = new Person();
            person.Id = id;
            person.Password = password;
            person.FirstName = Convert.ToString(list[0][1]);
            person.LastName = Convert.ToString(list[0][2]);
            person.PhoneNumber = Convert.ToInt32(list[0][3]);
            person.Email = Convert.ToString(list[0][4]);
            String[] split = Convert.ToString(list[0][6]).Split(';');
            person.Address["Street"] = split[0];
            person.Address["Street number"] = split[1];
            person.Address["Postal code"] = split[2];

            return person;
		}
		public static bool DbConnectEmployee(int seller_id, string password)
		{
            BDD database = new BDD("kitbox");
            List<List<object>> list = new List<List<object>>();
            string selection = "Seller_id, Password";
            string table_name = "seller";
            string condition = string.Format("WHERE (Seller_Id = '{0}' AND Password = '{1}')", seller_id.ToString(), password);
            list = database.readElement(selection,table_name,condition);
            if (list.Count == 0)
            {
                return false;
            }
            return true;
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
			string data = "'"+firstname +"','" + lastname + "'," + phonenumber + ",'" + email + "','" + password + "','" + address["Street"] + ";" + Convert.ToString(address["Street number"]) + ";" + address["Postal code"].ToString()+"',''";
			database.addElement("client", "Firstname,Lastname,Phonenumber,Email,Password,Address,Favoris", data);
		}
		public static bool DblsCLient(int id, string password)
		{
            BDD database = new BDD("kitbox");
			string tableName = "client";
			string columnNames = "Password";
            string condtion = string.Format("WHERE (Client_Id = '{0}')", id);
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


        public static Person searchClient(int id)
		{
            BDD database = new BDD("kitbox");
			string tableName = "client";
			string columnNames = "Firstname,Lastname";
            string condtion = string.Format("WHERE (Client_Id = '{0}')", id);
            List<List<object>> list = new List<List<object>>();
            list = database.readElement(columnNames, tableName, condtion);

            Person person = new Person();
            if (list.Count == 0)
            {
                return null;
            }
            else
            {
                person.FirstName = Convert.ToString(list[0][0]);
                person.LastName = Convert.ToString(list[0][1]);
                return person;
            }         
            
        }
		public static int searchId(int phonenumber)
		{
			BDD database = new BDD("kitbox");
			string tableName = "client";
			string columnNames = "Client_Id";
			string condtion = string.Format("WHERE (Phonenumber = '{0}')", phonenumber);
			List<List<object>> list = new List<List<object>>();
			list = database.readElement(columnNames, tableName, condtion);

			Person person = new Person();
			if (list.Count == 0)
			{
				return 0;
			}
			else
			{
				person.Id = Convert.ToInt32(list[0][0]);
				return person.Id;
			}

		}

		public static bool DblsEmployee(int id, string password)
		{
			BDD database = new BDD("kitbox");
			List<List<object>> result = database.readElement("Seller_Id, Password", "seller", "WHERE Seller_Id='" + id + "'");
			if (result.Count == 0)
			{
				return false;
			}
			else
			{
				if (Convert.ToString(result[0][1]) == password)
				{
					return true;
				}
			}
			return false;
		}
	}
}
