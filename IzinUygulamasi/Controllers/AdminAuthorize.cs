using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace IzinUygulamasi.Controllers
{
    public class AdminAuthorize : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (HttpContext.Current.Session["UYGiris"] != null && HttpContext.Current.Session["UYGiris"] == "basari")
            {
                return true;
            }
            else
            {
                httpContext.Response.Redirect("/Login/Admin");
                return false;
            }

        }
    }
}