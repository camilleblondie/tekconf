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
            using (teckconfdbEntities1 db = new teckconfdbEntities1())
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

        //public static bool AddTechnologiesToEvent(long[] technologies, long eventId)
        //{
        //    using (teckconfdbEntities db = new teckconfdbEntities())
        //    {
        //        try
        //        {
        //            var query = insert into teckconfdbEntities.Eve
        //            return true;
        //        }
        //        catch (Exception)
        //        {
        //            return false;
        //        }
        //    }
        //}
    }
}