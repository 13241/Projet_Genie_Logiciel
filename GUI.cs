using System;
using system.Collections.Generic
using System.Windows.Forms;
using System.Drawing;
namespace Kitbox
{
public class GUI : Form
{
    private Label message;
    private Button fermer;

    public GUI()
    {
        SuspendLayout();
        Text = "Une première fenêtre";    // Le titre de la fenêtre
        Size = new Size(200, 150);        // La taille initiale
        MinimumSize = new Size(200, 150); // La taille minimale

        // Le label "Hello world !"
        message = new Label();
        message.Text = "Hello World !";
        message.AutoSize = true;             // Taille selon le contenu
        message.Location = new Point(50, 30);// Position x=50 y=30

        // Le bouton "Fermer"
        fermer = new Button();
        fermer.Text = "Fermer";
        fermer.AutoSize = true;             // Taille selon le contenu
        fermer.Location = new Point(50, 60);// Position x=50 y=60

        fermer.Click += new System.EventHandler(fermer_Click);

        // Ajouter les composants à la fenêtre 
        Controls.Add(message);
        Controls.Add(fermer);

        ResumeLayout(false);
        PerformLayout();
    }

    // Gestionnaire d'événement
    private void fermer_Click(object sender, EventArgs evt)
    {
        // Fin de l'application :
        Application.Exit();
    }

    static void Main()
    {
        // Pour le style XP :
        Application.EnableVisualStyles();

        // Lancement de la boucle de messages
        // pour la fenêtre passée en argument :
        Application.Run(new GUI());
    }
}
