using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace test2
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<string, object> bill = new Dictionary<string, object>();
            string header = "Kitbox Fabric" + "\n" + "Avenue du Chocolat 50" + "\n" + "1400 Nivelles" + "\n" + "Tél: 02 99 99 99 99" + "  " + "TVA: BE08XXXXXXXX" + "\n" + "www.kitboxfabric.com";
            string footer = "Merci à bientôt - Dank u tot ziens" + "\n" + "    Ouvert de 10 à 20h" + "\n" + "   Open van 10u tot 20u";
            string components = "";

            bill.Add("header", header);
            bill.Add("components", components);
            bill.Add("footer", footer); // Add Total

            /*  Code permettant d'afficher le contenu du Bill
             
     
            Console.WriteLine(bill["header"]);

            / Console.WriteLine(bill["components"]);   Add Number - Description - Price of components

            Console.WriteLine("\n" + bill["footer"]);
            Console.ReadLine();

            */
            
        }
    }
}
