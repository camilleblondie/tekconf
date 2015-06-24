using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TekConf.Models
{
    public class EventCreateViewModel
    {
        public Event Event { get; set; }
        public List<Technology> technologies { get; set; }
    }

    public class EventDetailsViewModel
    {
        public Event Event { get; set; }
        public List<AspNetUsers> speakers { get; set; }
        public List<AspNetUsers> attendees { get; set; }
        public List<Technology> technologies { get; set; }
    }
}