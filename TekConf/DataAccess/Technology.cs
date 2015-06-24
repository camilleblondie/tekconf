using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TekConf.DataAccess
{
    public class Technology
    {
        public static List<TekConf.Technology> GetTechnologiesList()
        {
            using (teckconfdbEntities db = new teckconfdbEntities())
            {
                try
                {
                    return db.Technology.ToList();
                }
                catch (Exception)
                {
                    return new List<TekConf.Technology>();
                }
            }
        }
    }
}