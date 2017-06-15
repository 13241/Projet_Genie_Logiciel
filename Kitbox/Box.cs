using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Media.Media3D;

namespace Kitbox
{
    public class Box
    {  
        private VisualPart visual_part;
        private Size3D dimensions;
        private Point3D location;
        private Dictionary<string, Dictionary<string, Part>> pieces;
        private string position;


        public Box(Size3D dimensions)
        {
            pieces = new Dictionary<string, Dictionary<string, Part>>();
            this.dimensions = dimensions;
            location = new Point3D(0, 0, 0);

            DefaultBox();
        }

        public VisualPart Visual_part { get => visual_part; }
        public Size3D Dimensions { get => dimensions; }
        public Point3D Location { get => location; set => location = value; }
        public Dictionary<string, Dictionary<string, Part>> Pieces { get => pieces; }
        public string Position { get => position; set => position = value; }

        public virtual void DefaultBox()
        {
            //MODIF interaction avec la base de données nécessaire : créer la méthode permettant de sélectionner une part
            //MODIF code temporaire pour éviter de devoir faire la base de données pour l'instant.
            //Panneau Ar
            Part par = new Panel();
            par.Location = new Point3D(0, 2, 0);
            par.Dimensions = new Size3D(Dimensions.X, Dimensions.Y - 4, 0);
            par.Color = Color.Beige;
            par.Reference = "Panneau Ar";
            par.Position = "Ar";
            par.ConstructVisualPart();
            Pieces[par.Reference] = new Dictionary<string, Part>()
            {
                { par.Position, par }
            };
            //Panneau G
            Part pg = new Panel();
            pg.Location = new Point3D(0, 2, 0);
            pg.Dimensions = new Size3D(Dimensions.Z, Dimensions.Y - 4, 0);
            pg.Color = Color.Beige;
            pg.Reference = "Panneau GD";
            pg.Position = "G";
            pg.ConstructVisualPart();
            //Panneau D
            Part pd = new Panel();
            pd.Location = new Point3D(0, 2, 0);
            pd.Dimensions = new Size3D(Dimensions.Z, Dimensions.Y - 4, 0);
            pd.Color = Color.Beige;
            pd.Reference = "Panneau GD";
            pd.Position = "D";
            pd.ConstructVisualPart();
            Pieces[pg.Reference] = new Dictionary<string, Part>()
            {
                {pg.Position, pg },
                {pd.Position, pd }
            };
            //Panneau H
            Part ph = new Panel();
            ph.Location = new Point3D(0, 2, 0);
            ph.Dimensions = new Size3D(Dimensions.X, Dimensions.Z, 0);
            ph.Color = Color.Beige;
            ph.Reference = "Panneau HB";
            ph.Position = "H";
            ph.ConstructVisualPart();
            //Panneau B
            Part pb = new Panel();
            pb.Location = new Point3D(0, 2, 0);
            pb.Dimensions = new Size3D(Dimensions.X, Dimensions.Z, 0);
            pb.Color = Color.Beige;
            pb.Reference = "Panneau HB";
            pb.Position = "B";
            pb.ConstructVisualPart();
            Pieces[ph.Reference] = new Dictionary<string, Part>()
            {
                {ph.Position, ph },
                {pb.Position, pb }
            };
            //Porte G
            Part pog = new Door();
            pog.Location = new Point3D(0, 2, 0);
            pog.Dimensions = new Size3D(Math.Ceiling(Dimensions.X / 2), Dimensions.Y - 4, 0);
            pog.Color = Color.Beige;
            pog.Reference = "Porte";
            pog.Position = "G";
            //=>Coupelle G
            Part cg = new Knop();
            cg.Location = new Point3D(0, 0, 0);
            cg.Dimensions = new Size3D(6, 6, 0);
            cg.Color = Color.Black;
            cg.Reference = "Coupelles";
            cg.Position = "G";
            cg.ConstructVisualPart();
            ((Door)pog).SetKnop((Knop)cg, 4, "left");
            pog.ConstructVisualPart();
            //Porte D
            Part pod = new Door();
            pod.Location = new Point3D(pog.Dimensions.X, 2, 0);
            pod.Dimensions = new Size3D(Math.Floor(Dimensions.X / 2), Dimensions.Y - 4, 0);
            pod.Color = Color.Beige;
            pod.Reference = "Porte";
            pod.Position = "D";
            //=>Coupelle D
            Part cd = new Knop();
            cd.Location = new Point3D(0, 0, 0);
            cd.Dimensions = new Size3D(6, 6, 0);
            cd.Color = Color.Black;
            cd.Reference = "Coupelles";
            cd.Position = "D";
            cd.ConstructVisualPart();
            ((Door)pod).SetKnop((Knop)cd, 4, "right");
            pod.ConstructVisualPart();
            Pieces[pod.Reference] = new Dictionary<string, Part>()
            {
                {pod.Position, pod },
                {pog.Position, pog }
            };
            //Traverse Ar H
            Part tarh = new Panel();
            tarh.Location = new Point3D(0, 0, 0);
            tarh.Dimensions = new Size3D(Dimensions.X, 2, 0);
            tarh.Color = Color.Tan;
            tarh.Reference = "Traverse Ar";
            tarh.Position = "H";
            tarh.ConstructVisualPart();
            //Traverse Ar B
            Part tarb = new Panel();
            tarb.Location = new Point3D(0, Dimensions.Y - 2, 0);
            tarb.Dimensions = new Size3D(Dimensions.X, 2, 0);
            tarb.Color = Color.Tan;
            tarb.Reference = "Traverse Ar";
            tarb.Position = "B";
            tarb.ConstructVisualPart();
            Pieces[tarh.Reference] = new Dictionary<string, Part>()
            {
                {tarh.Position, tarh },
                {tarb.Position, tarb }
            };
            //Traverse Av H
            Part tavh = new Panel();
            tavh.Location = new Point3D(0, 0, 0);
            tavh.Dimensions = new Size3D(Dimensions.X, 2, 0);
            tavh.Color = Color.Tan;
            tavh.Reference = "Traverse Av";
            tavh.Position = "H";
            tavh.ConstructVisualPart();
            //Traverse Av B
            Part tavb = new Panel();
            tavb.Location = new Point3D(0, Dimensions.Y - 2, 0);
            tavb.Dimensions = new Size3D(Dimensions.X, 2, 0);
            tavb.Color = Color.Tan;
            tavb.Reference = "Traverse Av";
            tavb.Position = "B";
            tavb.ConstructVisualPart();
            Pieces[tavh.Reference] = new Dictionary<string, Part>()
            {
                {tavh.Position, tavh },
                {tavb.Position, tavb }
            };
            //Traverse GD G H
            Part tgh = new Panel();
            tgh.Location = new Point3D(0, 0, 0);
            tgh.Dimensions = new Size3D(Dimensions.Z, 2, 0);
            tgh.Color = Color.Tan;
            tgh.Reference = "Traverse GD";
            tgh.Position = "GH";
            tgh.ConstructVisualPart();
            //Traverse GD G B
            Part tgb = new Panel();
            tgb.Location = new Point3D(0, Dimensions.Y - 2, 0);
            tgb.Dimensions = new Size3D(Dimensions.Z, 2, 0);
            tgb.Color = Color.Tan;
            tgb.Reference = "Traverse GD";
            tgb.Position = "GB";
            tgb.ConstructVisualPart();
            //Traverse GD D H
            Part tdh = new Panel();
            tdh.Location = new Point3D(0, 0, 0);
            tdh.Dimensions = new Size3D(Dimensions.Z, 2, 0);
            tdh.Color = Color.Tan;
            tdh.Reference = "Traverse GD";
            tdh.Position = "DH";
            tdh.ConstructVisualPart();
            //Traverse GD D B
            Part tdb = new Panel();
            tdb.Location = new Point3D(0, Dimensions.Y - 2, 0);
            tdb.Dimensions = new Size3D(Dimensions.Z, 2, 0);
            tdb.Color = Color.Tan;
            tdb.Reference = "Traverse GD";
            tdb.Position = "DB";
            tdb.ConstructVisualPart();
            Pieces[tgh.Reference] = new Dictionary<string, Part>()
            {
                {tgh.Position, tgh },
                {tgb.Position, tgb },
                {tdh.Position, tdh },
                {tdb.Position, tdb }
            };

            //Box
            ConstructVisualPart();
        }

        /*
         * VisualPart section
         */
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
            //Panneau Ar
            visual_part.AddVisualPart("Panneau Ar*Ar", Pieces["Panneau Ar"]["Ar"].Visual_part,
                new Dictionary<string, string>()
                {
                    { "front", "rear" }
                },
                new Dictionary<string, Point>()
                {
                    { "front", new Point(Convert.ToInt32(Pieces["Panneau Ar"]["Ar"].Location.X), Convert.ToInt32(Pieces["Panneau Ar"]["Ar"].Location.Y)) }
                });
            //Panneau G
            visual_part.AddVisualPart("Panneau GD*G", Pieces["Panneau GD"]["G"].Visual_part,
                new Dictionary<string, string>()
                {
                    { "front", "left" }
                },
                new Dictionary<string, Point>()
                {
                    { "front", new Point(Convert.ToInt32(Pieces["Panneau GD"]["G"].Location.X), Convert.ToInt32(Pieces["Panneau GD"]["G"].Location.Y)) }
                });
            //Panneau D
            visual_part.AddVisualPart("Panneau GD*D", Pieces["Panneau GD"]["D"].Visual_part,
                new Dictionary<string, string>()
                {
                    { "front", "right" }
                },
                new Dictionary<string, Point>()
                {
                    { "front", new Point(Convert.ToInt32(Pieces["Panneau GD"]["D"].Location.X), Convert.ToInt32(Pieces["Panneau GD"]["D"].Location.Y)) }
                });
            //Panneau H
            visual_part.AddVisualPart("Panneau HB*H", Pieces["Panneau HB"]["H"].Visual_part,
                new Dictionary<string, string>()
                {
                    { "front", "top" }
                },
                new Dictionary<string, Point>()
                {
                    { "front", new Point(Convert.ToInt32(Pieces["Panneau HB"]["H"].Location.X), Convert.ToInt32(Pieces["Panneau HB"]["H"].Location.Y)) }
                });
            //Panneau B
            visual_part.AddVisualPart("Panneau HB*B", Pieces["Panneau HB"]["B"].Visual_part,
                new Dictionary<string, string>()
                {
                    { "front", "bottom" }
                },
                new Dictionary<string, Point>()
                {
                    { "front", new Point(Convert.ToInt32(Pieces["Panneau HB"]["B"].Location.X), Convert.ToInt32(Pieces["Panneau HB"]["B"].Location.Y)) }
                });
            //Porte G
            visual_part.AddVisualPart("Porte*G", Pieces["Porte"]["G"].Visual_part,
                new Dictionary<string, string>()
                {
                    { "front", "front" }
                },
                new Dictionary<string, Point>()
                {
                    { "front", new Point(Convert.ToInt32(Pieces["Porte"]["G"].Location.X), Convert.ToInt32(Pieces["Porte"]["G"].Location.Y)) }
                });
            //Porte D
            visual_part.AddVisualPart("Porte*D", Pieces["Porte"]["D"].Visual_part,
                new Dictionary<string, string>()
                {
                    { "front", "front" }
                },
                new Dictionary<string, Point>()
                {
                    { "front", new Point(Convert.ToInt32(Pieces["Porte"]["D"].Location.X), Convert.ToInt32(Pieces["Porte"]["D"].Location.Y)) }
                });
            //Traverse Ar H
            visual_part.AddVisualPart("Traverse Ar*H", Pieces["Traverse Ar"]["H"].Visual_part,
                new Dictionary<string, string>()
                {
                    { "front", "rear" }
                },
                new Dictionary<string, Point>()
                {
                    { "front", new Point(Convert.ToInt32(Pieces["Traverse Ar"]["H"].Location.X), Convert.ToInt32(Pieces["Traverse Ar"]["H"].Location.Y)) }
                });
            //Traverse Ar B
            visual_part.AddVisualPart("Traverse Ar*B", Pieces["Traverse Ar"]["B"].Visual_part,
                new Dictionary<string, string>()
                {
                    { "front", "rear" }
                },
                new Dictionary<string, Point>()
                {
                    { "front", new Point(Convert.ToInt32(Pieces["Traverse Ar"]["B"].Location.X), Convert.ToInt32(Pieces["Traverse Ar"]["B"].Location.Y)) }
                });
            //Traverse Av H
            visual_part.AddVisualPart("Traverse Av*H", Pieces["Traverse Av"]["H"].Visual_part,
                new Dictionary<string, string>()
                {
                    { "front", "front" }
                },
                new Dictionary<string, Point>()
                {
                    { "front", new Point(Convert.ToInt32(Pieces["Traverse Av"]["H"].Location.X), Convert.ToInt32(Pieces["Traverse Av"]["H"].Location.Y)) }
                });
            //Traverse Av B
            visual_part.AddVisualPart("Traverse Av*B", Pieces["Traverse Av"]["B"].Visual_part,
                new Dictionary<string, string>()
                {
                    { "front", "front" }
                },
                new Dictionary<string, Point>()
                {
                    { "front", new Point(Convert.ToInt32(Pieces["Traverse Av"]["B"].Location.X), Convert.ToInt32(Pieces["Traverse Av"]["B"].Location.Y)) }
                });
            //Traverse GD GH
            visual_part.AddVisualPart("Traverse GD*GH", Pieces["Traverse GD"]["GH"].Visual_part,
                new Dictionary<string, string>()
                {
                    { "front", "left" }
                },
                new Dictionary<string, Point>()
                {
                    { "front", new Point(Convert.ToInt32(Pieces["Traverse GD"]["GH"].Location.X), Convert.ToInt32(Pieces["Traverse GD"]["GH"].Location.Y)) }
                });
            //Traverse GD GB
            visual_part.AddVisualPart("Traverse GD*GB", Pieces["Traverse GD"]["GB"].Visual_part,
                new Dictionary<string, string>()
                {
                    { "front", "left" }
                },
                new Dictionary<string, Point>()
                {
                    { "front", new Point(Convert.ToInt32(Pieces["Traverse GD"]["GB"].Location.X), Convert.ToInt32(Pieces["Traverse GD"]["GB"].Location.Y)) }
                });
            //Traverse GD DH
            visual_part.AddVisualPart("Traverse GD*DH", Pieces["Traverse GD"]["DH"].Visual_part,
                new Dictionary<string, string>()
                {
                    { "front", "right" }
                },
                new Dictionary<string, Point>()
                {
                    { "front", new Point(Convert.ToInt32(Pieces["Traverse GD"]["DH"].Location.X), Convert.ToInt32(Pieces["Traverse GD"]["DH"].Location.Y)) }
                });
            //Traverse GD DB
            visual_part.AddVisualPart("Traverse GD*DB", Pieces["Traverse GD"]["DB"].Visual_part,
                new Dictionary<string, string>()
                {
                    { "front", "right" }
                },
                new Dictionary<string, Point>()
                {
                    { "front", new Point(Convert.ToInt32(Pieces["Traverse GD"]["DB"].Location.X), Convert.ToInt32(Pieces["Traverse GD"]["DB"].Location.Y)) }
                });
        }
    }
}
