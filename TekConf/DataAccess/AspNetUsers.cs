﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TekConf.DataAccess
{
    public class AspNetUsers
    {
        public static List<TekConf.AspNetUsers> GetUsersList()
        {
            try
            {
                teckconfdbEntities db = new teckconfdbEntities();
                return db.AspNetUsers.ToList();
            }
            catch (Exception)
            {
                return new List<TekConf.AspNetUsers>();
            }
        }
    }
}