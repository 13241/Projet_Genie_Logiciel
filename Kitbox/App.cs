using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.Media3D;

namespace Kitbox
{
    public partial class App : Form
    {
        public App()
        {
            InitializeComponent();
            order = new Order();
            wardrobe = new Wardrobe(new Size3D(120, 36, 42));
            order.Wardrobes.Add(wardrobe);
            player_modif = false;
            M_selectwardrobe.Text = Convert.ToString(0);
            Preset();
        }
        /******** MODELIZATION *********/

        private Order order;
        private Wardrobe wardrobe;
        private VisualPart part;
        Stack<VisualPart> parts;
        string view;
        bool player_modif;

        public void Preset()
        {
            part = wardrobe.Visual_part;
            parts = new Stack<VisualPart>();
            view = "front";
            M_etage.Text = "armoire";
            DisplayVisualPart(view);
            ZoneWardrobe(true);

            M_depth.Text = Convert.ToString(wardrobe.Dimensions.Z);

            M_width.Text = Convert.ToString(wardrobe.Dimensions.X);

            M_price.Text = Convert.ToString(wardrobe.SellingPrice());

            M_dim.Text = Convert.ToString(wardrobe.Dimensions.X) + "*" + Convert.ToString(wardrobe.Dimensions.Y) + "*" + Convert.ToString(wardrobe.Dimensions.Z);
        }

        public void DisplayVisualPart(string view)
        {
            if (part.Views1.ContainsKey(view))
            {
                M_screen.Controls.Clear();
                part.CleanFocus();
                part.ChangeScaling(M_screen.Size);
                M_screen.Controls.Add(part.Display()[view]);
            }
        }

        private void UnBook(object sender, FormClosingEventArgs e)
        {
            foreach (object ersatz in order.Wardrobes)
            {
                Wardrobe wardrobe = (Wardrobe)ersatz;
                wardrobe.UnBook();
            }
        }

        private void OnFocus(object sender, EventArgs e)
        {
            M_pointer.Text = part.Pointer;
            string piece = wardrobe.Visual_part.Pointer.Split('_').Last();
            player_modif = false;
            if (typeof(Part).IsInstanceOfType(wardrobe.Components[piece.Split('*').First()][piece.Split('*').Last()]))
            {
                M_height.Text = Convert.ToString(((Part)wardrobe.Components[piece.Split('*').First()][piece.Split('*').Last()]).Dimensions.Y);
                M_color.Text = DbCatalog.TraduireCouleur(((Part)wardrobe.Components[piece.Split('*').First()][piece.Split('*').Last()]).Color.Name);
            }
            else
            {
                M_height.Text = Convert.ToString(((Box)wardrobe.Components[piece.Split('*').First()][piece.Split('*').Last()]).Dimensions.Y);
                string part = ((Box)wardrobe.Components[piece.Split('*').First()][piece.Split('*').Last()]).Visual_part.Pointer.Split('_').Last();
                try//si la selection est une box mais qu'aucune piece n'est selectionnee, il ne faut pas changer le texte
                {
                    M_color.Text = DbCatalog.TraduireCouleur(((Box)wardrobe.Components[piece.Split('*').First()][piece.Split('*').Last()]).Pieces[part.Split('*').First()][part.Split('*').Last()].Color.Name);
                }
                catch { }
            }
        }

        private void ZoneWardrobe(bool isZoneWardrobe)
        {
            M_up.Enabled = !isZoneWardrobe;
            M_down.Enabled = !isZoneWardrobe;
            M_addbox.Enabled = isZoneWardrobe;
            M_removebox.Enabled = isZoneWardrobe;
        }

        private void M_up_Click(object sender, EventArgs e)
        {
            switch (view)
            {
                case "front":
                    view = "top";
                    break;
                case "bottom":
                    view = "front";
                    break;
            }
            DisplayVisualPart(view);
        }

        private void M_left_Click(object sender, EventArgs e)
        {
            switch (view)
            {
                case "front":
                    view = "left";
                    break;
                case "right":
                    view = "front";
                    break;
                case "left":
                    view = "rear";
                    break;
                case "rear":
                    view = "right";
                    break;
            }
            DisplayVisualPart(view);
        }

        private void M_right_Click(object sender, EventArgs e)
        {
            switch (view)
            {
                case "front":
                    view = "right";
                    break;
                case "left":
                    view = "front";
                    break;
                case "right":
                    view = "rear";
                    break;
                case "rear":
                    view = "left";
                    break;
            }
            DisplayVisualPart(view);
        }

        private void M_down_Click(object sender, EventArgs e)
        {
            switch (view)
            {
                case "front":
                    view = "bottom";
                    break;
                case "top":
                    view = "front";
                    break;
            }
            DisplayVisualPart(view);
        }

        private void M_zoom_Click(object sender, EventArgs e)
        {
            string piece = part.ConvertToPosition(part.Pointer).Last();
            if (part.Pieces.Keys.Contains(piece))
            {
                parts.Push(part);
                part = part.Pieces[piece].Item1;
                view = "front";
                DisplayVisualPart(view);
                ZoneWardrobe(false);
                if (parts.Count == 1)
                {
                    M_etage.Text = piece;
                }
            }
        }

        private void M_unzoom_Click(object sender, EventArgs e)
        {
            if (parts.Count() > 0)
            {
                VisualPart container = parts.Pop();
                string piece = container.ConvertToPosition(container.Pointer).Last();
                container.ReinsertPiece(piece);
                part = container;
                view = "front";
                DisplayVisualPart(view);
            }
            if (parts.Count() == 0)
            {
                ZoneWardrobe(true);
                M_etage.Text = "armoire";
            }
        }

        private void M_addbox_Click(object sender, EventArgs e)
        {
            if (DbCatalog.DbGetHeightOpt(0, wardrobe.Dimensions.Y).Count == 0 || wardrobe.Components["Etage"].Count == 7)
            {
                return;
            }
            wardrobe.AddBox(36);
            Preset();
        }

        private void M_removebox_Click(object sender, EventArgs e)
        {
            string position = wardrobe.Visual_part.Pointer.Split('*').Last();
            wardrobe.RemoveBox(position);
            Preset();
        }

        private void M_screen_Click(object sender, EventArgs e)
        {
            part.CleanFocus();
        }

        private void M_selectwardrobe_Enter(object sender, EventArgs e)
        {
            M_selectwardrobe.Items.Clear();
            for (int i = 0; i < order.Wardrobes.Count; i++)
            {
                M_selectwardrobe.Items.Add(i);
            }
        }

        private void M_selectwardrobe_SelectedIndexChanged(object sender, EventArgs e)
        {
            wardrobe = (Wardrobe)order.Wardrobes[M_selectwardrobe.SelectedIndex];
            Preset();
        }

        private void M_width_Enter(object sender, EventArgs e)
        {
            M_width.Items.Clear();
            List<string> options = DbCatalog.DbGetLateralDimOpt("Ar");
            foreach (string option in options)
            {
                M_width.Items.Add(option);
            }
        }

        private void M_depth_Enter(object sender, EventArgs e)
        {
            M_depth.Items.Clear();
            List<string> options = DbCatalog.DbGetLateralDimOpt("GD");
            foreach (string option in options)
            {
                M_depth.Items.Add(option);
            }
        }

        private void M_width_SelectedValueChanged(object sender, EventArgs e)
        {
            wardrobe.ChangeSurface(Convert.ToDouble(M_width.Items[M_width.SelectedIndex]), wardrobe.Dimensions.Z);
            M_width.Text = Convert.ToString(M_width.Items[M_width.SelectedIndex]);
            Preset();
        }

        private void M_depth_SelectedValueChanged(object sender, EventArgs e)
        {
            wardrobe.ChangeSurface(wardrobe.Dimensions.X, Convert.ToDouble(M_depth.Items[M_depth.SelectedIndex]));
            M_depth.Text = Convert.ToString(M_depth.Items[M_depth.SelectedIndex]);
            Preset();
        }

        private void M_height_Enter(object sender, EventArgs e)
        {
            try
            {
                player_modif = true;
                M_height.Items.Clear();
                List<string> options = DbCatalog.DbGetHeightOpt(Convert.ToDouble(M_height.Text), wardrobe.Dimensions.Y);
                foreach (string option in options)
                {
                    M_height.Items.Add(option);
                }
            }
            catch { }
        }

        private void M_height_SelectedValueChanged(object sender, EventArgs e)
        {
            if (player_modif)
            {
                string position = wardrobe.Visual_part.Pointer.Split('*').Last();
                wardrobe.ResizeBox(position, Convert.ToDouble(M_height.Items[M_height.SelectedIndex]));
                M_height.Text = Convert.ToString(M_height.Items[M_height.SelectedIndex]);
                Preset();
            }
        }

        private void M_color_Enter(object sender, EventArgs e)
        {
            try
            {
                player_modif = true;
                M_color.Items.Clear();
                string piece = wardrobe.Visual_part.Pointer.Split('_').Last();
                Part mypart;
                if (piece.Contains("Etage"))
                {
                    string part = ((Box)wardrobe.Components[piece.Split('*').First()][piece.Split('*').Last()]).Visual_part.Pointer.Split('_').Last();
                    mypart = ((Box)wardrobe.Components[piece.Split('*').First()][piece.Split('*').Last()]).Pieces[part.Split('*').First()][part.Split('*').Last()];
                }
                else
                {
                    mypart = ((Part)wardrobe.Components[piece.Split('*').First()][piece.Split('*').Last()]);
                }
                Dictionary<string, string> selected_characteristics = new Dictionary<string, string>()
                {
                    { "Ref", mypart.Reference },
                    { "largeur", Convert.ToString(mypart.Dimensions.X) },
                    { "hauteur", Convert.ToString(mypart.Dimensions.Y) },
                    { "profondeur", Convert.ToString(mypart.Dimensions.Z) }
                };
                List<string> options = DbCatalog.DbGetColors(selected_characteristics);
                foreach (string option in options)
                {
                    M_color.Items.Add(option);
                }
            }
            catch { }
        }

        private void M_color_SelectedValueChanged(object sender, EventArgs e)
        {
            if (player_modif)
            {
                string etage = wardrobe.Visual_part.Pointer.Split('_').Last();
                wardrobe.ChangeColor(Convert.ToString(M_color.Items[M_color.SelectedIndex]));
                M_color.Text = Convert.ToString(M_color.Items[M_color.SelectedIndex]);
                Preset();
            }
        }

        private void M_newwardrobe_Click(object sender, EventArgs e)
        {
            wardrobe = new Wardrobe(new Size3D(120, 36, 42));
            order.Wardrobes.Add(wardrobe);
            player_modif = false;
            M_selectwardrobe.Text = Convert.ToString(order.Wardrobes.Count - 1);
            Preset();
        }

        private void M_unavailable_Enter(object sender, EventArgs e)
        {
            M_unavailable.Items.Clear();
            foreach(string piece in wardrobe.Components.Keys)
            {
                foreach(string position in wardrobe.Components[piece].Keys)
                {
                    if(typeof(Box).IsInstanceOfType(wardrobe.Components[piece][position]))
                    {
                        Box thisbox = (Box)wardrobe.Components[piece][position];
                        foreach(string part in thisbox.Pieces.Keys)
                        {
                            foreach(string pos in thisbox.Pieces[part].Keys)
                            {
                                if(Convert.ToInt32(thisbox.Pieces[part][pos].Delayed) != 0)
                                {
                                    M_unavailable.Items.Add(part + "*" + pos + "_" + thisbox.Pieces[part][pos].Delayed);
                                }
                            }
                        }
                    }
                    else
                    {
                        if(Convert.ToInt32(((Part)wardrobe.Components[piece][position]).Delayed) != 0)
                        {
                            M_unavailable.Items.Add(piece + "*" + position + "_" + ((Part)wardrobe.Components[piece][position]).Delayed);
                        }
                    }
                }
            }
        }

        private void M_unavailable_SelectedIndexChanged(object sender, EventArgs e)
        {
            M_delai.Text = Convert.ToString(M_unavailable.Items[M_unavailable.SelectedIndex]).Split('_').Last();
        }

        private void M_previeworder_Click(object sender, EventArgs e)
        {
            
        }


        //BUTT

        /******** CONNECTION AND RESEARCH *********/
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

                    clientFName.Text = DbConnect.DbConnectClient(Convert.ToInt32(idUser.Text), pswUser.Text).FirstName;
                    clientLName.Text = DbConnect.DbConnectClient(Convert.ToInt32(idUser.Text), pswUser.Text).LastName;
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

                    string clientFname = client.FirstName;
                    string clientLName = client.LastName;

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
                    person.LastName = newUsLastName.Text;
                    person.FirstName = newUsFirstName.Text;
                    person.PhoneNumber = Convert.ToInt32(newUsPhone.Text);
                    person.Email = newUsMail.Text;
                    person.Password = newUsPsw.Text;
                    person.Address["Street"] = newUsStreet.Text;
                    person.Address["Street number"] = newUsStNum.Text;
                    person.Address["Postal code"] = newUsPostal.Text;


                    DbConnect.DbAddClient(person);
                    clientFName.Text = newUsFirstName.Text;
                    clientLName.Text = newUsLastName.Text;
                    newClientId.Text = DbConnect.searchId(person.PhoneNumber).ToString();
                }
                catch (Exception errorSignIn)
                {
                    panel2.Visible = true;
                    panel1.Visible = false;
                    panel3.Visible = false;
                }
            }
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
