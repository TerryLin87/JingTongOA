using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JingTongOA.Controllers
{
    public class NeedLoginController : BaseController
    {
        public NeedLoginController()
        {
            if (CurrentUser == null)
            {
                System.Web.HttpContext.Current.Response.Redirect("../Login/Login", true);
            }
        }
    }
}