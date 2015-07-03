using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TekConf.DataAccess
{
    public class Event
    {
        public static List<TekConf.Event> GetEventsList()
        {
            using (teckconfdbEntities1 db = new teckconfdbEntities1())
            {
                try
                {
                    return db.Event.ToList();
                }
                catch (Exception)
                {
                    return new List<TekConf.Event>();
                }
            }
        }

        public static List<TekConf.Event> GetLastEventsList(int nb)
        {
            using (teckconfdbEntities1 db = new teckconfdbEntities1())
            {
                try
                {
                    List<TekConf.Event> events = db.Event.OrderBy(i => i.time).ToList();
                    events.Reverse();
                    if (events.Count > nb)
                        return events.Take(nb).ToList();
                    return events;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }
    }
}