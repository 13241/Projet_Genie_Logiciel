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
            this.myConnectionString = "server = localhost; user id = root; database = " + database + "; persistsecurityinfo = True; Password = sapuvu;";
            this.database = database;
        }
        //par exemple insertInto = 1000, 'Cornieres', 'COR36BL', '1x32(h)', 36, 0, 0, 'Blanc', 84, 32, 0.35, 4, 0.3, 3, 0.23,9 
        public void addElement(string insertInto)
        {
            using (MySqlConnection connection = new MySqlConnection(this.myConnectionString))
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand("INSERT INTO mytable(Id, Ref, Code, Dimensionscm, hauteur, profondeur, largeur, Couleur, Enstock, Stock_minimum, PrixClient, NbPiecescasier, PrixFourn_1, DelaiFourn_1, PrixFourn2, DelaiFourn2) VALUES(" + insertInto + ")", connection);
                command.ExecuteNonQuery();
            }
        }
        //For selecting all the columns, the argument Selection is the caracter "*"
        //A condition example :  WHERE (Id=1 OR Ref="Corniere"). 
        //A condition is optional, the user may ask to copy all of the content from the database in a table.
        public List<List<object>> readElement(string selection, string condition = null)
        {
            //The following table will contain the selected data from the database.
            List<List<object>> Kitbox = new List<List<object>>();
            using (MySqlConnection connection = new MySqlConnection(this.myConnectionString))
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand("SELECT " + selection + " FROM mytable " + condition + "", connection);
                int numberColumn = selection.Split(',').Length;
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        List<object> row = new List<object>();
                        //This loop enables us to store the desired row in the table Kitbox
                        for (int i = 0; i <numberColumn; i++)
                        {
                            row.Add(reader[i]);
                        }
                        Kitbox.Add(row);
                    }
                    return Kitbox;
                }
            }
        }
    }
}
