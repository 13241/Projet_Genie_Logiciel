using System;
using System.Collections.Generic;
namespace Kitbox
{
	public class Dbconnect
	{
		public Dbconnect()
		{
		}
		public void DbconnectClient(string[] parameters) //on se connecte à la base de donnée
		{												/et on vérifie si les identifiants sont correctes
		 
		}
		public void DbConnectEmployee(string[] parameters) 
		//Identification requise au début, options supplémentaires disponibles
		//(récupérer commande d'un client, modifier commande en face du client et enregister le client dans la Bdd).
		{
		}
		public void DbAddClient(Person person)
		{
		}
		public Boolean DblsClient(int id, string password)
		{
			//return true or false
		}
		public Boolean DblsEmployee(int id, string password)
		{
			// return true or false
		}
	}
}
