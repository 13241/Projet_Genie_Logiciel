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
