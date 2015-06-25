using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TekConf.Models
{
    public class DashboardViewModel
    {
        public List<Event> events { get; set; }
        public AspNetUsers user { get; set; }
    }
}