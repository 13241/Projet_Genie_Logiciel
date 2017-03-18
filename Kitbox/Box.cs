using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kitbox
{
    class Box
    {
        protected Dictionary<string , Dictionary<string,Part>> parts 
            = new Dictionary<string, Dictionary<string, Part>>();   
        protected VisualPart visual;
        protected double height;

        private Dictionary<string, Part> new_Part;  // Dictionnnaire implémentant le dictonnaire parts

        // Besoin de get/Set?
        public VisualPart Visual
        {
            get { return visual; }
            set { visual = value; }
        }
        

        public bool IsComplete()
        {
            
            bool res = false;


            // Initilise objet de la classe Door
            Door test = new Door();
            Cleat cleat = new Cleat();

            // Compare le nombre de fois qu'on a les éléments constitutifs d'une box; Si on a le bon nombre, res =true et on renvoie res
            if (tasseaux == 4 & traverseAV == 2 & travaerseAR == 2 & traverseGD == 4 & panneauxHB == 2 & panneauxGD == 2 & panneauAR == 1 & test.tDoor == true || false & cleat == 4 parts.TryGetValue())
            { res = true; }

            return res;
            /*
             Idées pour compter le # de fois que pièce est dans parts
             
            A:
                - Faire liste pour chaque pièce, liste contenant les références de chaque pièce prisent par sql
                - Puis comparer clé du dico parts (reference) avec la liste référence de la piste corrrespondante
            
            B:
                - Créer une instance de chaque pièce; 
                - Mettre un compteur dans chaque instance, compteur comtabilisant le nbre de fois que l'on a créé une instance à partir de la classe.

             */
        }

        
        // hauteur casier = hauter tasseaux+2x2cm (hauteur traverses)
        public void SetHeight(double height)
        {
            this.height = height;

            // Ajouter cleats 
            // Valeur initiale de i est identique à la position => Elle est modifiable
            for (int i =0;i<4;i++)
            {
                Cleat cleat = new Cleat();
                string refer = " ";               // Variable contenant la référence de la cornière (Via sql?)
                Add(cleat,i.ToString(),refer);
            }          
        }

        // ajouter paramètre reference? - Que faire si part n'a pas de position
        public void Add(Part newP,string position,string reference)
        {
            /*
      
             Maximum
             # of tasseaux == 4
             # of traverseAv == 2 
             # of traverseAR == 2
             # of traverseGD == 2
             # of panneauxHB == 2
             # of panneauxGD == 2
             # of panneauxAR == 1
             # of door == 1      
                
             */



            new_Part.Clear();   // Vide le dico pour être sûr qu'il n'y pas d'élément superflu ajouté
            new_Part.Add(position, newP);  // Add new part in the Dictionary new_Parts [Create part]
             
            parts.Add(reference,new_Part);    // Add new part in the Dictionry parts [Add part in dico]   
        }

        // N'as pas besoin du paramètre position car chaque part a une ref unique
        public void Remove(string reference, string position)
        {

            parts.Remove(reference); // Delete the pair that has the Key equals to reference
                   
        }

        
        public Dictionary<string,Dictionary<string,Part>> GetParts()
        {
            return parts; // Send the Dictionary Parts
        }

    }
}
