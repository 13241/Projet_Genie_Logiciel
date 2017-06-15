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
            BDD database = new BDD("Kitbox");
            //string columnNames=
        }
        static void DbConnectEmployee(params string[] args)
        {

        }
        static void DbAddClient(Person person)
        {
            BDD database = new BDD("Kitbox");
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
