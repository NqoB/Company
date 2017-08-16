using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Final.Controllers
{
    public class FunctionController : Controller
    {
        // GET: Function
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Add(string button1, string button2, string button3)
        {
       
            if (button1 == "Add")
            {
                return RedirectToAction("Add", "Function");
            }


            if (button2 == "Change")
            {
                return RedirectToAction("Add", "Function");
            }

       

            if (button3 == "Delete")
            {
                return RedirectToAction("Add", "Function");
            }

            return View();
        }
        public ActionResult Change()
        {
            return View();
        }
    }
    
}