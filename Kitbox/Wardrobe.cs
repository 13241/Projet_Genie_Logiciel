using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Media.Media3D;

namespace Kitbox
{
	public class Wardrobe
	{
        private VisualPart visual_part;
        private Size3D dimensions;
        private Point3D location;
        private Dictionary<string, Dictionary<string, object>> components;

        public Wardrobe(Size3D dimensions)
        {
            components = new Dictionary<string, Dictionary<string, object>> ();
            this.dimensions = dimensions;
            location = new Point3D(0, 0, 0);

            DefaultWardrobe();
        }

        public VisualPart Visual_part { get => visual_part; set => visual_part = value; }
        public Size3D Dimensions { get => dimensions; set => dimensions = value; }
        public Point3D Location { get => location; set => location = value; }
        public Dictionary<string, Dictionary<string, object>> Components { get => components; }

        public virtual void DefaultWardrobe()
        {
            //Corniere AvG
            Part cag = new Angle();
            cag.Location = new Point3D(0, 0, Dimensions.Z-2);
            cag.Dimensions = new Size3D(2, Dimensions.Y, 2);
            cag.Color = Color.Black;
            cag.Reference = "Cornieres";
            cag.Position = "AvG";
            cag.ConstructVisualPart();
            //Corniere AvD
            Part cad = new Angle();
            cad.Location = new Point3D(Dimensions.X - 2, 0, 0);
            cad.Dimensions = new Size3D(2, Dimensions.Y, 2);
            cad.Color = Color.Black;
            cad.Reference = "Cornieres";
            cad.Position = "AvD";
            cad.ConstructVisualPart();
            //Corniere ArG
            Part crg = new Angle();
            crg.Location = new Point3D(Dimensions.X - 2, 0, 0);
            crg.Dimensions = new Size3D(2, Dimensions.Y, 2);
            crg.Color = Color.Black;
            crg.Reference = "Cornieres";
            crg.Position = "ArG";
            crg.ConstructVisualPart();
            //Corniere ArD
            Part crd = new Angle();
            crd.Location = new Point3D(0, 0, Dimensions.Z - 2);
            crd.Dimensions = new Size3D(2, Dimensions.Y, 2);
            crd.Color = Color.Black;
            crd.Reference = "Cornieres";
            crd.Position = "ArD";
            crd.ConstructVisualPart();
            Components[crd.Reference] = new Dictionary<string, object>()
            {
                { crd.Position, crd },
                { crg.Position, crg },
                { cad.Position, cad },
                { cag.Position, cag }
            };
            //Box
            Box box = new Box(dimensions);
            box.Location = new Point3D(0, 0, 0);
            box.Position = "1";
            box.DefaultBox();
            Components["Etage"] = new Dictionary<string, object>()
            {
                { box.Position, box }
            };

            ConstructVisualPart();
        }

        public virtual void ConstructVisualPart()
        {
            double larger_X = Math.Max(Dimensions.X, Dimensions.Z);
            double larger_Y = Math.Max(Dimensions.Y, Dimensions.Z);
            Size px_size = new Size(Convert.ToInt32(larger_X), Convert.ToInt32(larger_Y));
            visual_part = new VisualPart(px_size,
                new Dictionary<string, Size>()
                {
                    { "front", new Size(Convert.ToInt32(Dimensions.X), Convert.ToInt32(Dimensions.Y)) },
                    { "left", new Size(Convert.ToInt32(Dimensions.Z), Convert.ToInt32(Dimensions.Y)) },
                    { "right", new Size(Convert.ToInt32(Dimensions.Z), Convert.ToInt32(Dimensions.Y)) },
                    { "rear", new Size(Convert.ToInt32(Dimensions.X), Convert.ToInt32(Dimensions.Y)) },
                    { "top", new Size(Convert.ToInt32(Dimensions.X), Convert.ToInt32(Dimensions.Z)) },
                    { "bottom", new Size(Convert.ToInt32(Dimensions.X), Convert.ToInt32(Dimensions.Z)) }
                });
            //Corniere AvG
            visual_part.AddVisualPart("Cornieres*AvG", ((Part)Components["Cornieres"]["AvG"]).Visual_part,
                new Dictionary<string, string>()
                {
                    { "front", "front" },
                    { "left", "left" }
                },
                new Dictionary<string, Point>()
                {
                    { "front", new Point(Convert.ToInt32(((Part)Components["Cornieres"]["AvG"]).Location.X), Convert.ToInt32(((Part)Components["Cornieres"]["AvG"]).Location.Y)) },
                    { "left", new Point(Convert.ToInt32(((Part)Components["Cornieres"]["AvG"]).Location.Z), Convert.ToInt32(((Part)Components["Cornieres"]["AvG"]).Location.Y)) }
                });
            //Corniere AvD
            visual_part.AddVisualPart("Cornieres*AvD", ((Part)Components["Cornieres"]["AvD"]).Visual_part,
                new Dictionary<string, string>()
                {
                    { "front", "right" },
                    { "left", "front" }
                },
                new Dictionary<string, Point>()
                {
                    { "front", new Point(Convert.ToInt32(((Part)Components["Cornieres"]["AvD"]).Location.Z), Convert.ToInt32(((Part)Components["Cornieres"]["AvD"]).Location.Y)) },
                    { "left", new Point(Convert.ToInt32(((Part)Components["Cornieres"]["AvD"]).Location.X), Convert.ToInt32(((Part)Components["Cornieres"]["AvD"]).Location.Y)) }
                });
            //Corniere ArD
            visual_part.AddVisualPart("Cornieres*ArD", ((Part)Components["Cornieres"]["ArD"]).Visual_part,
                new Dictionary<string, string>()
                {
                    { "front", "rear" },
                    { "left", "right" }
                },
                new Dictionary<string, Point>()
                {
                    { "front", new Point(Convert.ToInt32(((Part)Components["Cornieres"]["ArD"]).Location.X), Convert.ToInt32(((Part)Components["Cornieres"]["ArD"]).Location.Y)) },
                    { "left", new Point(Convert.ToInt32(((Part)Components["Cornieres"]["ArD"]).Location.Z), Convert.ToInt32(((Part)Components["Cornieres"]["ArD"]).Location.Y)) }
                });
            //Corniere ArG
            visual_part.AddVisualPart("Cornieres*ArG", ((Part)Components["Cornieres"]["ArG"]).Visual_part,
                new Dictionary<string, string>()
                {
                    { "front", "left" },
                    { "left", "rear" }
                },
                new Dictionary<string, Point>()
                {
                    { "front", new Point(Convert.ToInt32(((Part)Components["Cornieres"]["ArG"]).Location.Z), Convert.ToInt32(((Part)Components["Cornieres"]["ArG"]).Location.Y)) },
                    { "left", new Point(Convert.ToInt32(((Part)Components["Cornieres"]["ArG"]).Location.X), Convert.ToInt32(((Part)Components["Cornieres"]["ArG"]).Location.Y)) }
                });
            //Box
            visual_part.AddVisualPart("Etage*1", ((Box)Components["Etage"]["1"]).Visual_part,
                new Dictionary<string, string>()
                {
                    { "front", "front" },
                    { "left", "left" },
                    { "right", "right" },
                    { "rear", "rear" },
                    { "top", "top" },
                    { "bottom", "bottom" }
                },
                new Dictionary<string, Point>()
                {
                    { "front", new Point(Convert.ToInt32(((Box)Components["Etage"]["1"]).Location.X), Convert.ToInt32(((Box)Components["Etage"]["1"]).Location.Y)) },
                    { "left", new Point(Convert.ToInt32(((Box)Components["Etage"]["1"]).Location.Z), Convert.ToInt32(((Box)Components["Etage"]["1"]).Location.Y)) },
                    { "right", new Point(Convert.ToInt32(((Box)Components["Etage"]["1"]).Location.Z), Convert.ToInt32(((Box)Components["Etage"]["1"]).Location.Y)) },
                    { "rear", new Point(Convert.ToInt32(((Box)Components["Etage"]["1"]).Location.X), Convert.ToInt32(((Box)Components["Etage"]["1"]).Location.Y)) },
                    { "top", new Point(Convert.ToInt32(((Box)Components["Etage"]["1"]).Location.X), Convert.ToInt32(((Box)Components["Etage"]["1"]).Location.Z)) },
                    { "bottom", new Point(Convert.ToInt32(((Box)Components["Etage"]["1"]).Location.X), Convert.ToInt32(((Box)Components["Etage"]["1"]).Location.Z)) }
                });
        }
    }
}
