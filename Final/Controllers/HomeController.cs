using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Net;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;


namespace Final.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
      
        public ActionResult Login(string button)
        {
            if (button == "Login")
            {
                return RedirectToAction("Company", "Home");
            }

            return View();
        }
          
        
        public ActionResult Company()
        {
        
            return View();
        }
        public ActionResult ForgotPassword()
        {

            return View();
        }
    }
}