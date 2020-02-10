using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QEMS.Models
{
    public static class StaticMethods
    {
        public static string GetAppId()
        {           
            var userid = HttpContext.Current.User.Identity.GetUserId();
            return userid;
        }
        public static string GetDate() 
        { 
            var datenow = System.DateTime.Now.ToString("MM/dd/yyyy");
            return datenow;
        }
        public static string GetTime() 
        { 
            var timenow = System.DateTime.Now.ToString("h:mm tt");
            return timenow;
        }
    }
}