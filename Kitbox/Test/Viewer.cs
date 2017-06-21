using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kitbox
{
    public partial class Viewer : Form
    {
        VisualPart current;
        //WARNING COUPELLE DE DROITE PLACEMENT ERRONE?

        public Viewer()
        {
            InitializeComponent();
            //6 coupelles
            Stack<VisualPart> knops = new Stack<VisualPart>();
            for (int i = 0; i < 6; i++)
            {
                Size scaled = new Size(6, 6);
                VisualPart knop = new VisualPart(scaled, 
                    new Dictionary<string, Size>()
                    {
                        { "front", scaled }
                    });
                knop.AddPanel("front_knop", new Point(0, 0), scaled, Color.Black, true);
                knops.Push(knop);
            }
            //6 portes
            Stack<VisualPart> doors = new Stack<VisualPart>();
            for (int i = 0; i < 6; i++)
            {
                Size scaled = new Size(62, 32);
                VisualPart door = new VisualPart(scaled,
                    new Dictionary<string, Size>()
                    {
                        { "front", scaled }
                    });
                door.AddPanel("front_door", new Point(0, 0), scaled, Color.White);
                VisualPart knop = knops.Pop();
                int invert = Convert.ToInt32(Math.Pow(-1, i));//1 ou -1
                int take = Convert.ToInt32(((1 + invert) / 2));//1 ou 0
                AddVisualPart1v(door, knop, "knop", "front", "front_door",
                    new Point(take * (scaled.Width - knop.Mm_size.Width) - invert * 4, Convert.ToInt32((scaled.Height - knop.Mm_size.Height) / 2)));
                doors.Push(door);
            }
            //3 panneaux AR
            Stack<VisualPart> ARpanels = new Stack<VisualPart>();
            for (int i = 0; i<3; i++)
            {
                Size scaled = new Size(120, 32);
                VisualPart ARpanel = new VisualPart(scaled,
                    new Dictionary<string, Size>()
                    {
                        { "front", scaled }
                    });
                ARpanel.AddPanel("front_arpanel", new Point(0, 0), scaled, Color.White);
                ARpanels.Push(ARpanel);
            }
            //6 panneaux GD
            Stack<VisualPart> GDpanels = new Stack<VisualPart>();
            for(int i = 0; i<6; i++)
            {
                Size scaled = new Size(42, 32);
                VisualPart GDpanel = new VisualPart(scaled,
                    new Dictionary<string, Size>()
                    {
                        { "front", scaled }
                    });
                GDpanel.AddPanel("front_gdpanel", new Point(0, 0), scaled, Color.White);
                GDpanels.Push(GDpanel);
            }
            //6 panneaux HB
            Stack<VisualPart> HBpanels = new Stack<VisualPart>();
            for (int i = 0; i < 6; i++)
            {
                Size scaled = new Size(120, 42);
                VisualPart HBpanel = new VisualPart(scaled,
                    new Dictionary<string, Size>()
                    {
                        { "front", scaled }
                    });
                HBpanel.AddPanel("front_HBpanel", new Point(0, 0), scaled, Color.White);
                HBpanels.Push(HBpanel);
            }
            //6 traverses AV
            Stack<VisualPart> AVnoggings = new Stack<VisualPart>();
            for (int i = 0; i < 6; i++)
            {
                Size scaled = new Size(120, 2);
                VisualPart AVnogging = new VisualPart(scaled,
                    new Dictionary<string, Size>()
                    {
                        { "front", scaled }
                    });
                AVnogging.AddPanel("front_AVnogging", new Point(0, 0), scaled, Color.Tan);
                AVnoggings.Push(AVnogging);
            }
            //6 traverses AR
            Stack<VisualPart> ARnoggings = new Stack<VisualPart>();
            for (int i = 0; i < 6; i++)
            {
                Size scaled = new Size(120, 2);
                VisualPart ARnogging = new VisualPart(scaled,
                    new Dictionary<string, Size>()
                    {
                        { "front", scaled }
                    });
                ARnogging.AddPanel("front_ARnogging", new Point(0, 0), scaled, Color.Tan);
                ARnoggings.Push(ARnogging);
            }
            //12 traverses GD
            Stack<VisualPart> GDnoggings = new Stack<VisualPart>();
            for (int i = 0; i < 12; i++)
            {
                Size scaled = new Size(42, 2);
                VisualPart GDnogging = new VisualPart(scaled,
                    new Dictionary<string, Size>()
                    {
                        { "front", scaled }
                    });
                GDnogging.AddPanel("front_GDnogging", new Point(0, 0), scaled, Color.Tan);
                GDnoggings.Push(GDnogging);
            }
            //4 cornières
            Stack<VisualPart> angles = new Stack<VisualPart>();
            for (int i = 0; i < 4; i++)
            {
                Size scaled = new Size(2, 108);
                VisualPart angle = new VisualPart(scaled,
                    new Dictionary<string, Size>()
                    {
                        { "front", scaled },
                        { "left", scaled}
                    });
                angle.AddPanel("front_angle", new Point(0, 0), scaled, Color.Black);
                angle.AddPanel("left_angle", new Point(0, 0), scaled, Color.Black);
                angles.Push(angle);
            }
            //3 etages
            Stack<VisualPart> boxes = new Stack<VisualPart>();
            for (int i = 0; i < 3; i++)
            {
                int h = 36;
                int l = 120;
                int p = 42;
                Size lh = new Size(l, h);
                Size lp = new Size(l, p);
                Size ph = new Size(p, h);
                VisualPart box = new VisualPart(front.Size,
                    new Dictionary<string, Size>()
                    {
                        { "front", lh },
                        { "left", ph },
                        { "right", ph },
                        { "rear", lh },
                        { "top", lp },
                        { "bottom", lp }
                    });
                //doors
                VisualPart Ldoor = doors.Pop();
                AddVisualPart1v(box, Ldoor, "Ldoor", "front", "front", new Point(0, 2));
                VisualPart Rdoor = doors.Pop();
                AddVisualPart1v(box, Rdoor, "Rdoor", "front", "front", new Point(l - Rdoor.Mm_size.Width, 2));
                //GDnoggings
                VisualPart BLnogging = GDnoggings.Pop();
                AddVisualPart1v(box, BLnogging, "BLnogging", "front", "left", new Point(0, h - BLnogging.Mm_size.Height));
                VisualPart HLnogging = GDnoggings.Pop();
                AddVisualPart1v(box, HLnogging, "HLnogging", "front", "left", new Point(0, 0));
                VisualPart BRnogging = GDnoggings.Pop();
                AddVisualPart1v(box, BRnogging, "BRnogging", "front", "right", new Point(0, h - BRnogging.Mm_size.Height));
                VisualPart HRnogging = GDnoggings.Pop();
                AddVisualPart1v(box, HRnogging, "HRnogging", "front", "right", new Point(0, 0));
                //AVnoggings
                VisualPart BAVnogging = AVnoggings.Pop();
                AddVisualPart1v(box, BAVnogging, "BAVnogging", "front", "front", new Point(0, h - BAVnogging.Mm_size.Height));
                VisualPart HAVnogging = AVnoggings.Pop();
                AddVisualPart1v(box, HAVnogging, "HAVnogging", "front", "front", new Point(0, 0));
                //ARnoggings
                VisualPart BARnogging = ARnoggings.Pop();
                AddVisualPart1v(box, BARnogging, "BARnogging", "front", "rear", new Point(0, h - BARnogging.Mm_size.Height));
                VisualPart HARnogging = ARnoggings.Pop();
                AddVisualPart1v(box, HARnogging, "HARnogging", "front", "rear", new Point(0, 0));
                //HBpanels
                VisualPart Hpanel = HBpanels.Pop();
                AddVisualPart1v(box, Hpanel, "Hpanel", "front", "top", new Point(0, 0));
                VisualPart Bpanel = HBpanels.Pop();
                AddVisualPart1v(box, Bpanel, "Bpanel", "front", "bottom", new Point(0, 0));
                //GDpanels
                VisualPart Gpanel = GDpanels.Pop();
                AddVisualPart1v(box, Gpanel, "Gpanel", "front", "left", new Point(0, 2));
                VisualPart Dpanel = GDpanels.Pop();
                AddVisualPart1v(box, Dpanel, "Dpanel", "front", "right", new Point(0, 2));
                //ARpanels
                VisualPart ARpanel = ARpanels.Pop();
                AddVisualPart1v(box, ARpanel, "ARpanel", "front", "rear", new Point(0, 2));
                boxes.Push(box);
            }
            //1 armoire
            int _h = 3*36;
            int _l = 120;
            int _p = 42;
            Size _lh = new Size(_l, _h);
            Size _lp = new Size(_l, _p);
            Size _ph = new Size(_p, _h);
            VisualPart wardrobe = new VisualPart(front.Size,
                new Dictionary<string, Size>()
                {
                        { "front", _lh },
                        { "left", _ph },
                        { "right", _ph },
                        { "rear", _lh },
                        { "top", _lp },
                        { "bottom", _lp }
                });
            //angles
            VisualPart angle1 = angles.Pop();
            wardrobe.AddVisualPart("angle1", angle1,
                new Dictionary<string, string>()
                {
                    { "front", "front" },
                    { "left", "left" }
                },
                new Dictionary<string, Point>()
                {
                    { "front", new Point(0,0) },
                    { "left", new Point(_p-angle1.Mm_size.Width,0) }
                });
            VisualPart angle2 = angles.Pop();
            wardrobe.AddVisualPart("angle2", angle2,
                new Dictionary<string, string>()
                {
                    { "front", "right" },
                    { "left", "front" }
                },
                new Dictionary<string, Point>()
                {
                    { "front", new Point(0,0) },
                    { "left", new Point(_l-angle2.Mm_size.Width,0) }
                });
            VisualPart angle3 = angles.Pop();
            wardrobe.AddVisualPart("angle3", angle3,
                new Dictionary<string, string>()
                {
                    { "front", "rear" },
                    { "left", "right" }
                },
                new Dictionary<string, Point>()
                {
                    { "front", new Point(0,0) },
                    { "left", new Point(_p-angle3.Mm_size.Width,0) }
                });
            VisualPart angle4 = angles.Pop();
            wardrobe.AddVisualPart("angle4", angle4,
                new Dictionary<string, string>()
                {
                    { "front", "left" },
                    { "left", "rear" }
                },
                new Dictionary<string, Point>()
                {
                    { "front", new Point(0,0) },
                    { "left", new Point(_l-angle4.Mm_size.Width,0) }
                });
            //floors
            VisualPart floor1 = boxes.Pop();
            wardrobe.AddVisualPart("floor1", floor1,
                new Dictionary<string, string>()
                {
                    { "front", "front" },
                    { "left", "left" },
                    { "right", "right" },
                    { "rear", "rear" },
                    { "top", "top" },
                    { "bottom", "bottom" },
                },
                new Dictionary<string, Point>()
                {
                    { "front", new Point(0,0) },
                    { "left", new Point(0,0) },
                    { "right", new Point(0,0) },
                    { "rear", new Point(0,0) },
                    { "top", new Point(0,0) },
                    { "bottom", new Point(0,0) }
                });
            VisualPart floor2 = boxes.Pop();
            wardrobe.AddVisualPart("floor2", floor2,
                new Dictionary<string, string>()
                {
                    { "front", "front" },
                    { "left", "left" },
                    { "right", "right" },
                    { "rear", "rear" },
                    { "top", "top" },
                    { "bottom", "bottom" },
                },
                new Dictionary<string, Point>()
                {
                    { "front", new Point(0,36) },
                    { "left", new Point(0,36) },
                    { "right", new Point(0,36) },
                    { "rear", new Point(0,36) },
                    { "top", new Point(0,0) },
                    { "bottom", new Point(0,0) }
                });
            VisualPart floor3 = boxes.Pop();
            wardrobe.AddVisualPart("floor3", floor3,
                new Dictionary<string, string>()
                {
                    { "front", "front" },
                    { "left", "left" },
                    { "right", "right" },
                    { "rear", "rear" },
                    { "top", "top" },
                    { "bottom", "bottom" },
                },
                new Dictionary<string, Point>()
                {
                    { "front", new Point(0,72) },
                    { "left", new Point(0,72) },
                    { "right", new Point(0,72) },
                    { "rear", new Point(0,72) },
                    { "top", new Point(0,0) },
                    { "bottom", new Point(0,0) }
                });
            

            current = wardrobe;
            //DimensionTester.Panel1.Controls.Add(current.Views["front"]);
            Dictionary<string, VPPanel> views = current.Display();
            front.Controls.Add(views["front"]);
            rear.Controls.Add(views["rear"]);
            top.Controls.Add(views["top"]);
            bottom.Controls.Add(views["bottom"]);
            left.Controls.Add(views["left"]);
            right.Controls.Add(views["right"]);

            
        }

        private void AddVisualPart1v(VisualPart container, VisualPart content, string content_name, string content_view, string container_pos, Point location)
        {
            container.AddVisualPart(content_name, content,
                    new Dictionary<string, string>()
                    {
                        { content_view, container_pos }
                    },
                    new Dictionary<string, Point>()
                    {
                        { content_view, location }
                    });
        }

        private void ChangeScaling(object sender, SplitterEventArgs e)
        {
            current.CleanFocus();
            current.ChangeScaling(DimensionTester.Panel1.Size);
            X.Text = DimensionTester.Panel1.Size.Width.ToString();
            Y.Text = DimensionTester.Panel1.Size.Height.ToString();
            selected.Text = current.Pointer;
        }

        private void Viewer_Click(object sender, EventArgs e)
        {
            current.CleanFocus();
        }
    }
}
