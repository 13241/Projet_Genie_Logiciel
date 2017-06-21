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
    /// <summary>
    /// Represents an interactive graphic figure, contains one or more views, each component can be selected by clicking on it,
    /// the components are either a VPPanel or another VisualPart
    /// </summary>
    public class VisualPart
    {
        //attributes
        /// <summary>
        /// tree (mathematics). The keys are the positions of the differents VPPanel, the values are other Dictionary with the
        /// same characteristics
        /// </summary>
        private Dictionary<string, object> positions;
        /// <summary>
        /// views as in a technical drawing, key : name of the view
        /// </summary>
        private Dictionary<string, VPPanel> views;
        /// <summary>
        /// most constrainig Size for the visualPart in the desired unit of measurement
        /// </summary>
        private Size mm_size;
        /// <summary>
        /// The size of the screen which will display the VisualPart (pixels)
        /// </summary>
        private Size px_size;
        /// <summary>
        /// pixels per unit of measurement
        /// </summary>
        private double scaling;
        /// <summary>
        /// references every VPPanel of the VisualPart
        /// </summary>
        private HashSet<string> references;
        /// <summary>
        /// Keys : pieces names, Values : Tuple : VisualPart of the piece, Keys : the name of the VPPanel which contains a view of the related VisualPart, Values : the name of the view in the related VisualPart
        /// </summary>
        private Dictionary<string, Tuple<VisualPart, Dictionary<string, string>>> pieces;

        //EventAttributes
        /// <summary>
        /// the name of the selected component
        /// </summary>
        private string pointer = "";
        /// <summary>
        /// the method used to select a component
        /// </summary>
        private Rule selection;
        
        //VisualPart
        /// <summary>
        /// Initialize a new instance of the class VisualPart.
        /// </summary>
        /// <param name="px_size">The size of the screen which will display the VisualPart (pixels)</param>
        /// <param name="mm_sizes">The sizes of the views (keys are the views names) (unit of measurement)</param>
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

        //CreateViews
        /// <summary>
        /// Construct as many VPPanel as the number of elements in mm_sizes, with the related dimensions
        /// </summary>
        /// <param name="mm_sizes">The sizes of the views (keys are the views names) (unit of measurement)</param>
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
        
        //ChangeConstrainingSize
        /// <summary>
        /// Determine the most constraining Size for the VisualPart and adapt the scaling
        /// </summary>
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

        //ChangeScaling : scaling
        /// <summary>
        /// Adapt the scaling of the VisualPart and each one of its components
        /// </summary>
        /// <param name="scaling">pixels per unit of measurement</param>
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

        //ChangeScaling : px_size
        /// <summary>
        /// Adapt the scaling of the VisualPart and each one of its components
        /// </summary>
        /// <param name="px_size">the new size (in pixels) of the screen used to display the VisualPart</param>
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
        
        //ScalePoint
        /// <summary>
        /// Rescale a Point (localisation)
        /// </summary>
        /// <param name="point"></param>
        /// <param name="scaling"></param>
        /// <returns> a new Point with the scaled dimensions (in pixels)</returns>
        private Point ScalePoint(Point point, double scaling)
        {
            return new Point(Convert.ToInt32(Math.Round(point.X * scaling)), Convert.ToInt32(Math.Round(point.Y * scaling)));
        }
        
        //ScaleSize
        /// <summary>
        /// Rescale a Size (dimension)
        /// </summary>
        /// <param name="size"></param>
        /// <param name="scaling"></param>
        /// <returns>a new Size with the scaled dimensions (in pixels)</returns>
        private Size ScaleSize(Size size, double scaling)
        {
            return new Size(Convert.ToInt32(Math.Round(size.Width * scaling)), Convert.ToInt32(Math.Round(size.Height * scaling)));
        }

        //Views
        /// <summary>
        /// views as in a technical drawing, key : name of the view
        /// </summary>
        public Dictionary<string, VPPanel> Views
        {
            get { return views; }
        }

        //Pointer
        /// <summary>
        /// the name of the selected component (in the VisualPart)
        /// </summary>
        public string Pointer
        {
            get { return pointer; }
        }

        //Display
        /// <summary>
        /// Adapt the method called when clicking on a component of the VisualPart for every components
        /// </summary>
        /// <returns> views as in a technical drawing, key : name of the view </returns>
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

        //ChangeEventHandler
        /// <summary>
        /// Adapt the method called when clicking on a component of the VisualPart for every components
        /// </summary>
        /// <param name="selection"></param>
        public void ChangeEventHandler(Rule selection)
        {
            this.selection = selection;
            foreach (string ref_piece in pieces.Keys)
            {
                pieces[ref_piece].Item1.ChangeEventHandler(selection);
            }
        }

        //References
        /// <summary>
        /// references every VPPanel of the VisualPart
        /// </summary>
        public HashSet<string> References
        {
            get { return references; }
        }

        //Mm_size
        /// <summary>
        /// most constrainig Size for the visualPart in the desired unit of measurement
        /// </summary>
        public Size Mm_size
        {
            get { return mm_size; }
        }

        //Positions
        /// <summary>
        /// tree (mathematics). The keys are the positions of the differents VPPanel, the values are other Dictionary with the
        /// same characteristics
        /// </summary>
        public Dictionary<string, object> Positions
        {
            get { return positions; }
        }

        //Pieces
        /// <summary>
        /// Keys : pieces names, Values : Tuple : VisualPart of the piece, Keys : the name of the VPPanel which contains a view of the related VisualPart, Values : the name of the view in the related VisualPart
        /// </summary>
        public Dictionary<string, Tuple<VisualPart, Dictionary<string, string>>> Pieces
        {
            get { return pieces; }
        }
        
        //ReinsertPiece
        /// <summary>
        /// When a piece is displayed in a screen, the related controls are removed from the VisualPart that contained the piece
        /// This method adds the said removed controls in the right place anew
        /// </summary>
        /// <param name="name">the name of the piece that had been displayed</param>
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

        //Scaling
        /// <summary>
        /// pixels per unit of measurement
        /// </summary>
        public double Scaling
        {
            get { return scaling; }
        }
        
        
        //ConvertToPosition
        /// <summary>
        /// The name(position) of a VPPanel contains the path used to attains it, as so, it contains the names(exclusive) of every Parent it has
        /// This method returns these names(exclusive) in a List
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<string> ConvertToPosition(string name)
        {
            return name.Split('_').ToList();
        }
        
        //ConvertToName
        /// <summary>
        /// Create the name(position) of a VPPanel with a List of the names(exclusive) of each of its Parent
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public string ConvertToName(List<string> position)
        {
            return string.Join("_", position);
        }
        
        //HasSubContainer
        /// <summary>
        /// Checks if a VPPanel has a Parent in the VisualPart
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        private bool HasSubcontainer(List<string> position)
        {
            if (!references.Contains(ConvertToName(position.Take(position.Count() - 1).ToList())))
            { return false; }
            return true;
        }
        
        //EnlargeView
        /// <summary>
        /// Change the size (unit of measurement) of a view
        /// </summary>
        /// <param name="view_name"></param>
        /// <param name="mm_size"></param>
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
        
        //GetPanel
        /// <summary>
        /// Returns the VPPanel with the name(position)
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
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

        //AlterPositions
        /// <summary>
        /// Alter (add or remove) a position in the tree Positions
        /// Rem : recursive function, does not modify attribute positions
        /// </summary>
        /// <param name="positions"></param>
        /// <param name="position"></param>
        /// <param name="deleting"></param>
        /// <returns></returns>
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
        
        //RemovePosition
        /// <summary>
        /// Remove a position in the tree Positions
        /// </summary>
        /// <param name="positions"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        private Dictionary<string, object> RemovePosition(Dictionary<string, object> positions, List<string> position)
        {
            return AlterPositions(positions, position, true);
        }
        
        //AddPosition
        /// <summary>
        /// Add a position in the tree Positions
        /// </summary>
        /// <param name="positions"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        private Dictionary<string, object> AddPosition(Dictionary<string, object> positions, List<string> position)
        {
            return AlterPositions(positions, position, false);
        }

        //AddPanel
        /// <summary>
        /// Add a VPPanel in the VisualPart
        /// </summary>
        /// <param name="name">the name(exclusive) of the VPPanel to be added</param>
        /// <param name="location"> the location of the VPPanel in the VisualPart (unit of measurement) </param>
        /// <param name="size"> The size of the VPPanel to be added (unit of measurement) </param>
        /// <param name="color"> The color of the VPPanel to be added </param>
        /// <param name="is_elliptic"> Specify if the VPPanel is elliptic (true) or rectangular (false) </param>
        /// <returns></returns>
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
        
        //AddVisualPart
        /// <summary>
        /// Add a VisualPart in the current VisualPart
        /// </summary>
        /// <param name="name"> The name of the VisualPart to be added </param>
        /// <param name="visual_part"> The VisualPart to be added </param>
        /// <param name="views_names"> a synchronisation of the views : keys : views from added VisualPart, values : name(position) in the current VisualPart </param>
        /// <param name="locations"> a synchronisation of the locations : keys : views from added VisualPart, values : location in the current VisualPart </param>
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
        
        //RemoveVisualPart
        /// <summary>
        /// Remove the VisualPart with the name
        /// </summary>
        /// <param name="name"></param>
        public void RemoveVisualPart(string name)
        {
            foreach(string reference in pieces[name].Item2.Keys)
            {
                RemovePanel(name);
            }
            pieces.Remove(name);
        }
        
        //RemovePanel
        /// <summary>
        /// Remove the VPPanel with the name
        /// </summary>
        /// <param name="name"></param>
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
        
        //ResizePanel
        /// <summary>
        /// Resize a VPPanel
        /// </summary>
        /// <param name="panel"></param>
        /// <param name="new_size"></param>
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
        
        //RelocatePanel
        /// <summary>
        /// Relocate a VPPanel
        /// </summary>
        /// <param name="panel"></param>
        /// <param name="new_location"></param>
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
        /// <summary>
        /// Add a Rule to a VPPanel, in short : when a dimension of a master VPPanel change, all of its slave must apply the related
        /// Rule. NONFONCTIONNAL
        /// </summary>
        /// <param name="ref_master"></param>
        /// <param name="ref_slave"></param>
        /// <param name="action"></param>
        /// <param name="args"></param>
        /// <param name="Ttrigger"></param>
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

        //AddSizeDependentPositionRule
        /// <summary>
        /// Add a rule which modify the location of a slave if a master Size change
        /// </summary>
        /// <param name="ref_master"></param>
        /// <param name="ref_slave"></param>
        /// <param name="axis_dependency"></param>
        /// <param name="axis_inversion"></param>
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
        
        //AddCopySizeChangeRule
        /// <summary>
        /// Add a rule which copy the change in size of the master (raw)
        /// </summary>
        /// <param name="ref_master"></param>
        /// <param name="ref_slave"></param>
        /// <param name="axis_dependency"></param>
        /// <param name="axis_inversion"></param>
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
        
        //AddCopySizeProportionRule
        /// <summary>
        /// Add a rule which copy the change in size of the master (proportionnal)
        /// </summary>
        /// <param name="ref_master"></param>
        /// <param name="ref_slave"></param>
        /// <param name="axis_dependency"></param>
        /// <param name="axis_inversion"></param>
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
        
        //AddNoOverlappingRule
        /// <summary>
        /// Add a rule wich relocate the slave if a master is resized or relocated
        /// </summary>
        /// <param name="ref_master"></param>
        /// <param name="ref_slave"></param>
        /// <param name="axis_dependency"></param>
        /// <param name="axis_inversion"></param>
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
        /// <summary>
        /// The rule which relocate a slave if the master is relocated
        /// </summary>
        /// <param name="slave"></param>
        /// <param name="master_locations"></param>
        /// <param name="axis_dependency"></param>
        /// <param name="axis_inversion"></param>
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
        
        //NoOverLappingRule : size changed
        /// <summary>
        /// The rule which relocate a slave if the master is resized
        /// </summary>
        /// <param name="slave"></param>
        /// <param name="master_sizes"></param>
        /// <param name="axis_dependency"></param>
        /// <param name="axis_inversion"></param>
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
        
        //CopySizeChangeRule
        /// <summary>
        /// The rule which change the size of a slave if the size of the master is modified
        /// </summary>
        /// <param name="slave"></param>
        /// <param name="master_sizes"></param>
        /// <param name="axis_dependency"></param>
        /// <param name="axis_inversion"></param>
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
        /// <summary>
        /// The rule which change the size of a slave if the master size is modified (proportions)
        /// </summary>
        /// <param name="slave"></param>
        /// <param name="master_sizes"></param>
        /// <param name="axis_dependency"></param>
        /// <param name="axis_inversion"></param>
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
        
        //SizeDependentPositionRule
        /// <summary>
        /// The rule which change the position if the size is changed
        /// </summary>
        /// <param name="slave"></param>
        /// <param name="master_sizes"></param>
        /// <param name="axis_dependency"></param>
        /// <param name="axis_inversion"></param>
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
        
        //Click
        /// <summary>
        /// The method called when clicking on a component of the VisualPart
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Click(object sender, EventArgs e)
        {
            selection.Execute(
                new Dictionary<object, object>()
                {
                    { "sender", sender }
                });
        }
        
        //SelectPiece
        /// <summary>
        /// This method can be used to select a Component of the VisualPart, the colors will be adapted to
        /// have a better visualization of the selection, the selected Component will have its name stored in Pointer
        /// </summary>
        /// <param name="sender"></param>
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
        
        //SearchScreen
        /// <summary>
        /// This method calls the focus on the screen used to display the VisualPart (=> can be used to clean a selection)
        /// The focus is then passed to the Parent of the screen so the screen can be Focuses again later
        /// </summary>
        /// <param name="sender"></param>
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
        
        //UndoFocus
        /// <summary>
        /// This method change the alpha of the colors of the non-selected components of the VisualPart
        /// </summary>
        public void UndoFocus()
        {
            int alpha = 50;
            foreach(VPPanel view in views.Values)
            {
                VaryAlpha(alpha, view);
            }
        }
        
        //CleanFocus
        /// <summary>
        /// This method recover the original colors of the components of the VisualPart
        /// </summary>
        public void CleanFocus()
        {
            int alpha = 255;
            foreach(VPPanel view in views.Values)
            {
                VaryAlpha(alpha, view);
            }
        }
        
        //Focus
        /// <summary>
        /// this method change the alpha of a single Component (must be used with undoFocus to create a visual effect for the selection)
        /// </summary>
        /// <param name="sender"></param>
        public void Focus(object sender)
        {
            int alpha = 255;
            VaryAlpha(alpha, sender);
        }
        
        //VaryAlpha
        /// <summary>
        /// change the alpha value of the colors of a component
        /// </summary>
        /// <param name="alpha"></param>
        /// <param name="sender"></param>
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