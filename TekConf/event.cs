//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré à partir d'un modèle.
//
//     Des modifications manuelles apportées à ce fichier peuvent conduire à un comportement inattendu de votre application.
//     Les modifications manuelles apportées à ce fichier sont remplacées si le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TekConf
{
    using System;
    using System.Collections.Generic;
    
    public partial class Event
    {
        public Event()
        {
            this.Event_AspNetUsers = new HashSet<Event_AspNetUsers>();
            this.Technology = new HashSet<Technology>();
        }
    
        public long id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string location { get; set; }
        public System.DateTime time { get; set; }
        public string event_link { get; set; }
        public string video_link { get; set; }
    
        public virtual ICollection<Event_AspNetUsers> Event_AspNetUsers { get; set; }
        public virtual ICollection<Technology> Technology { get; set; }
    }
}
