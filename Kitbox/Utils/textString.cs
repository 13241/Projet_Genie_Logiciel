using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kitbox
{
    public static class textString
    {
        public static Dictionary<string, object> bill()
        {
            Dictionary<string, object> bill = new Dictionary<string, object>();
            List<string> components = new List<string>();
            string header = "Kitbox Fabric" + "\n" + "Avenue du Chocolat 50" + "\n" + "1400 Nivelles" + "\n" + "Tél: 02 99 99 99 99" + "  " + "TVA: BE08XXXXXXXX" + "\n" + "www.kitboxfabric.com";
            string footer = "Merci à bientôt - Dank u tot ziens" + "\n" + "    Ouvert de 10 à 20h" + "\n" + "   Open van 10u tot 20u";
            bill.Add("header", header);
            bill.Add("components", components);
            bill.Add("footer", footer); // Add Total
            return bill;
        }
        public static string columnNames(string table)
        {
            string columns = "";
            if (table == "component") { columns = "Id, Ref, Code, Dimensionscm, hauteur, profondeur, largeur, Couleur, Enstock, Stock_minimum, PrixClient, NbPiecescasier, PrixFourn_1, DelaiFourn_1, PrixFourn2, DelaiFourn2, Reserve"; }
            else if (table == "orders") { columns = "Order_id,CLient_Id,Date,Header_Bill,Footer_Bill"; }
            else if (table == "client") { columns = "Client_Id,Firstname,Lastname,Phonenumber,Email,Password,Address,Favoris"; }
            return columns;
        }
    }
}
