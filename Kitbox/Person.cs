using System;
using System.Collections.Generic;
namespace Kitbox
{
	public class Person
	{
		protected string firstname;
		protected string lastname;
		protected int phone_number;
		protected string email;
		protected int id;
		protected string password;

		//The adress dictionary contains the keys : street, street number and postal code
		protected Dictionary<string, object> address = new Dictionary<string, object>();
        
        public Person()
        {
            this.address.Add("Street", null);
            this.address.Add("Street number", null);
            this.address.Add("Postal code", null);
        }

        public string FirstName { get => firstname; set => firstname = value; }

        public string LastName { get => lastname; set => lastname = value; }

        public int PhoneNumber { get => phone_number; set => phone_number = value; }

        public string Email { get => email; set => email = value; }

        public int Id { get => id; set => id = value; }

        public string Password { get => password; set => password = value; }

        public Dictionary<string, object> Address { get => address; set => address = value; }
	}
}
