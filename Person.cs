using System;
using System.Collections.Generic;
namespace Application
{
	public class Person
	{
		protected string firstname;
		protected string lastname;
		protected int phone_number; 
		protected string email; 
		protected int id; 
		protected string password;

		//adress est un dictionnaire dont les clés sont : rue, numéro de rue et code postal
		protected Dictionnary <int, object> adress = new Dictionary<int,object>();

		public string GetFirstName
			{
				get{ return this.firstname;}
			}

		public string GetLastName
			{
				get { return this.lastname; }
			}

		public int GetPhoneNumber
			{
				get { return this.phone_number; }
			}

		public string GetEmails
			{
				get { return this.email; }
			}

		public string GetId
			{
				get { return this.id; }
			}

		public string GetPassword
			{
				get { return this.password; }
			}

		 public Dictionary<string,object> GetAdress()
        {
			get	{ return this.Adress;} 
        }
}

)

