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
    public partial class Tests : Form
    {
        VisualPart part;
        Wardrobe wardrobe = null;
        Box box = null;
        Stack<VisualPart> parts;
        string view;
        public Tests()
        {
            InitializeComponent();
            parts = new Stack<VisualPart>();
        }
        //TestVisualPart
        public void TestVisualPart(string view)
        {
            Invisible();
            part.CleanFocus();
            part.ChangeScaling(screen.Size);
            screen.Controls.Add(part.Display()[view]);
            screen.Visible = true;
            if (wardrobe == null)
            {
                AddBox.Visible = false;
                ChangeSurface.Visible = false;
                RemoveBox.Visible = false;
                ResizeBox.Visible = false;
            }
            if (box == null)
            {
                ChangeColor.Visible = false;
            }
        }
        //TestKnop
        public Knop TestKnop(int x, int y, Color color, string reference = "knop")
        {
            Knop myknop = new Knop();
            myknop.Location = new Point3D(0, 0, 0);
            myknop.Dimensions = new Size3D(x, y, 0);
            myknop.Color = color;
            myknop.Reference = reference;
            myknop.ConstructVisualPart();
            return myknop;
        }
        //TestDoor
        public Door TestDoor(int x, int y, Color color, string reference = "knop")
        {
            Door mydoor = new Door();
            mydoor.Location = new Point3D(0, 0, 0);
            mydoor.Dimensions = new Size3D(x, y, 0);
            mydoor.Color = color;
            mydoor.SetKnop(TestKnop(6, 6, Color.Black), 4);
            mydoor.Reference = "door";
            mydoor.ConstructVisualPart();
            return mydoor;
        }
        //TestNogging
        public Panel TestPanel(int x, int y, Color color, string reference = "panel")
        {
            Panel mypanel = new Panel();
            mypanel.Location = new Point3D(0, 0, 0);
            mypanel.Dimensions = new Size3D(x, y , 0);
            mypanel.Color = color;
            mypanel.Reference = "panel";
            mypanel.ConstructVisualPart();
            return mypanel;
        }
        //TestAngle
        public Angle TestAngle(int x, int y, Color color, string reference = "angle")
        {
            Angle myangle = new Angle();
            myangle.Location = new Point3D(0, 0, 0);
            myangle.Dimensions = new Size3D(x, y, 0);
            myangle.Color = color;
            myangle.Reference = reference;
            myangle.ConstructVisualPart();
            return myangle;
        }
        //TestBox
        public Box TestBox(int l, int h, int p, string reference = "box")
        {
            Box mybox = new Box(new Size3D(l, h, p));
            return mybox;
        }
        //TestWardrobe
        public Wardrobe TestWardrobe(int l, int h, int p, string reference = "wardrobe")
        {
            Wardrobe mywardrobe = new Wardrobe(new Size3D(l, h, p));
            return mywardrobe;
        }

        private void compute_Click(object sender, EventArgs e)
        {
            wardrobe = null;
            switch (test_input.Text)
            {
                case "vp_door"://VisualPart_Door
                    part = TestDoor(62, 32, Color.Beige).Visual_part;
                    break;
                case "vp_knop"://VisualPart_Knop
                    part = TestKnop(6, 6, Color.Black).Visual_part;
                    break;
                case "vp_panel"://VisualPart_Panel
                    part = TestPanel(120, 2, Color.Tan).Visual_part;
                    break;
                case "vp_angle"://VisualPart_Angle
                    part = TestAngle(2, 108, Color.Black).Visual_part;
                    break;
                case "vp_box"://VisualPart_Box
                    box = TestBox(120, 36, 42);
                    part = box.Visual_part;
                    ChangeColor.Visible = true;
                    break;
                case "vp_wardrobe"://VisualPart_Wardrobe
                    wardrobe = TestWardrobe(120, 36, 42);
                    part = wardrobe.Visual_part;
                    AddBox.Visible = true;
                    ChangeSurface.Visible = true;
                    RemoveBox.Visible = true;
                    ResizeBox.Visible = true;
                    break;
            }
            try
            {
                view = "front";
                TestVisualPart(view);
            }
            catch { return; }
        }

        private void Invisible()
        {
            screen.Controls.Clear();
            screen.Visible = false;
        }

        private void UP_Click(object sender, EventArgs e)
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
            TestVisualPart(view);
        }

        private void LEFT_Click(object sender, EventArgs e)
        {
            switch(view)
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
            TestVisualPart(view);
        }

        private void RIGHT_Click(object sender, EventArgs e)
        {
            switch(view)
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
            TestVisualPart(view);
        }

        private void DOWN_Click(object sender, EventArgs e)
        {
            switch(view)
            {
                case "front":
                    view = "bottom";
                    break;
                case "top":
                    view = "front";
                    break;
            }
            TestVisualPart(view);
        }

        private void ZOOM_Click(object sender, EventArgs e)
        {
            string piece = part.ConvertToPosition(part.Pointer).Last();
            if (part.Pieces.Keys.Contains(piece))
            {
                parts.Push(part);
                part = part.Pieces[piece].Item1;
                view = "front";
                TestVisualPart(view);
            }
        }

        private void UNZOOM_Click(object sender, EventArgs e)
        {
            if (parts.Count() > 0)
            {
                VisualPart container = parts.Pop();
                string piece = container.ConvertToPosition(container.Pointer).Last();
                container.ReinsertPiece(piece);
                part = container;
                view = "front";
                TestVisualPart(view);
            }
        }

        private void AddBox_Click(object sender, EventArgs e)
        {
            wardrobe.AddBox(36);
            part = wardrobe.Visual_part;
            view = "front";
            TestVisualPart(view);
        }

        private void ChangeSurface_Click(object sender, EventArgs e)
        {
            wardrobe.ChangeSurface(80, 52);
            part = wardrobe.Visual_part;
            view = "front";
            TestVisualPart(view);
        }

        private void RemoveBox_Click(object sender, EventArgs e)
        {
            string position = wardrobe.Visual_part.Pointer.Split('*').Last();
            wardrobe.RemoveBox(position);
            part = wardrobe.Visual_part;
            view = "front";
            TestVisualPart(view);
        }

        private void ResizeBox_Click(object sender, EventArgs e)
        {
            string position = wardrobe.Visual_part.Pointer.Split('*').Last();
            wardrobe.ResizeBox(position, 56);
            part = wardrobe.Visual_part;
            view = "front";
            TestVisualPart(view);
        }

        private void ChangeColor_Click(object sender, EventArgs e)
        {
            string[] commands = box.Visual_part.Pointer.Split('*');
            box.ChangeColor("Brun", commands.First().Split('_').Last(), commands.Last());
            part = box.Visual_part;
            view = "front";
            TestVisualPart(view);
        }
    }
}
