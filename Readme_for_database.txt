Attention, on utilise une base de donnée par MYSQL et non SQL.
Les fonctions de MYSQL ne sont pas installées par défaut dans visual studio.

La procédure est simple pour installer le 'module' supplémentaire :

	- Parmis les onglets du dessus appuye sur 'Tools'
	- Un menu déroulant s'affiche
	- Appuye sur Nuget Package Manager
	- Dans la console encode : Install-Package MySql.Data

Normalement ça devrait fonctionner comme ça.
Si y a un problème demande moi car j'ai aussi fait d'autre truc mais je ne crois pas que c'est nécessaire.