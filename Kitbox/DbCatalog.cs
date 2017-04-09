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
		public Part DbSelectPart(Dictionary<string, object> selected_characteristics)
		{
            return null;
		}
		public Dictionary<string, object> DbGetOptions(string reference)
		{
            return null;
		}
	}
}


