using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kitbox
{
    public partial class App : Form
    {
        public App()
        {
            InitializeComponent();
        }
        private void Viewer_Click(object sender, EventArgs e)
        {
        }

        private void button2_Click(object sender, EventArgs e)
        {
            panel2.Visible = true;
            panel1.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (!string.IsNullOrWhiteSpace(idUser.Text) && !string.IsNullOrWhiteSpace(pswUser.Text) && !idUser.Text.All(char.IsLetter))
            {
                bool clientExist = DbConnect.DblsCLient(Convert.ToInt32(idUser.Text), pswUser.Text);

                if (clientExist)
                {

                    DbConnect.DbConnectClient(Convert.ToInt32(idUser.Text), pswUser.Text);

                    panel3.Visible = true;
                    panel1.Visible = false;
                    errorID.Visible = false;
                    errorPsw.Visible = false;

                    clientFName.Text = DbConnect.DbConnectClient(Convert.ToInt32(idUser.Text), pswUser.Text).Firstname;
                    clientLName.Text = DbConnect.DbConnectClient(Convert.ToInt32(idUser.Text), pswUser.Text).Lastname;
                    newClientId.Text = DbConnect.DbConnectClient(Convert.ToInt32(idUser.Text), pswUser.Text).Id.ToString();

                }
                else if (clientExist == false)
                {
                    errorID.Text = "L'id ou le mot de passe est incorrect ";
                    errorID.Visible = true;
                }
            }
            else if (string.IsNullOrWhiteSpace(idUser.Text) || idUser.Text.All(char.IsLetter))
            {
                errorID.Visible = true;

            }
            else if (string.IsNullOrWhiteSpace(pswUser.Text))
            {
                errorPsw.Visible = true;

            }
            else
            {
                ;
            }

        }

        private void button22_Click(object sender, EventArgs e)
        {

            idUser.Text = "";
            pswUser.Text = "";
            newUsFirstName.Text = "";
            newUsLastName.Text = "";
            newUsMail.Text = "";
            newUsPhone.Text = "";
            newUsPostal.Text = "";
            newUsPsw.Text = "";
            newUsStNum.Text = "";
            newUsStreet.Text = "";

            panel2.Visible = false;
            panel3.Visible = false;
            panel1.Visible = true;



        }

        // Show bill 
        void butShowBill(object sender, EventArgs e)
        {
            panel6.Visible = true;
        }

        // Show List of components
        void butShowLComp(object sender, EventArgs e)
        {
            panel5.Visible = true;
        }



        private void button21_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrWhiteSpace(textBox9.Text))
            {

                panel4.Visible = false;
            }
            else
            {
                if (DbConnect.searchClient(Convert.ToInt32(textBox9.Text)) != null)
                {
                    // Test the content of GetBill  To Delete???? [Plus besoin de ça?]

                    string testbox = textBox9.Text;
                    Person client = DbConnect.searchClient(Convert.ToInt32(textBox9.Text));

                    string clientFname = client.Firstname;
                    string clientLName = client.Lastname;

                    // For test - To delete??.
                    Order test = new Order();
                    test.CurrentClient = client;
                    client.Id = Convert.ToInt32(textBox9.Text);
                    test.CurrentClient = client;



                    Dictionary<string, object> dictBill;
                    dictBill = test.GetBill(1);             // Replace 1 by Order_Id

                    // Components of an order = Wardrobes
                    Dictionary<string, Dictionary<string, List<object>>> components = (Dictionary<string, Dictionary<string, List<object>>>)dictBill["Components"];

                    // Number of Wardrobes
                    int nbrWardRobes = components.Count();


                    /********************************** Prices of Wardrobes*********************************/
                    List<double> prices = new List<double>();
                    Dictionary<string, Dictionary<string, List<string>>> wardrobe_components_list = new Dictionary<string, Dictionary<string, List<string>>>();

                    int wardrobe_id = 0;
                    int nbr_components = 0;
                    foreach (KeyValuePair<string, Dictionary<string, List<object>>> kvp1 in components)
                    {
                        //wardrobe_components_list.Add(kvp1.Key, new List<string>());
                        prices.Add(0);
                        foreach (KeyValuePair<string, List<object>> kvp2 in kvp1.Value)
                        {

                            //foreach (List<object> spec_list in kvp2.Value)
                            //{
                            prices[wardrobe_id] += Convert.ToDouble(kvp2.Value[0]) * Convert.ToDouble(kvp2.Value[1]);
                            //}
                            //List<string> code_ref = new List<string>();
                            // // Add the code
                            // code_ref.Add(kvp2.Value[2].ToString());
                            // // Add the referene
                            // code_ref.Add(kvp2.Key);
                            // // Add the quantity
                            // code_ref.Add(kvp2.Value[0].ToString());

                            // //if (!wardrobe_components_list.ContainsKey(kvp1.Key))
                            // //{
                            //     wardrobe_components_list.Add(kvp1.Key, code_ref);
                            //// }

                        }
                        wardrobe_id++;
                    }


                    /*Code - Reference - Quantiy of Components of Wardrobes*/
                    foreach (KeyValuePair<string, Dictionary<string, List<object>>> kvp5 in components)
                    {
                        foreach (KeyValuePair<string, List<object>> kvp4 in kvp5.Value)
                        {
                            Console.WriteLine(kvp5.Key + kvp4.Key);     // Component's Code 
                            Console.WriteLine(kvp5.Key + kvp4.Value[2].ToString()); // Component's Ref
                            Console.WriteLine(kvp5.Key + kvp4.Value[0].ToString()); // COmponent's Quantity
                            nbr_components += 1;

                        }
                    }






                    // Console.WriteLine(test.GetOrder(1)[0][0]); // Order num
                    //Console.WriteLine(test.GetOrder(1)[0][1]); // Order date


                    // List of Order's Num and Order's Date
                    /*
                    List<object> OrderNum = new List<object>();
                    for (int i = 0; i < test.GetOrder(Convert.ToInt32(textBox9.Text)).Count();i++)
                    {
                        foreach (object yit in test.GetOrder(Convert.ToInt32(textBox9.Text))[i])
                        {
                            OrderNum.Add(yit);
                        }
                    }
                    */

                    List<List<object>> orders = test.GetOrder(Convert.ToInt32(textBox9.Text));
                    int OrderNum = orders.Count();













                    Console.WriteLine("----------------------------------------");




                    // Code of the components from one Wardrobe
                    foreach (object codeComp in components["Wardrobe1"].Keys)
                    {
                        Console.WriteLine(codeComp);
                    }

                    // Reference of the components from one Wardrobe
                    foreach (List<object> refComp in components["Wardrobe1"].Values)
                    {
                        Console.WriteLine(refComp[2]);
                    }

                    /*          Not FInished - Looking for how to  Count the number of componenets of an Order      !!!!!!!!!!!!
                    for (int i=1; i < components.Keys.Count();i++)
                    {
                        components["Wardrobe" + "i"].Values;
                    }
                    */

                    // List of Wardrobes' name
                    List<string> listWardName = new List<string>();
                    foreach (string codeComp in components.Keys)
                    {
                        listWardName.Add(codeComp);
                    }



                    // GetBill(1)["Header"] = affiche valeur associée à Header
                    // GetBill(1)["Footer"] = affiche valeur associée à Footer


                    label36.Text = clientFname + " " + clientLName;
                    clientIDValue.Text = textBox9.Text;

                    List<object> orders_numb = new List<object>();
                    // Row generation and Auto completion of the result Table from the client's search 
                    for (int i = 0; i < OrderNum; i++)
                    {
                        tableLayoutPanel1.RowCount = i + 1;
                        tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));

                        //Num order
                        Label NumCom = new Label();
                        NumCom.Text = Convert.ToString(orders[i][0]);
                        tableLayoutPanel1.Controls.Add(NumCom, 0, i + 1);
                        orders_numb.Add(orders[i][0].ToString());

                        // Date order
                        Label Date = new Label();
                        Date.Text = Convert.ToString(orders[i][1]);
                        tableLayoutPanel1.Controls.Add(Date, 1, i + 1);


                        /*
                        // Button Bill
                        Button ButBill = new Button();
                        int numOrder = (i + 1);
                        ButBill.Text = "Afficher";          
                        //ButBill.Text = string.Format("Afficher commande {0}", numOrder.ToString()) ;
                        tableLayoutPanel1.Controls.Add(ButBill, 2,i+1);

                        if (ButBill != null)
                        {
                            ButBill.Click += new System.EventHandler(this.butShowBill);
                        }
                        //Button List of components
                        Button ButLCom = new Button();
                        ButLCom.Text = "Afficher";

                        ButLCom.Size=new Size(130,20);
                        ButLCom.AutoSize = true;
                        tableLayoutPanel1.Controls.Add(ButLCom, 3, i+1);
                        if (ButLCom != null)
                        {
                            ButLCom.Click += new System.EventHandler(this.butShowLComp);
                            
                        }

                        */



                    }

                    boxBill.Items.AddRange(orders_numb.ToArray());
                    boxLComp.Items.AddRange(orders_numb.ToArray());


                    /*Code - Reference - Quantiy of Components of Wardrobes*/
                    List<List<object>> ward_specs = new List<List<object>>();
                    int current_ward = 0;
                    foreach (KeyValuePair<string, Dictionary<string, List<object>>> kvp5 in components)
                    {
                        current_ward++;

                        foreach (KeyValuePair<string, List<object>> kvp4 in kvp5.Value)
                        {
                            //ward_specs[current_ward].;
                            Console.WriteLine(kvp5.Key + kvp4.Key);     // Component's Code 
                            Console.WriteLine(kvp5.Key + kvp4.Value[2].ToString()); // Component's Ref
                            Console.WriteLine(kvp5.Key + kvp4.Value[0].ToString()); // COmponent's Quantity
                            nbr_components += 1;

                        }
                    }

                    // Number of Rows of List of Components' Table
                    tableLayoutPanel3.RowCount = components["Wardrobe1"].Count();
                    int count = 0;
                    foreach (KeyValuePair<string, Dictionary<string, List<object>>> kvp5 in components)
                    {
                        //current_ward++;

                        // Wardrobe name
                        //Code empty
                        Label col1 = new Label();
                        col1.Text = "";
                        tableLayoutPanel3.Controls.Add(col1, 0, count + 1);

                        //Reference
                        Label col2 = new Label();
                        col2.Text = kvp5.Key;
                        tableLayoutPanel3.Controls.Add(col2, 1, count + 1);

                        //Quantity empty
                        Label col3 = new Label();
                        col3.Text = "";
                        tableLayoutPanel3.Controls.Add(col3, 2, count + 1);




                        foreach (KeyValuePair<string, List<object>> kvp4 in kvp5.Value)
                        {
                            count++;
                            //for (int i = 0; i < nbr_components; i++)
                            //{

                            //current_ward++;

                            /*
                            //ward_specs[current_ward].;
                            Console.WriteLine(kvp5.Key + kvp4.Key);     // Component's Code 
                            Console.WriteLine(kvp5.Key + kvp4.Value[2].ToString()); // Component's Ref
                            Console.WriteLine(kvp5.Key + kvp4.Value[0].ToString()); // COmponent's Quantity
                            nbr_components += 1;
                            */

                            // Dictionary<string, List<object>> current_wardrobe = components[string.Format("Wardrobe{0}", (count + 1).ToString())];
                            //Code
                            Label Code = new Label();
                            Code.Text = kvp4.Key;
                            tableLayoutPanel3.Controls.Add(Code, 0, count + 1);

                            //Reference
                            Label Reference = new Label();
                            Reference.Text = kvp4.Value[2].ToString();
                            tableLayoutPanel3.Controls.Add(Reference, 1, count + 1);

                            //Quantity
                            Label Quantity = new Label();
                            Quantity.Text = kvp4.Value[0].ToString();
                            tableLayoutPanel3.Controls.Add(Quantity, 2, count + 1);
                        }

                        count++;
                    }


                    double total_price = 0;

                    // Number of Rows for  Bill's Table
                    tableLayoutPanel2.RowCount = nbrWardRobes + 1;
                    /*
                    for (int i = 0; i < nbrWardRobes; i++)
                    {
                        //Numb of Products
                        Label Product = new Label();
                        Product.Text = listWardName[i];
                        tableLayoutPanel2.Controls.Add(Product, 0, i+1);


                        //Numb of Priices per Prod
                        Label ProdPrice = new Label();
                        ProdPrice.Text = prices[i].ToString();
                        tableLayoutPanel2.Controls.Add(ProdPrice, 1, i+1);

                        total_price +=prices[i];
                    }
                    */

                    textHeader.Text = test.GetBill(1)["Header"].ToString();
                    textFooter.Text = test.GetBill(1)["Footer"].ToString();
                    totPrice.Text = total_price.ToString();

                    panel4.Visible = true;
                }
                else
                {
                    ; // Cas si pas de nom
                }

            }
        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {

        }

        private void button27_Click(object sender, EventArgs e)
        {
            panel6.Visible = true;
        }

        private void button26_Click(object sender, EventArgs e)
        {
            panel5.Visible = true;
        }

        private void button28_Click(object sender, EventArgs e)
        {
            panel5.Visible = false;
        }

        private void button29_Click(object sender, EventArgs e)
        {


            panel6.Visible = false;
        }

        private void clientPrenom_Click(object sender, EventArgs e)
        {

        }

        private void clientNom_Click(object sender, EventArgs e)
        {

        }

        private void clientNum_Click(object sender, EventArgs e)
        {

        }

        private void label36_Click(object sender, EventArgs e)
        {

        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void idUser_TextChanged(object sender, EventArgs e)
        {

            errorID.Visible = false;
            errorPsw.Visible = false;

        }
        private void Control1_MouseClick(Object sender, MouseEventArgs e)
        {
            errorID.Visible = false;
            errorPsw.Visible = false;
        }

        private void errorID_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            /*
            (Faut ajouter Client à la classe Order?)
            Person newUser = new Person();
            newUser.Last,ame = newUsLastName.Text;
            newUser.tFirstname = newUsFirstName.Text;
            newUser.Phone_Number = newUsPhone.Text;
            newUser.Id = newUsID.Text;
            newUser.Password = newUsPsw.Text;
            DbAddClient(newUser);
            */
            // clientPrenom.Text = order.GetCurrentClient.GetLastName; Il faut créer auparavant l'objet Order ayant un Objet Client

            if (!string.IsNullOrWhiteSpace(newUsLastName.Text) || !string.IsNullOrWhiteSpace(newUsFirstName.Text) || !string.IsNullOrWhiteSpace(newUsPhone.Text) || !string.IsNullOrWhiteSpace(newUsMail.Text) || !string.IsNullOrWhiteSpace(newUsPsw.Text))
            {
                panel2.Visible = false;
                panel3.Visible = true;
                try
                {
                    Person person = new Person();

                    // DTB s'en occupe automatiquement normallement person.Id = ;
                    person.Lastname = newUsLastName.Text;
                    person.Firstname = newUsFirstName.Text;
                    person.Phone_number = Convert.ToInt32(newUsPhone.Text);
                    person.Email = newUsMail.Text;
                    person.Password = newUsPsw.Text;
                    person.Address["Street"] = newUsStreet.Text;
                    person.Address["Street number"] = newUsStNum.Text;
                    person.Address["Postal code"] = newUsPostal.Text;


                    DbConnect.DbAddClient(person);
                    clientFName.Text = newUsFirstName.Text;
                    clientLName.Text = newUsLastName.Text;
                    newClientId.Text = DbConnect.searchId(person.Phone_number).ToString();
                }
                catch (Exception errorSignIn)
                {
                    panel2.Visible = true;
                    panel1.Visible = false;
                    panel3.Visible = false;
                }
            }
        }

        private void Rechercher_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button18_Click(object sender, EventArgs e)
        {

            Person person = DbConnect.searchClient(Convert.ToInt32(textBox8.Text));
            Order order = new Order();
            order.CurrentClient = person;
            if (person == null)
            { errorIdSearch.Visible = true; }

        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            errorIdSearch.Visible = false;
        }

        private void label35_Click(object sender, EventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label45_Click(object sender, EventArgs e)
        {

        }

        private void showBill_Click(object sender, EventArgs e)
        {
            if (boxBill.Text != "")
            {

                int numbOrder = Convert.ToInt32(boxBill.Text);

                Person client = new Person();
                Order test = new Order();
                test.CurrentClient = client;
                client.Id = Convert.ToInt32(textBox9.Text);
                test.CurrentClient = client;


                Dictionary<string, object> bill = test.GetBill(numbOrder);



                // Components of an order = Wardrobes
                Dictionary<string, Dictionary<string, List<object>>> components = (Dictionary<string, Dictionary<string, List<object>>>)bill["Components"];
                // Number of Wardrobes
                int nbrWardRobes = components.Count();


                List<string> listWardName = new List<string>();
                foreach (string codeComp in components.Keys)
                {
                    listWardName.Add(codeComp);
                }

                double total_price = 0;
                List<double> prices = new List<double>();
                Dictionary<string, Dictionary<string, List<string>>> wardrobe_components_list = new Dictionary<string, Dictionary<string, List<string>>>();

                int wardrobe_id = 0;

                int nbr_components = 0;
                foreach (KeyValuePair<string, Dictionary<string, List<object>>> kvp1 in components)
                {
                    //wardrobe_components_list.Add(kvp1.Key, new List<string>());
                    prices.Add(0);
                    foreach (KeyValuePair<string, List<object>> kvp2 in kvp1.Value)
                    {


                        prices[wardrobe_id] += Convert.ToDouble(kvp2.Value[0]) * Convert.ToDouble(kvp2.Value[1]);


                    }
                    wardrobe_id++;
                }

                tableLayoutPanel2.Controls.Clear();
                Label product = new Label();
                product.Text = " Produit(s)";
                tableLayoutPanel2.Controls.Add(product, 0, 0);

                Label price = new Label();
                price.Text = " Prix";
                tableLayoutPanel2.Controls.Add(price, 1, 0);


                for (int i = 0; i < listWardName.Count(); i++)
                {

                    //Numb of Products
                    Label Product = new Label();
                    Product.Text = listWardName[i];
                    tableLayoutPanel2.Controls.Add(Product, 0, i + 1);


                    //Numb of Prices per Prod
                    Label ProdPrice = new Label();
                    ProdPrice.Text = prices[i].ToString();
                    tableLayoutPanel2.Controls.Add(ProdPrice, 1, i + 1);

                    total_price += prices[i];
                }

                textHeader.Text = test.GetBill(1)["Header"].ToString();
                textFooter.Text = test.GetBill(1)["Footer"].ToString();
                totPrice.Text = total_price.ToString();

                panel6.Visible = true;

            }
        }

        private void boxBill_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void showLComp_Click(object sender, EventArgs e)
        {

            if (boxLComp.Text != "")
            {
                int numbOrder = Convert.ToInt32(boxLComp.Text);

                Person client = new Person();
                Order test = new Order();
                test.CurrentClient = client;
                client.Id = Convert.ToInt32(textBox9.Text);
                test.CurrentClient = client;

                Dictionary<string, object> bill = test.GetBill(numbOrder);

                // Components of an order = Wardrobes
                Dictionary<string, Dictionary<string, List<object>>> components = (Dictionary<string, Dictionary<string, List<object>>>)bill["Components"];
                // Number of Wardrobes
                int nbrWardRobes = components.Count();


                List<string> listWardName = new List<string>();
                foreach (string codeComp in components.Keys)
                {
                    listWardName.Add(codeComp);
                }

                double total_price = 0;
                List<double> prices = new List<double>();
                Dictionary<string, Dictionary<string, List<string>>> wardrobe_components_list = new Dictionary<string, Dictionary<string, List<string>>>();

                int wardrobe_id = 0;

                int nbr_components = 0;
                foreach (KeyValuePair<string, Dictionary<string, List<object>>> kvp1 in components)
                {
                    //wardrobe_components_list.Add(kvp1.Key, new List<string>());
                    prices.Add(0);
                    foreach (KeyValuePair<string, List<object>> kvp2 in kvp1.Value)
                    {


                        prices[wardrobe_id] += Convert.ToDouble(kvp2.Value[0]) * Convert.ToDouble(kvp2.Value[1]);


                    }
                    wardrobe_id++;
                }

                tableLayoutPanel3.Controls.Clear();
                //Code empties
                Label code_col = new Label();
                code_col.Text = "Code";
                tableLayoutPanel3.Controls.Add(code_col, 0, 0);

                //Reference empties
                Label ref_col = new Label();
                ref_col.Text = "Reference";
                tableLayoutPanel3.Controls.Add(ref_col, 1, 0);

                //Quantity empties
                Label quant_ref = new Label();
                quant_ref.Text = "Quantity";
                tableLayoutPanel3.Controls.Add(quant_ref, 2, 0);



                /*Code - Reference - Quantiy of Components of Wardrobes*/
                List<List<object>> ward_specs = new List<List<object>>();
                int current_ward = 0;
                foreach (KeyValuePair<string, Dictionary<string, List<object>>> kvp5 in components)
                {
                    current_ward++;

                    foreach (KeyValuePair<string, List<object>> kvp4 in kvp5.Value)
                    {
                        //ward_specs[current_ward].;
                        Console.WriteLine(kvp5.Key + kvp4.Key);     // Component's Code 
                        Console.WriteLine(kvp5.Key + kvp4.Value[2].ToString()); // Component's Ref
                        Console.WriteLine(kvp5.Key + kvp4.Value[0].ToString()); // COmponent's Quantity
                        nbr_components += 1;

                    }
                }


                // Number of Rows of List of Components' Table
                tableLayoutPanel3.RowCount = components["Wardrobe1"].Count();

                int count = 0;
                foreach (KeyValuePair<string, Dictionary<string, List<object>>> kvp5 in components)
                {
                    //current_ward++;

                    // Wardrobe name
                    //Code empty
                    Label col1 = new Label();
                    col1.Text = "";
                    tableLayoutPanel3.Controls.Add(col1, 0, count + 1);

                    //Reference
                    Label col2 = new Label();
                    col2.Text = kvp5.Key;
                    tableLayoutPanel3.Controls.Add(col2, 1, count + 1);

                    //Quantity empty
                    Label col3 = new Label();
                    col3.Text = "";
                    tableLayoutPanel3.Controls.Add(col3, 2, count + 1);



                    foreach (KeyValuePair<string, List<object>> kvp4 in kvp5.Value)
                    {
                        count++;
                        //for (int i = 0; i < nbr_components; i++)
                        //{

                        //current_ward++;

                        /*
                        //ward_specs[current_ward].;
                        Console.WriteLine(kvp5.Key + kvp4.Key);     // Component's Code 
                        Console.WriteLine(kvp5.Key + kvp4.Value[2].ToString()); // Component's Ref
                        Console.WriteLine(kvp5.Key + kvp4.Value[0].ToString()); // COmponent's Quantity
                        nbr_components += 1;
                        */

                        // Dictionary<string, List<object>> current_wardrobe = components[string.Format("Wardrobe{0}", (count + 1).ToString())];
                        //Code
                        Label Code = new Label();
                        Code.Text = kvp4.Key;
                        tableLayoutPanel3.Controls.Add(Code, 0, count + 1);

                        //Reference
                        Label Reference = new Label();
                        Reference.Text = kvp4.Value[2].ToString();
                        tableLayoutPanel3.Controls.Add(Reference, 1, count + 1);

                        //Quantity
                        Label Quantity = new Label();
                        Quantity.Text = kvp4.Value[0].ToString();
                        tableLayoutPanel3.Controls.Add(Quantity, 2, count + 1);
                    }

                    count++;
                }

                /*
                for (int i = 0; i < listWardName.Count(); i++)
                {
                    // Content of List of Components



                }
                */

                panel5.Visible = true;

            }
        }
    }
}
