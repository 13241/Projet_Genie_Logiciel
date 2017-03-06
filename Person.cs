using System;
System.Collections.Generic.Dictionary<TKey, TValue>
namespace Application
{
//adresse est un dictionnaire dont les clés sont : rue, numéro de rue et code postal
	public class Person
	{
		protected string firstname;
		protected string lastname;
		protected int phone_number; 
		protected string email; 
		protected int id; 
		protected string password;
		protected Dictionnary <int, object> address = new Dictionary<int,object>();

		public GetFirstName(string firstname)
			{
			get {return firstname;}
			}
		public GetLastName(string lastname)
			{
			get {return lastname; }
			}
		public GetPhoneNumber(int phone_number)
			{
			get {return phone_number; }
			}
		public GetEmails(string email)
			{
			get {return email; }
			}
		public GetId(string id)
			{
			get {return id; }
			}
		public GetPassword(string password)
			{
			get {return password; }
			}
		public Dictionnary <int, object> )GetAdress(int adress)
			{
			get {return adress<int,object>;}
			}
}
