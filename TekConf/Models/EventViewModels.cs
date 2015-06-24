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
        public List<AspNetUsers> speakers { get; set; }
    }

    public class EventEditViewModel
    {
        public Event Event { get; set; }
        public List<Technology> selectedTechnologies { get; set; }
        public List<Technology> technologies { get; set; }
        public List<AspNetUsers> selectedSpeakers { get; set; }
        public List<AspNetUsers> speakers { get; set; }
    }
}