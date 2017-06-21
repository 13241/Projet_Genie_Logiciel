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
    /// <summary>
    /// see App for comments
    /// </summary>
    public partial class AppBrided : Form
    {
        public AppBrided()
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
            if (part.Views.ContainsKey(view))
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
            if(order.CurrentClient != null)
            {
                wardrobe.Book(true);
                DbOrder.DbAddOrder(order);
                button22_Click(sender, e);
                tabControl1.SelectTab(0);
            }
            else
            {
                tabControl1.SelectTab(0);
            }
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

                    order.CurrentClient = DbConnect.DbConnectClient(Convert.ToInt32(idUser.Text), pswUser.Text);

                    panel3.Visible = true;
                    panel1.Visible = false;
                    errorID.Visible = false;
                    errorPsw.Visible = false;

                    clientFName.Text = order.CurrentClient.FirstName;
                    clientLName.Text = order.CurrentClient.LastName;
                    newClientId.Text = order.CurrentClient.Id.ToString();

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


            order = new Order();
            wardrobe = new Wardrobe(new Size3D(120, 36, 42));
            order.Wardrobes.Add(wardrobe);
            player_modif = false;
            M_selectwardrobe.Text = Convert.ToString(0);
            Preset();
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

                    order.CurrentClient = DbConnect.DbConnectClient(Convert.ToInt32(newClientId.Text), person.Password);
                }
                catch (Exception errorSignIn)
                {
                    panel2.Visible = true;
                    panel1.Visible = false;
                    panel3.Visible = false;
                }
            }
        }

        
    }
}
