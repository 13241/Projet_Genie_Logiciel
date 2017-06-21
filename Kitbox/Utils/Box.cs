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


        /// <summary>
        /// Initialize a Box with the specified dimensions, the VisualPart is constructed and the pieces are booked in the database
        /// </summary>
        /// <param name="dimensions"></param>
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

        /// <summary>
        /// change the color of the selected part in the box to the color specified in input
        /// </summary>
        /// <param name="color"></param>
        public virtual void ChangeColor(string color)
        {
            string[] positions = Visual_part.Pointer.Split('_');
            string[] selection = positions.Last().Split('*');
            Part requested = DbCatalog.DbSelectPart(new Dictionary<string, string>()
            {
                { "Ref", Pieces[selection[0]][selection[1]].Reference },
                { "largeur", Convert.ToString(Pieces[selection[0]][selection[1]].Dimensions.X) },
                { "hauteur", Convert.ToString(Pieces[selection[0]][selection[1]].Dimensions.Y) },
                { "profondeur", Convert.ToString(Pieces[selection[0]][selection[1]].Dimensions.Z) },
                { "couleur", color }
            });
            Dictionary<string, object> data = new Dictionary<string, object>();
            data["Code"] = requested.Code;
            data["Color"] = requested.Color;
            data["Selling_price"] = requested.Selling_price;
            Pieces[selection[0]][selection[1]].SetData(data);
            Pieces[selection[0]][selection[1]].ConstructVisualPart();
            DefaultBox();
        }

        /// <summary>
        /// calculate the price of the box
        /// </summary>
        /// <returns></returns>
        public double SellingPrice()
        {
            double selling_price = 0;
            foreach(string piece in Pieces.Keys)
            {
                foreach(string position in Pieces[piece].Keys)
                {
                    selling_price += Pieces[piece][position].Selling_price;
                }
            }
            return selling_price;
        }

        /// <summary>
        /// Book the box in the database (every piece)
        /// </summary>
        /// <param name="buy"></param>
        public virtual void Book(bool buy = false)
        {
            foreach (string name in Pieces.Keys)
            {
                foreach (string position in Pieces[name].Keys)
                {
                    if (!buy)
                    {
                        Pieces[name][position].Delayed = DbCatalog.DbBook(Pieces[name][position].Code);
                        if(typeof(Door).IsInstanceOfType(Pieces[name][position]) && ((Door)Pieces[name][position]).Knop != null)
                        {
                            ((Door)Pieces[name][position]).Knop.Delayed = DbCatalog.DbBook(((Door)Pieces[name][position]).Knop.Code);
                        }
                    }
                    else
                    {
                        Pieces[name][position].Delayed = DbCatalog.DbRemoveFromStock(Pieces[name][position].Code);
                        if (typeof(Door).IsInstanceOfType(Pieces[name][position]) && ((Door)Pieces[name][position]).Knop != null)
                        {
                            ((Door)Pieces[name][position]).Knop.Delayed = DbCatalog.DbRemoveFromStock(((Door)Pieces[name][position]).Knop.Code);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// unbook the box in the database (every piece)
        /// </summary>
        public virtual void UnBook()
        {
            foreach (string name in Pieces.Keys)
            {
                foreach (string position in Pieces[name].Keys)
                {
                    DbCatalog.DbUnBook(Pieces[name][position].Code);
                    if (typeof(Door).IsInstanceOfType(Pieces[name][position]) && ((Door)Pieces[name][position]).Knop != null)
                    {
                        DbCatalog.DbUnBook(((Door)Pieces[name][position]).Knop.Code);
                    }
                }
            }
        }

        /// <summary>
        /// call the database to fill in the box with every needed part, place all the parts at the right place
        /// </summary>
        public virtual void DefaultBox()
        {
            //MODIF interaction avec la base de données nécessaire : créer la méthode permettant de sélectionner une part
            //MODIF code temporaire pour éviter de devoir faire la base de données pour l'instant.
            //Panneau Ar
            Dictionary<string, string> spar = new Dictionary<string, string>()
            {
                { "Ref", "Panneau Ar" },
                { "largeur", Convert.ToString(Dimensions.X) },
                { "hauteur", Convert.ToString(Dimensions.Y - 4) }
            };
            if(Visual_part != null)
            {
                spar["Couleur"] = DbCatalog.TraduireCouleur((Pieces["Panneau Ar"]["Ar"]).Color.Name);
            }
            Part par = DbCatalog.DbSelectPart(spar);
            par.Location = new Point3D(0, 2, 0);
            par.Position = "Ar";
            par.ConstructVisualPart();
            Pieces[par.Reference] = new Dictionary<string, Part>()
            {
                { par.Position, par }
            };
            //Panneau G
            Dictionary<string, string> spg = new Dictionary<string, string>()
            {
                { "Ref", "Panneau GD" },
                { "largeur", Convert.ToString(Dimensions.Z) },
                { "hauteur", Convert.ToString(Dimensions.Y - 4) }
            };
            if (Visual_part != null)
            {
                spg["Couleur"] = DbCatalog.TraduireCouleur((Pieces["Panneau GD"]["G"]).Color.Name);
            }
            Part pg = DbCatalog.DbSelectPart(spg);
            pg.Location = new Point3D(0, 2, 0);
            pg.Position = "G";
            pg.ConstructVisualPart();
            //Panneau D
            Dictionary<string, string> spd = new Dictionary<string, string>()
            {
                { "Ref", "Panneau GD" },
                { "largeur", Convert.ToString(Dimensions.Z) },
                { "hauteur", Convert.ToString(Dimensions.Y - 4) }
            };
            if (Visual_part != null)
            {
                spd["Couleur"] = DbCatalog.TraduireCouleur((Pieces["Panneau GD"]["D"]).Color.Name);
            }
            Part pd = DbCatalog.DbSelectPart(spd);
            pd.Location = new Point3D(0, 2, 0);
            pd.Position = "D";
            pd.ConstructVisualPart();
            Pieces[pg.Reference] = new Dictionary<string, Part>()
            {
                {pg.Position, pg },
                {pd.Position, pd }
            };
            //Panneau H
            Dictionary<string, string> sph = new Dictionary<string, string>()
            {
                { "Ref", "Panneau HB" },
                { "largeur", Convert.ToString(Dimensions.X) },
                { "hauteur", Convert.ToString(Dimensions.Z) }
            };
            if (Visual_part != null)
            {
                sph["Couleur"] = DbCatalog.TraduireCouleur((Pieces["Panneau HB"]["H"]).Color.Name);
            }
            Part ph = DbCatalog.DbSelectPart(sph);
            ph.Location = new Point3D(0, 2, 0);
            ph.Position = "H";
            ph.ConstructVisualPart();
            //Panneau B
            Dictionary<string, string> spb = new Dictionary<string, string>()
            {
                { "Ref", "Panneau HB" },
                { "largeur", Convert.ToString(Dimensions.X) },
                { "hauteur", Convert.ToString(Dimensions.Z) }
            };
            if (Visual_part != null)
            {
                spb["Couleur"] = DbCatalog.TraduireCouleur((Pieces["Panneau HB"]["B"]).Color.Name);
            }
            Part pb = DbCatalog.DbSelectPart(spb);
            pb.Location = new Point3D(0, 2, 0);
            pb.Position = "B";
            pb.ConstructVisualPart();
            Pieces[ph.Reference] = new Dictionary<string, Part>()
            {
                {ph.Position, ph },
                {pb.Position, pb }
            };
            //Porte G
            Dictionary<string, string> spog = new Dictionary<string, string>()
            {
                { "Ref", "Porte" },
                { "largeur", Convert.ToString(Math.Ceiling(Dimensions.X / 2 + 2)) },
                { "hauteur", Convert.ToString(Dimensions.Y - 4) }
            };
            if (Visual_part != null)
            {
                spog["Couleur"] = DbCatalog.TraduireCouleur((Pieces["Porte"]["G"]).Color.Name);
            }
            Part pog = DbCatalog.DbSelectPart(spog);
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
            Dictionary<string, string> spod = new Dictionary<string, string>()
            {
                { "Ref", "Porte" },
                { "largeur", Convert.ToString(Math.Floor(Dimensions.X / 2 + 2)) },
                { "hauteur", Convert.ToString(Dimensions.Y - 4) }
            };
            if (Visual_part != null)
            {
                spod["Couleur"] = DbCatalog.TraduireCouleur((Pieces["Porte"]["D"]).Color.Name);
            }
            Part pod = DbCatalog.DbSelectPart(spod);
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
            Dictionary<string, string> starh = new Dictionary<string, string>()
            {
                { "Ref", "Traverse Ar" },
                { "largeur", Convert.ToString(Dimensions.X) },
                { "hauteur", Convert.ToString(2) }
            };
            if (Visual_part != null)
            {
                starh["Couleur"] = DbCatalog.TraduireCouleur((Pieces["Traverse Ar"]["H"]).Color.Name);
            }
            Part tarh = DbCatalog.DbSelectPart(starh);
            tarh.Location = new Point3D(0, 0, 0);
            tarh.Position = "H";
            tarh.ConstructVisualPart();
            //Traverse Ar B
            Dictionary<string, string> starb = new Dictionary<string, string>()
            {
                { "Ref", "Traverse Ar" },
                { "largeur", Convert.ToString(Dimensions.X) },
                { "hauteur", Convert.ToString(2) }
            };
            if (Visual_part != null)
            {
                starb["Couleur"] = DbCatalog.TraduireCouleur((Pieces["Traverse Ar"]["B"]).Color.Name);
            }
            Part tarb = DbCatalog.DbSelectPart(starb);
            tarb.Location = new Point3D(0, Dimensions.Y - 2, 0);
            tarb.Position = "B";
            tarb.ConstructVisualPart();
            Pieces[tarh.Reference] = new Dictionary<string, Part>()
            {
                {tarh.Position, tarh },
                {tarb.Position, tarb }
            };
            //Traverse Av H
            Dictionary<string, string> stavh = new Dictionary<string, string>()
            {
                { "Ref", "Traverse Av" },
                { "largeur", Convert.ToString(Dimensions.X) },
                { "hauteur", Convert.ToString(2) }
            };
            if (Visual_part != null)
            {
                stavh["Couleur"] = DbCatalog.TraduireCouleur((Pieces["Traverse Av"]["H"]).Color.Name);
            }
            Part tavh = DbCatalog.DbSelectPart(stavh);
            tavh.Location = new Point3D(0, 0, 0);
            tavh.Position = "H";
            tavh.ConstructVisualPart();
            //Traverse Av B
            Dictionary<string, string> stavb = new Dictionary<string, string>()
            {
                { "Ref", "Traverse Av" },
                { "largeur", Convert.ToString(Dimensions.X) },
                { "hauteur", Convert.ToString(2) }
            };
            if (Visual_part != null)
            {
                stavb["Couleur"] = DbCatalog.TraduireCouleur((Pieces["Traverse Av"]["B"]).Color.Name);
            }
            Part tavb = DbCatalog.DbSelectPart(stavb);
            tavb.Location = new Point3D(0, Dimensions.Y - 2, 0);
            tavb.Position = "B";
            tavb.ConstructVisualPart();
            Pieces[tavh.Reference] = new Dictionary<string, Part>()
            {
                {tavh.Position, tavh },
                {tavb.Position, tavb }
            };
            //Traverse GD G H
            Dictionary<string, string> stgh = new Dictionary<string, string>()
            {
                { "Ref", "Traverse GD" },
                { "largeur", Convert.ToString(Dimensions.Z) },
                { "hauteur", Convert.ToString(2) }
            };
            if (Visual_part != null)
            {
                stgh["Couleur"] = DbCatalog.TraduireCouleur((Pieces["Traverse GD"]["GH"]).Color.Name);
            }
            Part tgh = DbCatalog.DbSelectPart(stgh);
            tgh.Location = new Point3D(0, 0, 0);
            tgh.Position = "GH";
            tgh.ConstructVisualPart();
            //Traverse GD G B
            Dictionary<string, string> stgb = new Dictionary<string, string>()
            {
                { "Ref", "Traverse GD" },
                { "largeur", Convert.ToString(Dimensions.Z) },
                { "hauteur", Convert.ToString(2) }
            };
            if (Visual_part != null)
            {
                stgb["Couleur"] = DbCatalog.TraduireCouleur((Pieces["Traverse GD"]["GB"]).Color.Name);
            }
            Part tgb = DbCatalog.DbSelectPart(stgb);
            tgb.Location = new Point3D(0, Dimensions.Y - 2, 0);
            tgb.Position = "GB";
            tgb.ConstructVisualPart();
            //Traverse GD D H
            Dictionary<string, string> stdh = new Dictionary<string, string>()
            {
                { "Ref", "Traverse GD" },
                { "largeur", Convert.ToString(Dimensions.Z) },
                { "hauteur", Convert.ToString(2) }
            };
            if (Visual_part != null)
            {
                stdh["Couleur"] = DbCatalog.TraduireCouleur((Pieces["Traverse GD"]["DH"]).Color.Name);
            }
            Part tdh = DbCatalog.DbSelectPart(stdh);
            tdh.Location = new Point3D(0, 0, 0);
            tdh.Position = "DH";
            tdh.ConstructVisualPart();
            //Traverse GD D B
            Dictionary<string, string> stdb = new Dictionary<string, string>()
            {
                { "Ref", "Traverse GD" },
                { "largeur", Convert.ToString(Dimensions.Z) },
                { "hauteur", Convert.ToString(2) }
            };
            if (Visual_part != null)
            {
                stdb["Couleur"] = DbCatalog.TraduireCouleur((Pieces["Traverse GD"]["DB"]).Color.Name);
            }
            Part tdb = DbCatalog.DbSelectPart(stdb);
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
            //Tasseau AvG
            Dictionary<string, string> stavg = new Dictionary<string, string>()
            {
                { "Ref", "Tasseau" },
                { "hauteur", Convert.ToString(Dimensions.Y - 4) }
            };
            Part tavg = DbCatalog.DbSelectPart(stavg);
            tavg.Location = new Point3D(0, Dimensions.Y - 2, 0);
            tavg.Position = "AvG";
            //Tasseau AvD
            Dictionary<string, string> stavd = new Dictionary<string, string>()
            {
                { "Ref", "Tasseau" },
                { "hauteur", Convert.ToString(Dimensions.Y - 4) }
            };
            Part tavd = DbCatalog.DbSelectPart(stavd);
            tavg.Location = new Point3D(Dimensions.X, Dimensions.Y - 2, 0);
            tavg.Position = "AvD";
            //Tasseau ArD
            Dictionary<string, string> stard = new Dictionary<string, string>()
            {
                { "Ref", "Tasseau" },
                { "hauteur", Convert.ToString(Dimensions.Y - 4) }
            };
            Part tard = DbCatalog.DbSelectPart(stard);
            tavg.Location = new Point3D(0, Dimensions.Y - 2, 0);
            tavg.Position = "ArD";
            //Tasseau ArG
            Dictionary<string, string> starg = new Dictionary<string, string>()
            {
                { "Ref", "Tasseau" },
                { "hauteur", Convert.ToString(Dimensions.Y - 4) }
            };
            Part targ = DbCatalog.DbSelectPart(starg);
            tavg.Location = new Point3D(Dimensions.X, Dimensions.Y - 2, 0);
            tavg.Position = "ArD";

            //Box
            ConstructVisualPart();
        }

        /// <summary>
        /// construct the visualpart for the box
        /// </summary>
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
