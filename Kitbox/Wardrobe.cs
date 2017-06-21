using System;
using System.Collections.Generic;
using System.Linq;
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

        /// <summary>
        /// Initialize a Wardrobe
        /// </summary>
        /// <param name="dimensions"></param>
        public Wardrobe(Size3D dimensions)
        {
            components = new Dictionary<string, Dictionary<string, object>> ();
            this.dimensions = dimensions;
            location = new Point3D(0, 0, 0);

            DefaultWardrobe();
            Book();
        }

        public VisualPart Visual_part { get => visual_part; set => visual_part = value; }
        public Size3D Dimensions { get => dimensions; set => dimensions = value; }
        public Point3D Location { get => location; set => location = value; }
        public Dictionary<string, Dictionary<string, object>> Components { get => components; }

        /// <summary>
        /// change the color of the selected part in the visualpart of the wardrobe to the color 
        /// </summary>
        /// <param name="color"></param>
        public virtual void ChangeColor(string color)
        {
            UnBook();
            string[] positions = Visual_part.Pointer.Split('_');
            string[] selection = positions.Last().Split('*');
            if (typeof(Box).IsInstanceOfType(Components[selection[0]][selection[1]]))
            {
                ((Box)Components[selection[0]][selection[1]]).ChangeColor(color);
            }
            else
            {
                Part requested = DbCatalog.DbSelectPart(new Dictionary<string, string>()
                {
                    { "Ref", ((Part)Components[selection[0]][selection[1]]).Reference },
                    { "hauteur>", Convert.ToString(((Part)Components[selection[0]][selection[1]]).Dimensions.Y) },
                    { "couleur", color }
                }, "hauteur ASC");
                Dictionary<string, object> data = new Dictionary<string, object>();
                data["Code"] = requested.Code;
                data["Color"] = requested.Color;
                data["Selling_price"] = requested.Selling_price;
                ((Part)Components[selection[0]][selection[1]]).SetData(data);
                ((Angle)Components[selection[0]][selection[1]]).Cut_height = requested.Dimensions.Y - Dimensions.Y;
                ((Part)Components[selection[0]][selection[1]]).ConstructVisualPart();
            }
            DefaultWardrobe();
            Book();
        }

        /// <summary>
        /// compute the selling price of the wardrobe 
        /// </summary>
        /// <returns></returns>
        public double SellingPrice()
        {
            double selling_price = 0;
            foreach (string piece in Components.Keys)
            {
                foreach (string position in Components[piece].Keys)
                {
                    if(typeof(Box).IsInstanceOfType(Components[piece][position]))
                    {
                        selling_price += ((Box)Components[piece][position]).SellingPrice();
                    }
                    else
                    {
                        selling_price += ((Part)Components[piece][position]).Selling_price;
                    }
                }
            }
            return selling_price;
        }

        /// <summary>
        /// modify the X and Z dimensions of the wardrobe
        /// </summary>
        /// <param name="w"></param>
        /// <param name="d"></param>
        public virtual void ChangeSurface(double w, double d)
        {
            UnBook();
            dimensions.X = w;
            dimensions.Z = d;
            for(int i = 1; i <= Components["Etage"].Count; i++)
            {
                ((Box)Components["Etage"][Convert.ToString(i)]).Dimensions = new Size3D(Dimensions.X, ((Box)Components["Etage"][Convert.ToString(i)]).Dimensions.Y, Dimensions.Z);
                ((Box)Components["Etage"][Convert.ToString(i)]).DefaultBox();
            }
            DefaultWardrobe();
            Book();
        }

        public virtual void RemoveBox(string position)
        {
            UnBook();
            if (Components["Etage"].ContainsKey(position) && components["Etage"].Count > 1)
            {
                int count = Components["Etage"].Count;
                Components["Etage"].Remove(position);
                for (int i = Convert.ToInt32(position)+1; i<=count; i++)
                {
                    Components["Etage"][Convert.ToString(i - 1)] = Components["Etage"][Convert.ToString(i)];
                    ((Box)Components["Etage"][Convert.ToString(i - 1)]).Position = Convert.ToString(i - 1);
                }
                Components["Etage"].Remove(Convert.ToString(count));
                AdjustHeight(Convert.ToString(Convert.ToInt32(position) - 1));
                DefaultWardrobe();
            }
            Book();
        }

        /// <summary>
        /// modify the Y dimension of a box in the wardrobe
        /// </summary>
        /// <param name="position"></param>
        /// <param name="h"></param>
        public virtual void ResizeBox(string position, double h)
        {
            UnBook();
            if (Components["Etage"].ContainsKey(position))
            {
                ((Box)Components["Etage"][position]).Dimensions = new Size3D(Dimensions.X, h, Dimensions.Z);
                ((Box)Components["Etage"][position]).DefaultBox();
                AdjustHeight(position);
                DefaultWardrobe();
            }
            Book();
        }

        /// <summary>
        /// modify the Y dimension of the wardrobe from the box at position (0 check all boxes)
        /// </summary>
        /// <param name="position"></param>
        public virtual void AdjustHeight(string position = "0")
        {
            double height = 0; 
            if (position == "0")
            {
                position = Convert.ToString(Convert.ToInt32(position) + 1);
                ((Box)Components["Etage"]["1"]).Location = new Point3D(0, 0, 0);
                height += ((Box)Components["Etage"]["1"]).Dimensions.Y;
            }
            else
            {
                height = ((Box)Components["Etage"][position]).Location.Y + ((Box)Components["Etage"][position]).Dimensions.Y;
            }
            for (int i = Convert.ToInt32(position) + 1; i <= Components["Etage"].Count; i++)
            {
                ((Box)Components["Etage"][Convert.ToString(i)]).Location = new Point3D(0, ((Box)Components["Etage"][Convert.ToString(i-1)]).Dimensions.Y + ((Box)Components["Etage"][Convert.ToString(i - 1)]).Location.Y, 0);
                height += ((Box)Components["Etage"][Convert.ToString(i)]).Dimensions.Y;
            }
            dimensions.Y = height;
        }

        /// <summary>
        /// Book every part of the wardrobe in the database
        /// </summary>
        /// <param name="buy"></param>
        public virtual void Book(bool buy = false)
        {
            foreach(string name in Components.Keys)
            {
                foreach(string position in Components[name].Keys)
                {
                    if(typeof(Part).IsInstanceOfType(Components[name][position]))
                    {
                        if(!buy)
                        {
                            ((Part)Components[name][position]).Delayed = DbCatalog.DbBook(((Part)Components[name][position]).Code);
                        }
                        else
                        {
                            ((Part)Components[name][position]).Delayed = DbCatalog.DbRemoveFromStock(((Part)Components[name][position]).Code);
                        }
                    }
                    else
                    {
                        ((Box)Components[name][position]).Book(buy);
                    }
                }
            }
        }

        /// <summary>
        /// unbook every part of the wardrobe in the database
        /// </summary>
        public virtual void UnBook()
        {
            foreach (string name in Components.Keys)
            {
                foreach (string position in Components[name].Keys)
                {
                    if (typeof(Part).IsInstanceOfType(Components[name][position]))
                    {
                        DbCatalog.DbUnBook(((Part)Components[name][position]).Code);
                    }
                    else
                    {
                        ((Box)Components[name][position]).UnBook();
                    }
                }
            }
        }

        /// <summary>
        /// Add a box to the wardrobe (at Y position = height of the wardrobe)
        /// </summary>
        /// <param name="h"></param>
        public virtual void AddBox(double h)
        {
            UnBook();
            //resize Wardrobe 
            double location_newbox = Dimensions.Y;
            dimensions.Y = Dimensions.Y + h;
            Size3D dimensions_box = new Size3D(Dimensions.X, h, Dimensions.Z);
            DefaultWardrobe(dimensions_box);
            //component
            Box box = new Box(dimensions_box);
            box.Location = new Point3D(0, location_newbox, 0);
            box.Position = Convert.ToString(Components["Etage"].Count + 1);
            box.DefaultBox();
            Components["Etage"][box.Position] = box;
            //visualPart
            visual_part.AddVisualPart("Etage*" + Convert.ToString(box.Position), ((Box)Components["Etage"][Convert.ToString(box.Position)]).Visual_part,
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
                { "front", new Point(Convert.ToInt32(((Box)Components["Etage"][Convert.ToString(box.Position)]).Location.X), Convert.ToInt32(((Box)Components["Etage"][Convert.ToString(box.Position)]).Location.Y)) },
                { "left", new Point(Convert.ToInt32(((Box)Components["Etage"][Convert.ToString(box.Position)]).Location.Z), Convert.ToInt32(((Box)Components["Etage"][Convert.ToString(box.Position)]).Location.Y)) },
                { "right", new Point(Convert.ToInt32(((Box)Components["Etage"][Convert.ToString(box.Position)]).Location.Z), Convert.ToInt32(((Box)Components["Etage"][Convert.ToString(box.Position)]).Location.Y)) },
                { "rear", new Point(Convert.ToInt32(((Box)Components["Etage"][Convert.ToString(box.Position)]).Location.X), Convert.ToInt32(((Box)Components["Etage"][Convert.ToString(box.Position)]).Location.Y)) },
                { "top", new Point(Convert.ToInt32(((Box)Components["Etage"][Convert.ToString(box.Position)]).Location.X), Convert.ToInt32(((Box)Components["Etage"][Convert.ToString(box.Position)]).Location.Z)) },
                { "bottom", new Point(Convert.ToInt32(((Box)Components["Etage"][Convert.ToString(box.Position)]).Location.X), Convert.ToInt32(((Box)Components["Etage"][Convert.ToString(box.Position)]).Location.Z)) }
            });
            Book();
        }

        /// <summary>
        /// Construct the default wardrobe with the dimensions. Either a single box, or all the box currently in the wardrobe
        /// </summary>
        /// <param name="dimensions_box"></param>
        public virtual void DefaultWardrobe(Size3D dimensions_box = new Size3D())
        {
            if(dimensions_box.Equals(new Size3D()))
            {
                dimensions_box = dimensions;
            }
            string color = "Noir";
            if(Visual_part != null)
            {
                color = DbCatalog.TraduireCouleur(((Part)Components["Cornieres"]["AvG"]).Color.Name);
            }
            //Corniere AvG
            Part cag = DbCatalog.DbSelectPart(new Dictionary<string, string>()
            {
                { "Ref", "Cornieres" },
                { "hauteur>", Convert.ToString(Dimensions.Y) },
                { "couleur", color }
            }, "hauteur ASC");
            cag.Location = new Point3D(0, 0, Dimensions.Z-2);
            cag.Position = "AvG";
            ((Angle)cag).Cut_height = cag.Dimensions.Y - Dimensions.Y;
            cag.ConstructVisualPart();
            if (Visual_part != null)
            {
                color = DbCatalog.TraduireCouleur(((Part)Components["Cornieres"]["AvD"]).Color.Name);
            }
            //Corniere AvD
            Part cad = DbCatalog.DbSelectPart(new Dictionary<string, string>()
            {
                { "Ref", "Cornieres" },
                { "hauteur>", Convert.ToString(Dimensions.Y) },
                { "couleur", color }
            }, "hauteur ASC");
            cad.Location = new Point3D(Dimensions.X - 2, 0, 0);
            cad.Position = "AvD";
            ((Angle)cag).Cut_height = cag.Dimensions.Y - Dimensions.Y;
            cad.ConstructVisualPart();
            if (Visual_part != null)
            {
                color = DbCatalog.TraduireCouleur(((Part)Components["Cornieres"]["ArG"]).Color.Name);
            }
            //Corniere ArG
            Part crg = DbCatalog.DbSelectPart(new Dictionary<string, string>()
            {
                { "Ref", "Cornieres" },
                { "hauteur>", Convert.ToString(Dimensions.Y) },
                { "couleur", color }
            }, "hauteur ASC");
            crg.Location = new Point3D(Dimensions.X - 2, 0, 0);
            crg.Position = "ArG";
            ((Angle)cag).Cut_height = cag.Dimensions.Y - Dimensions.Y;
            crg.ConstructVisualPart();
            if (Visual_part != null)
            {
                color = DbCatalog.TraduireCouleur(((Part)Components["Cornieres"]["ArD"]).Color.Name);
            }
            //Corniere ArD
            Part crd = DbCatalog.DbSelectPart(new Dictionary<string, string>()
            {
                { "Ref", "Cornieres" },
                { "hauteur>", Convert.ToString(Dimensions.Y) },
                { "couleur", color }
            }, "hauteur ASC");
            crd.Location = new Point3D(0, 0, Dimensions.Z - 2);
            crd.Position = "ArD";
            ((Angle)cag).Cut_height = cag.Dimensions.Y - Dimensions.Y;
            crd.ConstructVisualPart();
            Components[crd.Reference] = new Dictionary<string, object>()
            {
                { crd.Position, crd },
                { crg.Position, crg },
                { cad.Position, cad },
                { cag.Position, cag }
            };
            //Box
            if(!Components.ContainsKey("Etage"))
            {
                Box box = new Box(dimensions_box);
                box.Location = new Point3D(0, 0, 0);
                box.Position = "1";
                box.DefaultBox();
                Components["Etage"] = new Dictionary<string, object>()
                {
                    { box.Position, box }
                };
            }

            ConstructVisualPart();
        }

        /// <summary>
        /// Construct the visualPart for the wardrobe
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
            for (int i = 1; i <= Components["Etage"].Count; i++)
            {
                visual_part.AddVisualPart("Etage*" + Convert.ToString(i), ((Box)Components["Etage"][Convert.ToString(i)]).Visual_part,
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
                    { "front", new Point(Convert.ToInt32(((Box)Components["Etage"][Convert.ToString(i)]).Location.X), Convert.ToInt32(((Box)Components["Etage"][Convert.ToString(i)]).Location.Y)) },
                    { "left", new Point(Convert.ToInt32(((Box)Components["Etage"][Convert.ToString(i)]).Location.Z), Convert.ToInt32(((Box)Components["Etage"][Convert.ToString(i)]).Location.Y)) },
                    { "right", new Point(Convert.ToInt32(((Box)Components["Etage"][Convert.ToString(i)]).Location.Z), Convert.ToInt32(((Box)Components["Etage"][Convert.ToString(i)]).Location.Y)) },
                    { "rear", new Point(Convert.ToInt32(((Box)Components["Etage"][Convert.ToString(i)]).Location.X), Convert.ToInt32(((Box)Components["Etage"][Convert.ToString(i)]).Location.Y)) },
                    { "top", new Point(Convert.ToInt32(((Box)Components["Etage"][Convert.ToString(i)]).Location.X), Convert.ToInt32(((Box)Components["Etage"][Convert.ToString(i)]).Location.Z)) },
                    { "bottom", new Point(Convert.ToInt32(((Box)Components["Etage"][Convert.ToString(i)]).Location.X), Convert.ToInt32(((Box)Components["Etage"][Convert.ToString(i)]).Location.Z)) }
                });
            }
        }
    }
}
