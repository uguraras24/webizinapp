using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace IzinUygulamasi.Controllers
{
    public class PersonelAuthorize : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (HttpContext.Current.Session["PersonelGiris"] != null && HttpContext.Current.Session["PersonelGiris"] == "basari")
            {
                return true;
            }
            else
            {
                httpContext.Response.Redirect("/Login/Index");
                return false;
            }

        }
    }
}