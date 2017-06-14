using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections.Specialized;

namespace Kitbox
{
    public class VPPanel : System.Windows.Forms.Panel
    {
        private Size mm_size;
        private Point mm_location;
        private int zpos;
        private string xdim;
        private string ydim;//MODIF (testeur d'overlapping)
        private Dictionary<string, List<Rule>> rules;
        private EventHandler shape;

        public VPPanel()
        {
            rules = new Dictionary<string, List<Rule>>();
            
        }

        public Size Mm_size
        {
            set { mm_size = value; }
            get { return mm_size; }
        }

        public Point Mm_location
        {
            set { mm_location = value; }
            get { return mm_location; }
        }

        public int Zpos
        {
            set { zpos = value; }
            get { return zpos; }
        }

        public Dictionary<string, List<Rule>> Rules
        {
            get { return rules; }
        }

        public void AddRule(string slave, Rule rule)
        {
            if(!rules.ContainsKey(slave))
            {
                rules[slave] = new List<Rule>()
                {
                    rule
                };
            }
            else if(rules[slave].Contains(rule))
            {
                throw new Exception("This rule already exists");
            }
            else
            {
                rules[slave].Add(rule);
            }
        }

        public void RemoveRule(string slave, Rule rule)
        {
            if(rules.ContainsKey(slave) && rules[slave].Contains(rule))
            {
                rules[slave].Remove(rule);
            }
            else
            {
                throw new Exception("This rule does not exists");
            }
        }

        public void RemoveSlave(string slave)
        {
            if(rules.ContainsKey(slave))
            {
                rules.Remove(slave);
            }
            else
            {
                throw new Exception("This slave does not exists");
            }
        }

        public void ShapeElliptic(Object sender, EventArgs e)
        {
            GraphicsPath path = new GraphicsPath();
            path.AddEllipse(new Rectangle(0, 0, Size.Width, Size.Height));
            Region = new Region(path);
        }

        public EventHandler Shape
        {
            set
            {
                SizeChanged -= shape;
                SizeChanged += value;
                shape = value;
            }
            get
            {
                return shape;
            }
        }
    }
}
