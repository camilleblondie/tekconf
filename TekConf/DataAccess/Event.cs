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
            try
            {
                teckconfdbEntities db = new teckconfdbEntities();
                return db.Event.ToList();
            }
            catch (Exception)
            {
                return new List<TekConf.Event>();
            }
        }

        public static List<TekConf.Event> GetLastEventsList(int nb)
        {
            try
            {
                teckconfdbEntities db = new teckconfdbEntities();
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