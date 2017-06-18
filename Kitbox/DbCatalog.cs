using System;
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
		public static string DbRemoveFromStock (string code, int ratio_command = 2)
		{
            BDD database = new BDD("kitbox");
            string selection = "Enstock, Reserve, Stock_minimum, PrixFourn1, DelaiFourn1, CommandeFourn1, PrixFourn2, DelaiFourn2, CommandeFourn2, Id";
            string table_name = "catalog";
            string condition = "WHERE (Code = '" + code + "');";
            List<List<object>> result = database.readElement(selection, table_name, condition);
            string delayed = "0";
            if (result.Count > 0)
            {
                if (Convert.ToInt32(result[0][1]) > 0)
                {
                    string modification = "Enstock = '" + Convert.ToString(Convert.ToInt32(result[0][0]) - 1) + "'";
                    modification += ", Reserve = '" + Convert.ToString(Convert.ToInt32(result[0][1]) - 1) + "'";
                    if(Convert.ToInt32(result[0][0]) - 1 < Convert.ToInt32(result[0][2]) && Convert.ToInt32(result[0][5]) == 0 && Convert.ToInt32(result[0][8]) == 0)
                    {
                        if(Convert.ToDouble(result[0][3]) < Convert.ToDouble(result[0][6]))
                        {
                            modification += ", CommandeFourn1 = '" + Convert.ToString(Convert.ToInt32(result[0][2]) * ratio_command) + "'";
                        }
                        else if(Convert.ToDouble(result[0][3]) > Convert.ToDouble(result[0][6]))
                        {
                            modification += ", CommandeFourn2 = '" + Convert.ToString(Convert.ToInt32(result[0][2]) * ratio_command) + "'";
                        }
                        else
                        {
                            if(Convert.ToDouble(result[0][4]) <= Convert.ToDouble(result[0][7]))
                            {
                                modification += ", CommandeFourn1 = '" + Convert.ToString(Convert.ToInt32(result[0][2]) * ratio_command) + "'";
                            }
                            else
                            {
                                modification += ", CommandeFourn2 = '" + Convert.ToString(Convert.ToInt32(result[0][2]) * ratio_command) + "'";
                            }
                        }
                    }
                }
                if(Convert.ToInt32(result[0][0]) < 1)
                {
                    if (Convert.ToInt32(result[0][5]) > 0)
                    {
                        delayed = Convert.ToString(result[0][4]);
                    }
                    else if (Convert.ToInt32(result[0][8]) > 0)
                    {
                        delayed = Convert.ToString(result[0][7]);
                    }
                    else
                    {
                        delayed = "-1";
                    }
                }
            }
            return delayed;
		}
		public static void DbAddTostock(Dictionary<string, int> codes)
		{

		}
		public static string DbBook(string code)
		{
            BDD database = new BDD("kitbox");
            string selection = "Enstock, Reserve, Id, DelaiFourn1, CommandeFourn1, DelaiFourn2, CommandeFourn2";
            string table_name = "catalog";
            string condition = "WHERE (Code = '" + code + "');";
            List<List<object>> result = database.readElement(selection, table_name, condition);
            string delayed = "0";
            if(result.Count>0)
            {
                string modification = "Reserve = '" + Convert.ToString(Convert.ToInt32(result[0][1]) + 1) +"'";
                database.modifyElement(table_name, modification, Convert.ToString(result[0][2]));
                if (Convert.ToInt32(result[0][0]) < Convert.ToInt32(result[0][1]))
                {
                    if (Convert.ToInt32(result[0][4]) > 0)
                    {
                        delayed = Convert.ToString(result[0][3]);
                    }
                    else if (Convert.ToInt32(result[0][6]) > 0)
                    {
                        delayed = Convert.ToString(result[0][5]);
                    }
                    else
                    {
                        delayed = "-1";
                    }
                }
            }
            return delayed;
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

        // need ref + 3 dims
		public static List<string> DbGetColors(Dictionary<string, string> selected_characteristics)
		{
            BDD database = new BDD("kitbox");
            string selection = "Couleur";
            string table_name = "catalog";
            string condition = "WHERE (";
            int counter = 0;
            foreach (string key in selected_characteristics.Keys)
            {
                counter++;
                condition += key + "='" + selected_characteristics[key] + "'";
                if (counter != selected_characteristics.Count)
                {
                    condition += " AND ";
                }
            }
            condition += ")";
            List<List<object>> result = database.readElement(selection, table_name, condition);
            List<string> colors = new List<string>();
            foreach(List<object> color in result)
            {
                colors.Add(Convert.ToString(color[0]));
            }
            return colors;
        }
        
        //GD ou Ar
        public static List<string> DbGetLateralDimOpt(string lateral_dim)
        {
            BDD database = new BDD("kitbox");
            string selection = "largeur";
            string table_name = "catalog";
            string condition = "WHERE (";
            Dictionary<string, string> selected_characteristics = new Dictionary<string, string>()
            {
                { "Ref", "Panneau " + lateral_dim },
                { "hauteur", Convert.ToString(32) },
                { "profondeur", Convert.ToString(0) },
                { "couleur", "Blanc" }
            };
            int counter = 0;
            foreach (string key in selected_characteristics.Keys)
            {
                counter++;
                condition += key + "='" + selected_characteristics[key] + "'";
                if (counter != selected_characteristics.Count)
                {
                    condition += " AND ";
                }
            }
            condition += ")";
            List<List<object>> result = database.readElement(selection, table_name, condition);
            List<string> laterals = new List<string>();
            foreach (List<object> lateral in result)
            {
                laterals.Add(Convert.ToString(lateral[0]));
            }
            return laterals;
        }

        public static List<string> DbGetHeightOpt(double h_box, double h_wardrobe)
        {
            BDD database = new BDD("kitbox");
            string selection = "hauteur";
            string table_name = "catalog";
            string condition = "WHERE (";
            Dictionary<string, string> selected_characteristics = new Dictionary<string, string>()
            {
                { "Ref", "Panneau Ar" },
                { "largeur", Convert.ToString(32) },
                { "profondeur", Convert.ToString(0) },
                { "couleur", "Blanc" }
            };
            int counter = 0;
            foreach (string key in selected_characteristics.Keys)
            {
                counter++;
                condition += key + "='" + selected_characteristics[key] + "'";
                if (counter != selected_characteristics.Count)
                {
                    condition += " AND ";
                }
            }
            condition += ")";
            List<List<object>> result = database.readElement(selection, table_name, condition);

            condition = "WHERE (Ref = 'Cornieres') ORDER BY hauteur DESC;";
            List<List<object>> cornieres = database.readElement(selection, table_name, condition);
            double h_max = Convert.ToDouble(cornieres[0][0]);

            List<string> hs = new List<string>();
            foreach (List<object> h in result)
            {
                if(h_wardrobe - h_box + 4 + Convert.ToDouble(h[0]) <= h_max)
                {
                    hs.Add(Convert.ToString(4 + Convert.ToDouble(h[0])));
                }
            }

            return hs;
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


