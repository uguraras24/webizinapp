using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace IzinUygulamasi.Controllers
{
    public class YoneticiAuthorize : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (HttpContext.Current.Session["AYGiris"] != null && HttpContext.Current.Session["AYGiris"] == "basari")
            {
                return true;
            }
            else
            {
                httpContext.Response.Redirect("/Login/AltYonetici");
                return false;
            }

        }
    }
}