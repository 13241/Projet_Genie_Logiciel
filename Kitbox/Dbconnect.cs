using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kitbox
{
    static class DbConnect
    {
        static void DbConnectClient(params string[] args)
        {
            BDD database = new BDD("kitbox");
            //string columnNames=
        }
        public static bool DbConnectEmployee(int seller_Id, string password)
        {
            BDD database = new BDD("kitbox");
            List<List<object>> result = database.readElement("Seller_Id, Password","seller","WHERE Seller_Id="+seller_Id);
            if (result[0].Count == 0)
            {
                Console.WriteLine("Vous n'apparaissez pas dans la base de donnée");
                return false;
            }
            else if ((string) result[0][1] != password)
            {
                Console.WriteLine("Mot de passe incorrect");
                return false;
            }
            Console.WriteLine("Connexion réussie !");
            return true;
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
            string data = string.Join(",",array);
            database.modifyElement("catalog",data,number_Id);
        }
        public static void DbAddClient(Person person)
        {
            BDD database = new BDD("kitbox");
            string firstname = person.GetFirstName;
            string lastname = person.GetLastName;
            int phonenumber = person.GetPhoneNumber;
            string email = person.GetEmails;
            int id = person.GetId;
            string password = person.GetPassword;
            Dictionary<string, object> address = person.GetAddress;
            string data = firstname + "," + lastname + "," + phonenumber + "," + email + "," + password + "," + address["Street"] + ";" + address["Street number"] + ";" + address["Postal code"];
            database.addElement("client_table", textString.columnNames("client_table"), data);
        }
        static bool DblsCLient(int id,string password)
        {
            return true;
        }
        static bool DblsEmployee(int id,string password)
        {
            return true;
        }
    }
}
