using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace TekConf
{
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom de classe "ServiceTekConf" à la fois dans le code, le fichier svc et le fichier de configuration.
    // REMARQUE : pour lancer le client test WCF afin de tester ce service, sélectionnez ServiceTekConf.svc ou ServiceTekConf.svc.cs dans l'Explorateur de solutions et démarrez le débogage.
    public class ServiceTekConf : IServiceTekConf
    {
        public List<Event> GetLastEventsList(int max)
        {
            return DataAccess.Event.GetLastEventsList(max);
        }
    }
}
