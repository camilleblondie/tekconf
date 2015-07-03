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
            try
            {
                teckconfdbEntities db = new teckconfdbEntities();
                return db.Technology.ToList();
            }
            catch (Exception)
            {
                return new List<TekConf.Technology>();
            }
        }
    }
}