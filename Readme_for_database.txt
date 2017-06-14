Attention, on utilise une base de donnée par MYSQL et non SQL.
Les fonctions de MYSQL ne sont pas installées par défaut dans visual studio.

La procédure est simple pour installer le 'module' supplémentaire :

	- Parmis les onglets du dessus appuye sur 'Tools'
	- Un menu déroulant s'affiche
	- Appuye sur Nuget Package Manager
	- Dans la console encode : Install-Package MySql.Data

Normalement ça devrait fonctionner comme ça.
Si il y a un problème demander à Gaëtan car il a aussi fait d'autres trucs mais à priori pas nécessaires.
