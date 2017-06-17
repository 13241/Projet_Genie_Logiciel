﻿using System;
using System.Collections.Generic;
using System.Windows.Media.Media3D;
using System.Drawing;
namespace Kitbox
{
	public static class DbCatalog
	{
		public static void DbConnectCatalog(string[] parameters)
		{
		}
		public static void DbRemoveFromStock (List<string> codes)
		{
		}
		public static void DbAddTostock(Dictionary<string, int> codes)
		{
		}
		public static void DbBook(string code)
		{
            BDD database = new BDD("kitbox");
            string selection = "Enstock, Reserve, Id";
            string table_name = "catalog";
            string condition = "WHERE (Code = '" + code + "');";
            List<List<object>> result = database.readElement(selection, table_name, condition);
            if(result.Count>0)
            {
                if (Convert.ToInt32(result[0][0]) > Convert.ToInt32(result[0][1]))
                {
                    string modification = "Reserve = '" + Convert.ToString(Convert.ToInt32(result[0][1]) + 1) +"'";
                    database.modifyElement(table_name, modification, Convert.ToString(result[0][2]));
                }
            }
        }
		public static void DbUnBook(string code)
		{
            BDD database = new BDD("kitbox");
            string selection = "Reserve, Id";
            string table_name = "catalog";
            string condition = "WHERE (Code = '" + code + "');";
            List<List<object>> result = database.readElement(selection, table_name, condition);
            if (result.Count > 0)
            {
                if (Convert.ToInt32(result[0][0]) > 0)
                {
                    string modification = "Reserve = '" + Convert.ToString(Convert.ToInt32(result[0][0]) - 1) + "'";
                    database.modifyElement(table_name, modification, Convert.ToString(result[0][1]));
                }
            }
        }
		public static Part DbSelectPart(Dictionary<string, string> selected_characteristics, string order_by = "")
		{
            BDD database = new BDD("kitbox");
            string selection = "Ref, Code, hauteur, profondeur, largeur, Couleur, PrixClient";
            string table_name = "catalog";
            string condition = "WHERE (";
            int counter = 0;
            foreach(string key in selected_characteristics.Keys)
            {
                counter++;
                condition += key + "='" + selected_characteristics[key] +"'";
                if(counter != selected_characteristics.Count)
                {
                    condition += " AND ";
                }
            }
            condition += ")";
            if(order_by != "")
            {
                condition += "ORDER BY " + order_by + ";";
            }
            Part request = null;
            List<List<object>> result = database.readElement(selection, table_name, condition);
            if(result.Count > 0)
            {
                if(result[0].Count == selection.Split(',').Length)
                {
                    Dictionary<string, object> data = new Dictionary<string, object>();
                    data["Reference"] = Convert.ToString(result[0][0]);
                    data["Code"] = Convert.ToString(result[0][1]);
                    data["Dimensions"] = new Size3D(Convert.ToDouble(result[0][4]), Convert.ToDouble(result[0][2]), Convert.ToDouble(result[0][3]));
                    data["Color"] = Color.FromName(TranslateColor(Convert.ToString(result[0][5])));
                    data["Selling_price"] = Convert.ToDouble(result[0][6]);

                    if(Convert.ToString(result[0][0]).Contains("Panneau") || Convert.ToString(result[0][0]).Contains("Traverse"))
                    {
                        request = new Panel();
                        request.SetData(data);
                    }
                    else if(Convert.ToString(result[0][0]).Contains("Porte"))
                    {
                        request = new Door();
                        request.SetData(data);
                    }
                    else if (Convert.ToString(result[0][0]).Contains("Coupelles"))
                    {
                        request = new Knop();
                        request.SetData(data);
                    }
                    else if (Convert.ToString(result[0][0]).Contains("Cornieres"))
                    {
                        request = new Angle();
                        request.SetData(data);
                    }
                    else if (Convert.ToString(result[0][0]).Contains("Tasseau"))
                    {
                        request = new Door();
                        request.SetData(data);
                    }
                }
            }
            return request;
		}
		public static Dictionary<string, object> DbGetOptions(string reference)
		{
            return null;
		}
        public static string TranslateColor(string fr)
        {
            switch(fr)
            {
                case "Brun":
                    return "Tan";
                case "Galvanise":
                    return "Gray";
                case "Blanc":
                    return "White";
                case "Noir":
                    return "Black";
                case "Verre":
                    return "LightBlue";
            }
            return "Transparent";
        }
        public static string TraduireCouleur(string en)
        {
            switch (en)
            {
                case "Tan":
                    return "Brun";
                case "Gray":
                    return "Galvanise";
                case "White":
                    return "Blanc";
                case "Black":
                    return "Noir";
                case "LightBlue":
                    return "Verre";
            }
            return "Transparent";
        }
	}
}


