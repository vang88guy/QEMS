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

    }
}