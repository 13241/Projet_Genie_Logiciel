using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using FPanel = System.Windows.Forms.Panel;


namespace Kitbox
{
    public class VisualPart
    {
        //attributes
        private Dictionary<string, object> positions;
        private Dictionary<string, Tuple<FPanel, Size>> views; //views as in a technical drawing, key : name, value : Panel, Size in milimeters
        private Size mm_size; //most constrainig size for the visualPart
        private Size px_size; //size for the viewer (in pixels)
        private double scaling; //pixels per milimeter
        private Dictionary<
            string, //references each panel of the VisualPart
            Dictionary<
                string, //slaves of the panel
                List<Rule>> 
            > references;
        private Dictionary<string, Tuple<VisualPart, HashSet<string>>> pieces;


        //methods
        /*
         * 
         */
        //BasicSizes
        public static Dictionary<string, Size> BasicSizes(Size mm_size)
        {
            return new Dictionary<string, Size>()
            {
                { "front", new Size(mm_size.Width, mm_size.Height) },
                { "rear", new Size(mm_size.Width, mm_size.Height) },
                { "left", new Size(mm_size.Width, mm_size.Height) },
                { "right", new Size(mm_size.Width, mm_size.Height) },
                { "top", new Size(mm_size.Width, mm_size.Height) },
                { "bottom", new Size(mm_size.Width, mm_size.Height) }
            };
        }

        /*
         * constructor
         * mm_size : most constraining size in milimeters
         * px_size : size available to display a view in pixels
         */
        //VisualPart
        public VisualPart(Size px_size, Dictionary<string, Size> mm_sizes = null)
        {
            references = new Dictionary<string, Dictionary<string, List<Rule>>>();
            pieces = new Dictionary<string, Tuple<VisualPart, HashSet<string>>>();
            positions = new Dictionary<string, object>();

            this.px_size = px_size;

            if(mm_sizes == null)
            {
                mm_sizes = BasicSizes(px_size);
            }
            CreateViews(mm_sizes);
            ChangeConstrainingSize();
        }

        /*
         * 
         */
        //CreateViews
        private void CreateViews(Dictionary<string, Size> mm_sizes)
        {
            this.views = new Dictionary<string, Tuple<FPanel, Size>>();
            List<string> list_views = mm_sizes.Keys.ToList();
            for (int i = 0; i < list_views.Count(); i++)
            {
                FPanel new_panel = new FPanel();
                new_panel.BackColor = SystemColors.Control;
                new_panel.Location = new Point(0, 0);
                new_panel.Name = list_views[i];
                new_panel.Size = mm_sizes[list_views[i]];
                views[list_views[i]] = new Tuple<FPanel, Size>(new_panel, mm_sizes[list_views[i]]);
                positions[list_views[i]] = null;
                references.Add(list_views[i],
                    new Dictionary<string, List<Rule>>());
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
                if (views[view].Item2.Width > maxWidth)
                {
                    maxWidth = views[view].Item2.Width;
                }
                if (views[view].Item2.Height > maxHeight)
                {
                    maxHeight = views[view].Item2.Height;
                }
            }
            this.mm_size = new Size(maxWidth, maxHeight);
            ChangeScaling();
        }

        /*
         * 
         */
        //ChangeScaling
        public void ChangeScaling(Size? px_size = null)
        {
            if(px_size == null)
            {
                px_size = this.px_size;
            }
            else
            {
                this.px_size = (Size)px_size;
            }
            double old_scaling = scaling;
            this.scaling = Math.Min((double)this.px_size.Width / mm_size.Width, (double)this.px_size.Height / mm_size.Height);
            foreach(string reference in references.Keys)
            {
                List<string> position = ConvertToPosition(reference);
                FPanel current = GetPanel(position);
                RelocatePanel(current, ScalePoint(current.Location, scaling / old_scaling));
                ResizePanel(current, ScaleSize(current.Size, scaling / old_scaling));
            }
        }

        /*
         * 
         */
        //ScalePoint
        private Point ScalePoint(Point point, double scaling)
        {
            return new Point(Convert.ToInt32(point.X * scaling), Convert.ToInt32(point.Y * scaling));
        }

        /*
         * 
         */
        //ScaleSize
        private Size ScaleSize(Size size, double scaling)
        {
            return new Size(Convert.ToInt32(size.Width * scaling), Convert.ToInt32(size.Height * scaling));
        }

        /*
         * 
         */
        //Views
        public Dictionary<string, Tuple<FPanel, Size>> Views
        {
            get { return views; }
        }

        /*
         * 
         */
        //References
        public List<string> References
        {
            get { return references.Keys.ToList(); }
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
        public bool HasSubcontainer(List<string> position)
        {
            for (int i = 0; i<position.Count()-1; i++)
            {
                if (!positions.ContainsKey(position[i]))
                { return false; }
            }
            return true;
        }

        /*
         * 
         */
        //EnlargeView
        public void EnlargeView(string view_name, Size mm_size)
        {
            FPanel view = views[view_name].Item1;
            view.Size = ScaleSize(mm_size, scaling);
            views[view_name] = new Tuple<FPanel, Size>(view, mm_size);
            ChangeConstrainingSize();
        }

        /*
         * 
         */
        //GetPanel
        public FPanel GetPanel(List<string> position)
        {
            FPanel[] subcontainers;
            if (position.Count() == 1 && views.ContainsKey(position[0]))
            {
                subcontainers = new FPanel[1]
                    { views[position[0]].Item1 };
            }
            else if (position.Count() > 1 && views.ContainsKey(position[0]))
            {
                subcontainers = (FPanel[])views[position[0]].Item1.Controls.Find(ConvertToName(position), true);
            }
            else
            {
                subcontainers = new FPanel[0];
            }
            if (subcontainers.Count() == 1)
            {
                return subcontainers[0];
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
        public FPanel AddPanel(List<string> position, Point location, Size size, Color? color = null)
        {
            location = ScalePoint(location, scaling);
            size = ScaleSize(size, scaling);
            FPanel subcontainer;
            if (HasSubcontainer(position))
            {
                if (position.Count() > 1)
                {
                    subcontainer = GetPanel(position.Take(position.Count() - 1).ToList());
                }
                else if (position.Count() == 1)
                {
                    subcontainer = null;
                }
                else
                { throw new Exception("Specify a nonempty position"); }

                positions = AddPosition(positions, position);

                bool mouseHover = true;
                if (color == null)
                {
                    color = SystemColors.Control;
                    mouseHover = false;
                }

                FPanel new_panel = new FPanel();
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
                    views[ConvertToName(position)] = new Tuple<FPanel, Size>(new_panel, size);
                    ChangeConstrainingSize();
                }
                new_panel.BackColor = (Color)color;
                new_panel.Location = location;
                new_panel.Name = ConvertToName(position);
                new_panel.Size = size;
                references.Add(new_panel.Name,
                    new Dictionary<string, List<Rule>>());
                return new_panel;
            }
            else { throw new Exception("The specified position does not contain a subcontainer in this VisualPart"); }
        }

        /*
         * positions : keys = views from added visualPart, values = positions in the current visualPart
         * locations : keys = views from added visualPart, values = locations in the current visualPart
         * 
         * create a new panel in current visualPart for each concerned view (path = position + name)
         */
        //AddVisualPart
        public void AddVisualPart(string name, VisualPart visual_part, Dictionary<string, List<string>> positions, Dictionary<string, Point> locations)
        {
            visual_part.ChangeScaling(ScaleSize(visual_part.Mm_size, scaling));
            pieces.Add(name, new Tuple<VisualPart, HashSet<string>>(visual_part, new HashSet<string>()));
            foreach(string view in positions.Keys)
            {
                positions[view].Add(name);
                FPanel container = AddPanel(positions[view], locations[view], visual_part.Views[view].Item1.Size);
                pieces[name].Item2.Add(visual_part.ConvertToName(positions[view]));
                container.Controls.Add(visual_part.Views[view].Item1);
                OrderedDictionary size = new OrderedDictionary()
                {
                    {"slave", null },
                    {"master_sizes", null },
                    {"axis_dependency", null },
                    {"axis_inversion", false }
                };
                if (view == "front" || view == "left")
                {
                    if(view == "front")
                    {
                        size["axis_dependency"] = new Tuple<bool, bool>(true, true);
                    }
                    if(view == "left")
                    {
                        size["axis_dependency"] = new Tuple<bool, bool>(true, false);
                    }
                    Action<FPanel, Tuple<Size, Size>, Tuple<bool, bool>, bool> SizCopySizeChangeRule = visual_part.CopySizeChangeRule;
                    AddRule(container.Name, string.Concat(name, "_" + view), SizCopySizeChangeRule, size, typeof(Size));
                }

                //MODIF IL FAUT 12 bool true par visual part en 3 dimensions (2 par face 6 faces), il faut 3 dimensions master
                //sur 2 faces
                //MODIF déplacement en position des visualpart???
                //MODIF desactive peut etre l'opportunite de cliquer sur l'etage, a verifier, probleme lors de changements de dimensions?
            }
        }

        /*
         * 
         */
        //RemoveVisualPart
        public void RemoveVisualPart(string name)
        {
            foreach(string reference in pieces[name].Item2)
            {
                List<string> position = ConvertToPosition(reference);
                RemovePanel(position);
            }
            pieces.Remove(name);
        }

        /*
         * 
         */
        //RemovePanel
        public void RemovePanel(List<string> position)
        {
            string reference = ConvertToName(position);
            List<string> container_position = position.Take(position.Count() - 1).ToList();
            FPanel container = GetPanel(container_position);
            container.Controls.RemoveByKey(reference);
            references.Remove(reference);
            if (position.Count() == 1)
            {
                views.Remove(reference);
            }
            positions = RemovePosition(positions, position);
        }

        /*
         * 
         */
        //ResizePanel
        public void ResizePanel(FPanel panel, Size new_size)
        {
            Size old_size = panel.Size;
            panel.Size = new_size;
            foreach(string slave in references[panel.Name].Keys)
            {
                foreach(Rule rule in references[panel.Name][slave])
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
                                    pieces[visual_part].Item1.GetPanel(ConvertToPosition(view)) : 
                                    GetPanel(ConvertToPosition(slave))
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
        public void RelocatePanel(FPanel panel, Point new_location)
        {
            Point old_location = panel.Location;
            panel.Location = new_location;
            foreach (string slave in references[panel.Name].Keys)
            {
                foreach (Rule rule in references[panel.Name][slave])
                {
                    if (rule.Trigger == typeof(Point))
                    {
                        Dictionary<object, object> args = new Dictionary<object, object>()
                        {
                            { "slave", GetPanel(ConvertToPosition(slave)) },
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
            Type Ttarget = typeof(FPanel);
            if(references[ref_master][ref_slave] == null)
            {
                references[ref_master][ref_slave] = new List<Rule>();
            }
            references[ref_master][ref_slave].Add(new Rule(action, args, Ttrigger, Ttarget));
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
            Action<FPanel, Tuple<Size, Size>, Tuple<bool, bool>, bool> SizSizeDependentPositionRule = SizeDependentPositionRule;
            AddRule(ref_master, ref_slave, SizSizeDependentPositionRule, size, typeof(Size));
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
            Action<FPanel, Tuple<Size, Size>, Tuple<bool, bool>, bool> SizCopySizeChangeRule = CopySizeChangeRule;
            AddRule(ref_master, ref_slave, SizCopySizeChangeRule, size, typeof(Size));
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
            Action<FPanel, Tuple<Size, Size>, Tuple<bool, bool>, bool> SizCopySizeProportionRule = CopySizeProportionRule;
            AddRule(ref_master, ref_slave, SizCopySizeProportionRule, size, typeof(Size));
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
            Action<FPanel, Tuple<Point, Point>, Tuple<bool, bool>, bool> LocNoOverlappingRule = NoOverlappingRule;
            AddRule(ref_master, ref_slave, LocNoOverlappingRule, location, typeof(Point));
            OrderedDictionary size = new OrderedDictionary()
            {
                {"slave", null },
                {"master_sizes", null },
                {"axis_dependency", axis_dependency },
                {"axis_inversion", axis_inversion }
            };
            Action<FPanel, Tuple<Size, Size>, Tuple<bool, bool>, bool> SizNoOverlappingRule = NoOverlappingRule;
            AddRule(ref_master, ref_slave, SizNoOverlappingRule, size, typeof(Size));
        }

        /*
         * master_locations : old, new
         * axis_dependency : x, y : refering to the master
         */
        //NoOverlappingRule : location changed
        public void NoOverlappingRule(FPanel slave, Tuple<Point, Point> master_locations, 
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
        public void NoOverlappingRule(FPanel slave, Tuple<Size, Size> master_sizes, 
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
        public void CopySizeChangeRule(FPanel slave, Tuple<Size, Size> master_sizes,
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
        public void CopySizeProportionRule(FPanel slave, Tuple<Size, Size> master_sizes,
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
        public void SizeDependentPositionRule(FPanel slave, Tuple<Size, Size> master_sizes, 
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
        public void Click (object sender, EventArgs e)
        {
            
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