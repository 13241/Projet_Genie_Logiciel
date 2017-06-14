using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;


namespace Kitbox
{
    public class BDD
    {
        string myConnectionString = "";
        string database ="";
        public BDD(string database)
        {
            this.myConnectionString = "server = localhost; user id = root; database = " + database + "; persistsecurityinfo = True;";
            this.database = database;
        }
        //par exemple 
        //tableName = "mytable" 
        //columnsName = "Id, Ref, Code, Dimensionscm, hauteur, profondeur, largeur, Couleur, Enstock, Stock_minimum, PrixClient, NbPiecescasier, PrixFourn_1, DelaiFourn_1, PrixFourn2, DelaiFourn2" 
        //data = s
        public void addElement(string tableName, string columnNames, string data)
        {
            using (MySqlConnection connection = new MySqlConnection(this.myConnectionString))
            {
                try
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand("INSERT INTO " + tableName + " (" + columnNames + ") VALUES(" + data + ")", connection);
                    command.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
        public void removeElement(string tableName, string id)
        {
            using (MySqlConnection connection = new MySqlConnection(this.myConnectionString))
            {
                connection.Open();
                string str = "DELETE FROM " + tableName + " WHERE  Id = " + id;
                MySqlCommand command = new MySqlCommand(str, connection);
                command.ExecuteNonQuery();
            }
        }
        public void modifyElement(string tableName, string columnNames, string id)
        {
            using (MySqlConnection connection = new MySqlConnection(this.myConnectionString))
            {
                connection.Open();
                string str = "UPDATE "+ tableName + " SET " + columnNames + " WHERE Id = " + id;
                MySqlCommand command = new MySqlCommand(str, connection);
                command.ExecuteNonQuery();
            }
        }
        //For selecting all the columns, the argument Selection is the caracter "*".
        //A condition example :  WHERE (Id=1 OR Ref="Corniere"). 
        //A condition is optional, the user may ask to copy all of the content from the database in a table.
        public List<List<object>> readElement(string selection, string tableName, string condition = null)
        {
            //The following table will contain the selected data from the database.
            List<List<object>> Kitbox = new List<List<object>>();
            using (MySqlConnection connection = new MySqlConnection(this.myConnectionString))
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand("SELECT " + selection + " FROM " + tableName + " " + condition + "", connection);
                int numberColumn = 0;
                //Calculate the total number of rows
                if (selection == "*")
                {
                    using (MySqlCommand totalColumns = new MySqlCommand("SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE table_name = '" + tableName + "'", connection))
                    {
                        numberColumn = (int)(long)totalColumns.ExecuteScalar();
                    }
                }
                else
                {
                    numberColumn = selection.Split(',').Length;
                }
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        List<object> row = new List<object>();
                        //This loop enables us to store the desired row in the collection Kitbox
                        for (int i = 0; i < numberColumn; i++)
                        {
                            row.Add(reader[i]);
                        }
                        Kitbox.Add(row);
                    }
                    return Kitbox;
                }
            }
        }
        public int totalRows(string tableName)
        {
            using (MySqlConnection connection = new MySqlConnection(this.myConnectionString))
            {
				connection.Open();
                MySqlCommand rows = new MySqlCommand("SELECT COUNT(*) FROM " + tableName + "", connection);
                return Convert.ToInt32(rows.ExecuteScalar());
            }
        }
        public int totalColumns(string tableName)
        {
            using (MySqlConnection connection = new MySqlConnection(this.myConnectionString))
            {
				connection.Open();
                MySqlCommand columns = new MySqlCommand("SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE table_name = '" + tableName + "'", connection);
                return Convert.ToInt32(columns.ExecuteScalar());
            }
        }
    }
}
