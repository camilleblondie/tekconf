//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApplicationBackEnd.DataAccess
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
