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
    public partial class Modelize : Form
    {
        private Order order;
        private Wardrobe wardrobe;
        private VisualPart part;
        Stack<VisualPart> parts;
        string view;
        bool player_modif;

        public Modelize()
        {
            InitializeComponent();
            order = new Order();
            wardrobe = new Wardrobe(new Size3D(120, 36, 42));
            order.Wardrobes.Add(wardrobe);
            player_modif = false;
            Preset();
        }

        public void Preset()
        {
            part = wardrobe.Visual_part;
            parts = new Stack<VisualPart>();
            view = "front";
            M_etage.Text = "armoire";
            DisplayVisualPart(view);
            ZoneWardrobe(true);

            M_selectwardrobe.Text = Convert.ToString(0);

            M_depth.Text = Convert.ToString(wardrobe.Dimensions.Z);

            M_width.Text = Convert.ToString(wardrobe.Dimensions.X);
        }

        public void DisplayVisualPart(string view)
        {
            if(part.Views1.ContainsKey(view))
            {
                M_screen.Controls.Clear();
                part.CleanFocus();
                part.ChangeScaling(M_screen.Size);
                M_screen.Controls.Add(part.Display()[view]);
            }
        }

        private void UnBook(object sender, FormClosingEventArgs e)
        {
            foreach(object ersatz in order.Wardrobes)
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
                if(parts.Count == 1)
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
            if(parts.Count() == 0)
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
            part = wardrobe.Visual_part;
            parts = new Stack<VisualPart>();
            view = "front";
            DisplayVisualPart(view);
        }

        private void M_removebox_Click(object sender, EventArgs e)
        {
            string position = wardrobe.Visual_part.Pointer.Split('*').Last();
            wardrobe.RemoveBox(position);
            part = wardrobe.Visual_part;
            view = "front";
            DisplayVisualPart(view);
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
            foreach(string option in options)
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
            player_modif = true;
            M_height.Items.Clear();
            List<string> options = DbCatalog.DbGetHeightOpt(Convert.ToDouble(M_height.Text), wardrobe.Dimensions.Y);
            foreach(string option in options)
            {
                M_height.Items.Add(option);
            }
        }

        private void M_height_SelectedValueChanged(object sender, EventArgs e)
        {
            if(player_modif)
            {
                string position = wardrobe.Visual_part.Pointer.Split('*').Last();
                wardrobe.ResizeBox(position, Convert.ToDouble(M_height.Items[M_height.SelectedIndex]));
                M_height.Text = Convert.ToString(M_height.Items[M_height.SelectedIndex]);
                Preset();
            }
        }

        private void M_color_Enter(object sender, EventArgs e)
        {
            player_modif = true;
            M_color.Items.Clear();
            string piece = wardrobe.Visual_part.Pointer.Split('_').Last();
            Part mypart;
            if(piece.Contains("Etage"))
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

        private void M_color_SelectedValueChanged(object sender, EventArgs e)
        {
            if(player_modif)
            {
                string etage = wardrobe.Visual_part.Pointer.Split('_').Last();
                wardrobe.ChangeColor(Convert.ToString(M_color.Items[M_color.SelectedIndex]));
                M_color.Text = Convert.ToString(M_color.Items[M_color.SelectedIndex]);
                Preset();
            }
        }
    }
}
