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
        public Size3D Dimensions { get => dimensions; set => dimensions = value; }
        public Point3D Location { get => location; set => location = value; }
        public Dictionary<string, Dictionary<string, Part>> Pieces { get => pieces; }
        public string Position { get => position; set => position = value; }

        public virtual void DefaultBox()
        {
            //MODIF interaction avec la base de données nécessaire : créer la méthode permettant de sélectionner une part
            //MODIF code temporaire pour éviter de devoir faire la base de données pour l'instant.
            //Panneau Ar
            Part par = DbCatalog.DbSelectPart(new Dictionary<string, string>()
            {
                { "Ref", "Panneau Ar" },
                { "largeur", Convert.ToString(Dimensions.X) },
                { "hauteur", Convert.ToString(Dimensions.Y - 4) }
            });
            par.Location = new Point3D(0, 2, 0);
            par.Position = "Ar";
            par.ConstructVisualPart();
            Pieces[par.Reference] = new Dictionary<string, Part>()
            {
                { par.Position, par }
            };
            //Panneau G
            Part pg = DbCatalog.DbSelectPart(new Dictionary<string, string>()
            {
                { "Ref", "Panneau GD" },
                { "largeur", Convert.ToString(Dimensions.Z) },
                { "hauteur", Convert.ToString(Dimensions.Y - 4) }
            });
            pg.Location = new Point3D(0, 2, 0);
            pg.Position = "G";
            pg.ConstructVisualPart();
            //Panneau D
            Part pd = DbCatalog.DbSelectPart(new Dictionary<string, string>()
            {
                { "Ref", "Panneau GD" },
                { "largeur", Convert.ToString(Dimensions.Z) },
                { "hauteur", Convert.ToString(Dimensions.Y - 4) }
            });
            pd.Location = new Point3D(0, 2, 0);
            pd.Position = "D";
            pd.ConstructVisualPart();
            Pieces[pg.Reference] = new Dictionary<string, Part>()
            {
                {pg.Position, pg },
                {pd.Position, pd }
            };
            //Panneau H
            Part ph = DbCatalog.DbSelectPart(new Dictionary<string, string>()
            {
                { "Ref", "Panneau HB" },
                { "largeur", Convert.ToString(Dimensions.X) },
                { "hauteur", Convert.ToString(Dimensions.Z) }
            });
            ph.Location = new Point3D(0, 2, 0);
            ph.Position = "H";
            ph.ConstructVisualPart();
            //Panneau B
            Part pb = DbCatalog.DbSelectPart(new Dictionary<string, string>()
            {
                { "Ref", "Panneau HB" },
                { "largeur", Convert.ToString(Dimensions.X) },
                { "hauteur", Convert.ToString(Dimensions.Z) }
            });
            pb.Location = new Point3D(0, 2, 0);
            pb.Position = "B";
            pb.ConstructVisualPart();
            Pieces[ph.Reference] = new Dictionary<string, Part>()
            {
                {ph.Position, ph },
                {pb.Position, pb }
            };
            //Porte G
            Part pog = DbCatalog.DbSelectPart(new Dictionary<string, string>()
            {
                { "Ref", "Porte" },
                { "largeur", Convert.ToString(Math.Ceiling(Dimensions.X / 2 + 2)) },
                { "hauteur", Convert.ToString(Dimensions.Y - 4) }
            });
            pog.Location = new Point3D(0, 2, 0);
            pog.Position = "G";
            //=>Coupelle G
            Part cg = DbCatalog.DbSelectPart(new Dictionary<string, string>()
            {
                { "Ref", "Coupelles" }
            });
            cg.Location = new Point3D(0, 0, 0);
            cg.Position = "G";
            cg.ConstructVisualPart();
            ((Door)pog).SetKnop((Knop)cg, 4, "left");
            pog.ConstructVisualPart();
            //Porte D
            Part pod = DbCatalog.DbSelectPart(new Dictionary<string, string>()
            {
                { "Ref", "Porte" },
                { "largeur", Convert.ToString(Math.Floor(Dimensions.X / 2 + 2)) },
                { "hauteur", Convert.ToString(Dimensions.Y - 4) }
            });
            pod.Location = new Point3D(pog.Dimensions.X - 4, 2, 0);
            pod.Position = "D";
            //=>Coupelle D
            Part cd = DbCatalog.DbSelectPart(new Dictionary<string, string>()
            {
                { "Ref", "Coupelles" }
            });
            cd.Location = new Point3D(0, 0, 0);
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
            Part tarh = DbCatalog.DbSelectPart(new Dictionary<string, string>()
            {
                { "Ref", "Traverse Ar" },
                { "largeur", Convert.ToString(Dimensions.X) },
                { "hauteur", Convert.ToString(2) }
            });
            tarh.Location = new Point3D(0, 0, 0);
            tarh.Position = "H";
            tarh.ConstructVisualPart();
            //Traverse Ar B
            Part tarb = DbCatalog.DbSelectPart(new Dictionary<string, string>()
            {
                { "Ref", "Traverse Ar" },
                { "largeur", Convert.ToString(Dimensions.X) },
                { "hauteur", Convert.ToString(2) }
            });
            tarb.Location = new Point3D(0, Dimensions.Y - 2, 0);
            tarb.Position = "B";
            tarb.ConstructVisualPart();
            Pieces[tarh.Reference] = new Dictionary<string, Part>()
            {
                {tarh.Position, tarh },
                {tarb.Position, tarb }
            };
            //Traverse Av H
            Part tavh = DbCatalog.DbSelectPart(new Dictionary<string, string>()
            {
                { "Ref", "Traverse Av" },
                { "largeur", Convert.ToString(Dimensions.X) },
                { "hauteur", Convert.ToString(2) }
            });
            tavh.Location = new Point3D(0, 0, 0);
            tavh.Position = "H";
            tavh.ConstructVisualPart();
            //Traverse Av B
            Part tavb = DbCatalog.DbSelectPart(new Dictionary<string, string>()
            {
                { "Ref", "Traverse Av" },
                { "largeur", Convert.ToString(Dimensions.X) },
                { "hauteur", Convert.ToString(2) }
            });
            tavb.Location = new Point3D(0, Dimensions.Y - 2, 0);
            tavb.Position = "B";
            tavb.ConstructVisualPart();
            Pieces[tavh.Reference] = new Dictionary<string, Part>()
            {
                {tavh.Position, tavh },
                {tavb.Position, tavb }
            };
            //Traverse GD G H
            Part tgh = DbCatalog.DbSelectPart(new Dictionary<string, string>()
            {
                { "Ref", "Traverse GD" },
                { "largeur", Convert.ToString(Dimensions.Z) },
                { "hauteur", Convert.ToString(2) }
            });
            tgh.Location = new Point3D(0, 0, 0);
            tgh.Position = "GH";
            tgh.ConstructVisualPart();
            //Traverse GD G B
            Part tgb = DbCatalog.DbSelectPart(new Dictionary<string, string>()
            {
                { "Ref", "Traverse GD" },
                { "largeur", Convert.ToString(Dimensions.Z) },
                { "hauteur", Convert.ToString(2) }
            });
            tgb.Location = new Point3D(0, Dimensions.Y - 2, 0);
            tgb.Position = "GB";
            tgb.ConstructVisualPart();
            //Traverse GD D H
            Part tdh = DbCatalog.DbSelectPart(new Dictionary<string, string>()
            {
                { "Ref", "Traverse GD" },
                { "largeur", Convert.ToString(Dimensions.Z) },
                { "hauteur", Convert.ToString(2) }
            });
            tdh.Location = new Point3D(0, 0, 0);
            tdh.Position = "DH";
            tdh.ConstructVisualPart();
            //Traverse GD D B
            Part tdb = DbCatalog.DbSelectPart(new Dictionary<string, string>()
            {
                { "Ref", "Traverse GD" },
                { "largeur", Convert.ToString(Dimensions.Z) },
                { "hauteur", Convert.ToString(2) }
            });
            tdb.Location = new Point3D(0, Dimensions.Y - 2, 0);
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
