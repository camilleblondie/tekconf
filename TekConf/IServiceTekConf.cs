using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace TekConf
{
	// REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom d'interface "IServiceTekConf" à la fois dans le code et le fichier de configuration.
    [ServiceContract]
    public interface IServiceTekConf
    {
        [OperationContract]
        List<Event> GetLastEventsList(int max);
    }
}
