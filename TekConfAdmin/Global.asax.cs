using System;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.DynamicData;
using System.Web.Routing;
using System.Web.UI;

namespace TekConfAdmin
{
    public class Global : System.Web.HttpApplication
    {
        private static MetaModel s_defaultModel = new MetaModel();
        public static MetaModel DefaultModel
        {
            get
            {
                return s_defaultModel;
            }
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            //                    IMPORTANT : INSCRIPTION DU MODÈLE DE DONNÉES 
            // Supprimez les marques de commentaire de la ligne pour inscrire un modèle ADO.NET Entity Framework pour Dynamic Data ASP.NET.
            // Définissez ScaffoldAllTables = true uniquement si vous êtes certain que vous voulez que toutes les tables du
            // modèle de données prennent en charge une vue de génération de modèles automatique (c’est-à-dire des modèles). Pour contrôler la génération de modèles automatique
            // des tables individuelles, créez une classe partielle pour la table et appliquez
            // l'attribut [ScaffoldTable(true)] à la classe partielle.
            // Remarque : vérifiez que vous remplacez "YourDataContextType" par le nom de la classe du contexte de données
            // de votre application.
            // Pour plus d’informations sur l’enregistrement de l’Entity Data Model avec Dynamic Date, consultez http://go.microsoft.com/fwlink/?LinkId=257395            
            // DefaultModel.RegisterContext(() =>
            // {
            //    return ((IObjectContextAdapter)new YourDataContextType()).ObjectContext;
            // }, new ContextConfiguration() { ScaffoldAllTables = false });
            DefaultModel.RegisterContext(typeof(TekConf.teckconfdbEntities)
             , new ContextConfiguration() { ScaffoldAllTables = true });
            // L’enregistrement suivant doit être utilisé si  YourDataContextType ne dérive pas de DbContext
            // DefaultModel.RegisterContext(typeof(YourDataContextType), new ContextConfiguration() { ScaffoldAllTables = false });

            // L’instruction suivante prend en charge le mode separate-page, où les tâches Liste, Détail, Insérer et 
            // Mettre à jour sont exécutées à l’aide de pages distinctes. Pour activer ce mode, supprimez les marques de commentaire 
            // de la définition route suivante et commentez les définitions de route dans la section de mode combined-page qui suit.
            routes.Add(new DynamicDataRoute("{table}/{action}.aspx")
            {
                Constraints = new RouteValueDictionary(new { action = "List|Details|Edit|Insert" }),
                Model = DefaultModel
            });

            // Les instructions suivantes prennent en charge le mode combined-page, où les tâches Liste, Détail, Insérer et
            // Mettre à jour sont exécutées à l’aide de la même page. Pour activer ce mode, supprimez les marques de commentaire
            // suivant les routes et commentez la définition de l’itinéraire dans la section du mode page séparée ci-dessus.
            //routes.Add(new DynamicDataRoute("{table}/ListDetails.aspx") {
            //    Action = PageAction.List,
            //    ViewName = "ListDetails",
            //    Model = DefaultModel
            //});

            //routes.Add(new DynamicDataRoute("{table}/ListDetails.aspx") {
            //    Action = PageAction.Details,
            //    ViewName = "ListDetails",
            //    Model = DefaultModel
            //});
        }

        private static void RegisterScripts()
        {
            ScriptManager.ScriptResourceMapping.AddDefinition("jquery", new ScriptResourceDefinition
            {
                Path = "~/Scripts/jquery-1.7.1.min.js",
                DebugPath = "~/Scripts/jquery-1.7.1.js",
                CdnPath = "http://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.7.1.min.js",
                CdnDebugPath = "http://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.7.1.js",
                CdnSupportsSecureConnection = true,
                LoadSuccessExpression = "window.jQuery"
            });
        }

        void Application_Start(object sender, EventArgs e)
        {
            RegisterRoutes(RouteTable.Routes);
            RegisterScripts();
        }
    }
}
