using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;


namespace Kitbox
{
    public class VisualPart
    {
        //attributes
        private Dictionary<string, object> positions;
        private Dictionary<string, VPPanel> views; //views as in a technical drawing, key : name, value : VPPanel
        private Size mm_size; //most constrainig size for the visualPart
        private Size px_size; //size for the viewer (in pixels)
        private double scaling; //pixels per milimeter
        private HashSet<string> references; //references each panel of the VisualPart
        private Dictionary<string, Tuple<VisualPart, Dictionary<string, string>>> pieces;
        //piece name, visualPart, every path to access the different views, and the name of the view

        //EventAttributes
        private string pointer = "";
        private Rule selection;
        //methods

        /*
         * constructor
         * mm_size : most constraining size in milimeters
         * px_size : size available to display a view in pixels
         */
        //VisualPart
        public VisualPart(Size px_size, Dictionary<string, Size> mm_sizes)
        {
            references = new HashSet<string>();
            pieces = new Dictionary<string, Tuple<VisualPart, Dictionary<string, string>>>();
            positions = new Dictionary<string, object>();
            scaling = 1;
            Action<object> RuleSelected = SelectPiece;
            selection = new Rule(RuleSelected,
                new OrderedDictionary()
                {
                    { "sender", null }
                }, typeof(EventHandler), typeof(VisualPart));


            this.px_size = px_size;

            CreateViews(mm_sizes);
            ChangeConstrainingSize();
        }

        /*
         * 
         */
        //CreateViews
        private void CreateViews(Dictionary<string, Size> mm_sizes)
        {
            this.views = new Dictionary<string, VPPanel>();
            List<string> list_views = mm_sizes.Keys.ToList();
            for (int i = 0; i < list_views.Count(); i++)
            {
                VPPanel new_panel = new VPPanel();
                new_panel.BackColor = SystemColors.Control;
                new_panel.Location = new Point(0, 0);
                new_panel.Mm_location = new Point(0, 0);
                new_panel.Name = list_views[i];
                new_panel.Size = mm_sizes[list_views[i]];
                new_panel.Mm_size = mm_sizes[list_views[i]];
                views[list_views[i]] = new_panel;
                positions[list_views[i]] = null;
                references.Add(list_views[i]);
            }
        }

        /*
         * 
         */
        //ChangeConstrainingSize
        public void ChangeConstrainingSize()
        {
            int maxWidth = 0;
            int maxHeight = 0;
            foreach (string view in views.Keys)
            {
                if (views[view].Mm_size.Width > maxWidth)
                {
                    maxWidth = views[view].Mm_size.Width;
                }
                if (views[view].Mm_size.Height > maxHeight)
                {
                    maxHeight = views[view].Mm_size.Height;
                }
            }
            this.mm_size = new Size(maxWidth, maxHeight);
            ChangeScaling();
        }

        /*
         * 
         */
        //ChangeScaling : scaling
        public void ChangeScaling(double scaling)
        {
            this.scaling = scaling;
            foreach(string reference in references)
            {
                VPPanel current = GetPanel(reference);
                current.Location = ScalePoint(current.Mm_location, scaling);
                current.Size = ScaleSize(current.Mm_size, scaling);
            }
            foreach(string visual_part in pieces.Keys)
            {
                VisualPart piece = pieces[visual_part].Item1;
                piece.ChangeScaling(scaling);
            }
        }

        /*
         * 
         */
        //ChangeScaling : px_size
        public void ChangeScaling(Size? px_size = null)
        {
            if (px_size == null)
            {
                px_size = this.px_size;
            }
            else
            {
                this.px_size = (Size)px_size;
            }
            double scaling = Math.Min((double)this.px_size.Width / mm_size.Width, (double)this.px_size.Height / mm_size.Height);
            ChangeScaling(scaling);
        }

        /*
         * 
         */
        //ScalePoint
        private Point ScalePoint(Point point, double scaling)
        {
            return new Point(Convert.ToInt32(Math.Round(point.X * scaling)), Convert.ToInt32(Math.Round(point.Y * scaling)));
        }

        /*
         * 
         */
        //ScaleSize
        private Size ScaleSize(Size size, double scaling)
        {
            return new Size(Convert.ToInt32(Math.Round(size.Width * scaling)), Convert.ToInt32(Math.Round(size.Height * scaling)));
        }

        /*
         * 
         */
        //Views
        private Dictionary<string, VPPanel> Views
        {
            get { return views; }
        }

        /*
         * 
         */
        //Pointer
        public string Pointer
        {
            get { return pointer; }
        }

        /*
         * 
         */
        //Display
        public Dictionary<string, VPPanel> Display()
        {
            Action<object> RuleSelected = SelectPiece;
            Rule selection = new Rule(RuleSelected,
                new OrderedDictionary()
                {
                    { "sender", null }
                }, typeof(EventHandler), typeof(VisualPart));
            this.selection = selection;
            foreach (string ref_piece in pieces.Keys)
            {
                VisualPart current = pieces[ref_piece].Item1;
                current.ChangeEventHandler(selection);
            }
            return Views;
        }

        /*
         * 
         */
        //ChangeEventHandler
        public void ChangeEventHandler(Rule selection)
        {
            this.selection = selection;
            foreach (string ref_piece in pieces.Keys)
            {
                pieces[ref_piece].Item1.ChangeEventHandler(selection);
            }
        }

        /*
         * 
         */
        //References
        public HashSet<string> References
        {
            get { return references; }
        }

        /*
         * 
         */
        //Mm_size
        public Size Mm_size
        {
            get { return mm_size; }
        }

        /*
         * 
         */
        //Positions
        public Dictionary<string, object> Positions
        {
            get { return positions; }
        }

        /*
         * 
         */
        //Pieces
        public Dictionary<string, Tuple<VisualPart, Dictionary<string, string>>> Pieces
        {
            get { return pieces; }
        }

        /*
         * 
         */
        //ReinsertPiece
        public void ReinsertPiece(string name)
        {
            VisualPart piece = Pieces[name].Item1;
            foreach(string container in Pieces[name].Item2.Keys)
            {
                VPPanel master = GetPanel(container);
                VPPanel slave = piece.GetPanel(Pieces[name].Item2[container]);
                if(!master.Controls.Contains(slave))
                {
                    master.Controls.Add(slave);
                }
            }
        }

        /*
         * 
         */
        //Scaling
        public double Scaling
        {
            get { return scaling; }
        }

        /*
         * 
         */
        //ConvertToPosition
        public List<string> ConvertToPosition(string name)
        {
            return name.Split('_').ToList();
        }

        /*
         * 
         */
        //ConvertToName
        public string ConvertToName(List<string> position)
        {
            return string.Join("_", position);
        }

        /*
         *
         */
        //HasSubContainer
        private bool HasSubcontainer(List<string> position)
        {
            if (!references.Contains(ConvertToName(position.Take(position.Count() - 1).ToList())))
            { return false; }
            return true;
        }

        /*
         * 
         */
        //EnlargeView
        public void EnlargeView(string view_name, Size mm_size)
        {
            try
            {
                views[view_name].Size = ScaleSize(mm_size, scaling);
                views[view_name].Mm_size = mm_size;
                ChangeConstrainingSize();
            }
            catch { }
        }

        /*
         * 
         */
        //GetPanel
        public VPPanel GetPanel(string name)
        {
            List<string> position = ConvertToPosition(name);
            Control[] subcontainers;
            if (position.Count() == 1 && views.ContainsKey(position[0]))
            {
                subcontainers = new VPPanel[1]
                    { views[position[0]] };
            }
            else if (position.Count() > 1 && views.ContainsKey(position[0]))
            {
                subcontainers = views[position[0]].Controls.Find(name, true);
            }
            else
            {
                subcontainers = new VPPanel[0];
            }
            if (subcontainers.Count() == 1)
            {
                return (VPPanel) subcontainers[0];
            }
            else
            {
                throw new Exception("Could not find consistent control at specified position");
            }
            
        }

        /*
         * recursive function, does not modify attribute positions
         */
        //AlterPositions
        private Dictionary<string, object> AlterPositions(Dictionary<string, object> positions, List<string> position, bool deleting)
        {
            if (position.Count() == 1)
            {
                if(positions == null)
                {
                    positions = new Dictionary<string, object>();
                }

                if (positions.ContainsKey(position[0]))
                {
                    if(deleting)
                    {
                        positions.Remove(position[0]);
                    }
                    else
                    {
                        throw new Exception("The specified position is already taken");
                    }
                }
                else
                {
                    if(deleting)
                    {
                        throw new Exception("The specified position does not exists");
                    }
                    else
                    {
                        positions.Add(position[0], null);
                    }
                }
            }
            else
            {
                positions[position[0]] = AlterPositions((Dictionary<string, object>)positions[position[0]], position.Skip(1).ToList(), deleting);
            }
            return positions;
        }

        /*
         * 
         */
        //RemovePosition
        private Dictionary<string, object> RemovePosition(Dictionary<string, object> positions, List<string> position)
        {
            return AlterPositions(positions, position, true);
        }

        /*
         * 
         */
        //AddPosition
        private Dictionary<string, object> AddPosition(Dictionary<string, object> positions, List<string> position)
        {
            return AlterPositions(positions, position, false);
        }

        /*
         * location and size units are milimeters
         */
        //AddPanel
        public VPPanel AddPanel(string name, Point location, Size size, Color? color = null, bool is_elliptic = false)
        {
            List<string> position = ConvertToPosition(name);
            VPPanel subcontainer;
            if (HasSubcontainer(position))
            {
                if (position.Count() > 1)
                {
                    subcontainer = GetPanel(ConvertToName(position.Take(position.Count() - 1).ToList()));
                }
                else if (position.Count() == 1)
                {
                    subcontainer = null;
                }
                else
                { throw new Exception("Specify a nonempty position"); }

                positions = AddPosition(positions, position);

                VPPanel new_panel = new VPPanel();

                bool mouseHover = true;
                if (color == null)
                {
                    color = SystemColors.Control;
                    mouseHover = false;
                }
                
                if(subcontainer != null)
                {
                    subcontainer.Controls.Add(new_panel);
                    new_panel.Click += new EventHandler(Click);
                    if (mouseHover)
                    {
                        new_panel.MouseHover += new EventHandler(MouseHover);
                    }
                }
                else
                {
                    views[name] = new_panel;
                    ChangeConstrainingSize();
                }
                new_panel.BackColor = (Color)color;
                new_panel.Mm_location = location;
                new_panel.Location = ScalePoint(location, scaling);
                new_panel.Name = name;
                new_panel.Mm_size = size;
                new_panel.Size = ScaleSize(size, scaling);
                if(is_elliptic)
                {
                    new_panel.Shape = new EventHandler(new_panel.ShapeElliptic);
                }
                references.Add(new_panel.Name);
                return new_panel;
            }
            else { throw new Exception("The specified position does not contain a subcontainer in this VisualPart"); }
        }

        /*
         * views_names : keys = views from added visualPart, values = positions in the current visualPart
         * locations : keys = views from added visualPart, values = locations in the current visualPart
         * 
         * create a new panel in current visualPart for each concerned view (path = position + name)
         */
        //AddVisualPart
        public void AddVisualPart(string name, VisualPart visual_part, Dictionary<string, string> views_names, Dictionary<string, Point> locations)
        {
            visual_part.ChangeScaling(scaling);
            pieces.Add(name, new Tuple<VisualPart, Dictionary<string, string>>(visual_part, new Dictionary<string, string>()));
            foreach(string view in views_names.Keys)
            {
                List<string> position = ConvertToPosition(views_names[view]);
                position.Add(name);
                string view_container = ConvertToName(position);
                VPPanel container = AddPanel(view_container, locations[view], visual_part.Views[view].Mm_size);

                //container.BorderStyle = BorderStyle.FixedSingle;//MODIF wtf apparence bizarre?

                pieces[name].Item2[view_container] = visual_part.Views[view].Name;
                container.Controls.Add(visual_part.Views[view]);
                OrderedDictionary size = new OrderedDictionary()
                {
                    {"slave", null },
                    {"master_sizes", null },
                    {"axis_dependency", new Tuple<bool, bool>(true, true) },
                    {"axis_inversion", false }
                };
                Action<VPPanel, Tuple<Size, Size>, Tuple<bool, bool>, bool> SizCopySizeChangeRule = visual_part.CopySizeChangeRule;
                AddRule(container.Name, string.Concat(view, "_" + name), SizCopySizeChangeRule, size, typeof(Size));
                //MODIF desactive peut etre l'opportunite de cliquer sur l'etage, a verifier
            }
        }

        /*
         * 
         */
        //RemoveVisualPart
        public void RemoveVisualPart(string name)
        {
            foreach(string reference in pieces[name].Item2.Keys)
            {
                RemovePanel(name);
            }
            pieces.Remove(name);
        }

        /*
         * 
         */
        //RemovePanel
        public void RemovePanel(string name)
        {
            List<string> position = ConvertToPosition(name);
            List<string> container_position = position.Take(position.Count() - 1).ToList();
            VPPanel container = GetPanel(ConvertToName(container_position));
            container.Controls.RemoveByKey(name);
            references.Remove(name);
            if (position.Count() == 1)
            {
                views.Remove(name);
            }
            positions = RemovePosition(positions, position);
        }

        /*
         * 
         */
        //ResizePanel
        public void ResizePanel(VPPanel panel, Size new_size)
        {
            Size old_size = panel.Size;
            panel.Size = new_size;
            foreach(string slave in panel.Rules.Keys)
            {
                foreach(Rule rule in panel.Rules[slave])
                {
                    if(rule.Trigger == typeof(Size))
                    {
                        string visual_part = ConvertToPosition(slave).First();
                        string view = ConvertToPosition(slave).Last();
                        Dictionary<object, object> args = new Dictionary<object, object>()
                        {
                            {
                                "slave",
                                (pieces.Keys.Contains(visual_part)) ? 
                                    pieces[visual_part].Item1.GetPanel(view) : 
                                    GetPanel(slave)
                            },
                            { "master_sizes", new Tuple<Size, Size>(old_size, new_size) }
                        };
                        rule.Execute(args);
                    }
                }
            }
        }

        /*
         * 
         */
        //RelocatePanel
        public void RelocatePanel(VPPanel panel, Point new_location)
        {
            Point old_location = panel.Location;
            panel.Location = new_location;
            foreach (string slave in panel.Rules.Keys)
            {
                foreach (Rule rule in panel.Rules[slave])
                {
                    if (rule.Trigger == typeof(Point))
                    {
                        Dictionary<object, object> args = new Dictionary<object, object>()
                        {
                            { "slave", GetPanel(slave) },
                            { "master_locations", new Tuple<Point, Point>(old_location, new_location) }
                        };
                        rule.Execute(args);
                    }
                }
            }
        }

        /*
         * //MODIF on peut certainement modifier tous les ADDRULES pour qu'ils dépendent d'un code à paramètres (pcq la c'est du recopiage)
         */
        //AddRule
        public void AddRule(string ref_master, string ref_slave, Delegate action, OrderedDictionary args, Type Ttrigger)
        {
            Type Ttarget = typeof(VPPanel);
            VPPanel master = GetPanel(ref_master);
            if(!master.Rules.ContainsKey(ref_slave))
            {
                master.Rules[ref_slave] = new List<Rule>();
            }
            master.Rules[ref_slave].Add(new Rule(action, args, Ttrigger, Ttarget));
        }

        /*
         * 
         */
        //AddSizeDependentPositionRule
        public void AddSizeDependentPositionRule(string ref_master, string ref_slave, Tuple<bool, bool> axis_dependency, bool axis_inversion)
        {
            OrderedDictionary size = new OrderedDictionary()
            {
                {"slave", null },
                {"master_sizes", null },
                {"axis_dependency", axis_dependency },
                {"axis_inversion", axis_inversion }
            };
            Action<VPPanel, Tuple<Size, Size>, Tuple<bool, bool>, bool> RuleSizeDependentPositionRule = SizeDependentPositionRule;
            AddRule(ref_master, ref_slave, RuleSizeDependentPositionRule, size, typeof(Size));
        }

        /*
         * 
         */
        //AddCopySizeChangeRule
        public void AddCopySizeChangeRule(string ref_master, string ref_slave, Tuple<bool, bool> axis_dependency, bool axis_inversion)
        {
            OrderedDictionary size = new OrderedDictionary()
            {
                {"slave", null },
                {"master_sizes", null },
                {"axis_dependency", axis_dependency },
                {"axis_inversion", axis_inversion }
            };
            Action<VPPanel, Tuple<Size, Size>, Tuple<bool, bool>, bool> RuleCopySizeChangeRule = CopySizeChangeRule;
            AddRule(ref_master, ref_slave, RuleCopySizeChangeRule, size, typeof(Size));
        }

        /*
         * 
         */
        //AddCopySizeProportionRule
        public void AddCopySizeProportionRule(string ref_master, string ref_slave, Tuple<bool, bool> axis_dependency, bool axis_inversion)
        {
            OrderedDictionary size = new OrderedDictionary()
            {
                {"slave", null },
                {"master_sizes", null },
                {"axis_dependency", axis_dependency },
                {"axis_inversion", axis_inversion }
            };
            Action<VPPanel, Tuple<Size, Size>, Tuple<bool, bool>, bool> RuleCopySizeProportionRule = CopySizeProportionRule;
            AddRule(ref_master, ref_slave, RuleCopySizeProportionRule, size, typeof(Size));
        }

        /*
         * 
         */
        //AddNoOverlappingRule
        public void AddNoOverlappingRule(string ref_master, string ref_slave, Tuple<bool, bool> axis_dependency, bool axis_inversion)
        {
            OrderedDictionary location = new OrderedDictionary()
            {
                {"slave", null },
                {"master_locations", null },
                {"axis_dependency", axis_dependency },
                {"axis_inversion", axis_inversion }
            };
            Action<VPPanel, Tuple<Point, Point>, Tuple<bool, bool>, bool> RuleNoOverlappingRule = NoOverlappingRule;
            AddRule(ref_master, ref_slave, RuleNoOverlappingRule, location, typeof(Point));
            OrderedDictionary size = new OrderedDictionary()
            {
                {"slave", null },
                {"master_sizes", null },
                {"axis_dependency", axis_dependency },
                {"axis_inversion", axis_inversion }
            };
            Action<VPPanel, Tuple<Size, Size>, Tuple<bool, bool>, bool> SizNoOverlappingRule = NoOverlappingRule;
            AddRule(ref_master, ref_slave, SizNoOverlappingRule, size, typeof(Size));
        }

        /*
         * master_locations : old, new
         * axis_dependency : x, y : refering to the master
         */
        //NoOverlappingRule : location changed
        public void NoOverlappingRule(VPPanel slave, Tuple<Point, Point> master_locations, 
            Tuple<bool, bool> axis_dependency, bool axis_inversion)
        {
            int dlx = Convert.ToInt32(axis_dependency.Item1) * (master_locations.Item2.X - master_locations.Item1.X);
            int dly = Convert.ToInt32(axis_dependency.Item2) * (master_locations.Item2.Y - master_locations.Item1.Y);
            RelocatePanel(
                slave, 
                new Point(
                    slave.Location.X + (axis_inversion ? dly : dlx), 
                    slave.Location.Y + (axis_inversion ? dlx : dly)));
        }

        /*
         *
         */
        //NoOverLappingRule : size changed
        public void NoOverlappingRule(VPPanel slave, Tuple<Size, Size> master_sizes, 
            Tuple<bool, bool> axis_dependency, bool axis_inversion)
        {
            int dsx = Convert.ToInt32(axis_dependency.Item1) * (master_sizes.Item2.Width - master_sizes.Item1.Width);
            int dsy = Convert.ToInt32(axis_dependency.Item2) * (master_sizes.Item2.Height - master_sizes.Item1.Height);
            RelocatePanel(
                slave, 
                new Point(
                    slave.Location.X + (axis_inversion ? dsy : dsx), 
                    slave.Location.Y + (axis_inversion ? dsx : dsy)));
        }

        /*
         * 
         */
        //CopySizeChangeRule
        public void CopySizeChangeRule(VPPanel slave, Tuple<Size, Size> master_sizes,
            Tuple<bool, bool> axis_dependency, bool axis_inversion)
        {
            int dx = Convert.ToInt32(axis_dependency.Item1) * (master_sizes.Item2.Width - master_sizes.Item1.Width);
            int dy = Convert.ToInt32(axis_dependency.Item2) * (master_sizes.Item2.Height - master_sizes.Item1.Height);
            ResizePanel(
                slave, 
                new Size(
                    slave.Size.Width + (axis_inversion ? dy : dx), 
                    slave.Size.Height + (axis_inversion ? dx : dy)));
        }

        /*
         * 
         */
        //CopySizeProportionRule 
        public void CopySizeProportionRule(VPPanel slave, Tuple<Size, Size> master_sizes,
            Tuple<bool, bool> axis_dependency, bool axis_inversion)
        {
            double cx = Convert.ToInt32(axis_dependency.Item1) * (master_sizes.Item2.Width / master_sizes.Item1.Width);
            double cy = Convert.ToInt32(axis_dependency.Item2) * (master_sizes.Item2.Height / master_sizes.Item1.Height);
            ResizePanel(
                slave,
                new Size(
                    Convert.ToInt32(slave.Size.Width * (axis_inversion ? cy : cx)),
                    Convert.ToInt32(slave.Size.Height * (axis_inversion ? cx : cy))));
        }

        /*
         * 
         */
        //SizeDependentPositionRule
        public void SizeDependentPositionRule(VPPanel slave, Tuple<Size, Size> master_sizes, 
            Tuple<bool, bool> axis_dependency, bool axis_inversion)
        {
            double sx = slave.Size.Width;
            double sy = slave.Size.Height;
            double lx = slave.Location.X + sx / 2;
            double ly = slave.Location.Y + sy / 2;
            double x = (master_sizes.Item2.Width / master_sizes.Item1.Width);
            double y = (master_sizes.Item2.Height / master_sizes.Item1.Height);
            if(axis_dependency.Item1)
            {
                if(axis_inversion)
                {
                    ly = ly * x;
                }
                else
                {
                    lx = lx * x;
                }
            }
            if(axis_dependency.Item2)
            {
                if(axis_inversion)
                {
                    lx = lx * y;
                }
                else
                {
                    ly = ly * y;
                }
            }
            lx = lx - sx / 2;
            ly = ly - sy / 2;
            RelocatePanel(slave, new Point(Convert.ToInt32(lx), Convert.ToInt32(ly)));
        }

        /*
         * 
         */
        //Click
        public void Click(object sender, EventArgs e)
        {
            selection.Execute(
                new Dictionary<object, object>()
                {
                    { "sender", sender }
                });
        }

        /*
         * 
         */
        //SelectPiece
        public void SelectPiece(object sender)
        {
            if(!typeof(VPPanel).IsInstanceOfType(sender))
            {
                return;
            }
            VPPanel current_sender = (VPPanel)sender;
            string piece_name = ConvertToPosition(current_sender.Name).Last();
            if(pieces.Keys.Contains(piece_name))
            {
                UndoFocus();
                pointer = current_sender.Name;
                Focus(current_sender);
                SearchScreen(sender);
            }
            else
            {
                SelectPiece(current_sender.Parent);
            }
        }

        /*
         * 
         */
        //SearchScreen
        public void SearchScreen(object sender)
        {
            if(!typeof(VPPanel).IsInstanceOfType(sender))
            {
                ((Control)sender).Focus();
                ((Control)sender).Parent.Focus();
            }
            else
            {
                SearchScreen(((Control)sender).Parent);
            }
        }

        /*
         * 
         */
        //UndoFocus
        public void UndoFocus()
        {
            int alpha = 50;
            foreach(VPPanel view in views.Values)
            {
                VaryAlpha(alpha, view);
            }
        }

        /*
         * 
         */
        //CleanFocus
        public void CleanFocus()
        {
            int alpha = 255;
            foreach(VPPanel view in views.Values)
            {
                VaryAlpha(alpha, view);
            }
        }

        /*
         * 
         */
        //Focus
        public void Focus(object sender)
        {
            int alpha = 255;
            VaryAlpha(alpha, sender);
        }

        /*
         * 
         */
        //VaryAlpha
        public void VaryAlpha(int alpha, object sender)
        {
            if(alpha>255)
            {
                alpha = 255;
            }
            else if(alpha<0)
            {
                alpha = 0;
            }
            Control control_sender = (Control)sender;
            foreach (Control elem in control_sender.Controls)
            {
                elem.BackColor = Color.FromArgb(alpha, elem.BackColor.R, elem.BackColor.G, elem.BackColor.B);
                if (elem.HasChildren)
                {
                    VaryAlpha(alpha, elem);
                }
            }
        }

        /*
         * 
         */
        //MouseHover
        public void MouseHover(object sender, EventArgs e)
        {
            
        }
    }
}
//end

//MODIF : ajouter changement de position/dimension à partir d'un visualpart, ajouter les références aux 3 dimensions de l'espace pour toutes les
//vues => modifier les dimensions du visualpart conteneur, modifier la position des visualpart contenues.
//MODIF : ResizePanel & RelocatePanel => problemes d'arrondis avec les rules? à vérifier


    //DOUBLE MODIF : LE POINTER QUI DIT QUEL ETAGE ON SELECTIONNE EST CELUI DE LETAGE ET PAS DE LARMOIRE
    // ENSUITE : LE CLICK DES CONTENUS NE SONT PAS SUPPRIMES : ON A UN DOUBLE CLIC LORS DE LEVENT CLIC.