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
		protected Dictionnary <string, object> adress = new Dictionary<string,object>();

		public string GetFirstName
		{
			get
			{
				return this.firstname;
			}
			set
			{
				this.firstname = value;
			}	
		}

		public string GetLastName
		{
			get
			{
				return this.lastname;
			}
			set
			{
				this.lastname = value;
			}
		}

		public int GetPhoneNumber
		{
			get
			{
				return this.phone_number;
			}
			set
			{
				this.phone_number = value;
			}
		}

		public string GetEmails
		{
			get 
			{ 
				return this.email; 
			}
			set
			{
				this.email = value;
			}
		}

		public string GetId
		{
			get
			{ 
				return this.id;
			}
			set
			{
				this.id = value;
			}
		}

		public string GetPassword
		{
			get
			{
				return this.password; 
			}
			set
			{
				this.password = value;
			}
		}

		public Dictionary<string,object> GetAdress
        {
			get
			{ 
				return this.adress;
			}
			set
			{
				this.adresss = value;
			}
        }
	}
}

