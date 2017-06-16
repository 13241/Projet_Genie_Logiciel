using System;
using System.Collections.Generic;
namespace Kitbox
{
	public class DbCatalog
	{
		public DbCatalog()
		{
		}
		public void DbConnectCatalog(string[] parameters)
		{
		}
		public void DbRemoveFromStock (List<string> codes)
		{
		}
		public void DbAddTostock(Dictionary<string, int> codes)
		{
		}
		public void DbBook(List<string> codes)
		{
		}
		public void DbUnBook(List<string> codes)
		{
		}
		public Part DbSelectPart(Dictionary<string, string> selected_characteristics)
		{
            BDD database = new BDD("kitbox");
            string selection = "*";
            string table_name = "catalog";
            string condition = "WHERE (";
            int counter = 0;
            foreach(string key in selected_characteristics.Keys)
            {
                counter++;
                condition += key + "=" + selected_characteristics[key];
                if(counter != selected_characteristics.Count)
                {
                    condition += "AND";
                }
            }
            List<List<object>> result = database.readElement(selection, table_name, condition);
            
            
            return null;
		}
		public Dictionary<string, object> DbGetOptions(string reference)
		{
            return null;
		}
	}
}


