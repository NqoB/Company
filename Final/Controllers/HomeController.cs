using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Net;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using Final.Models;


namespace Final.Controllers
{
    public class HomeController : Controller
    {


        private CompanyCaptureDBEntities2 db = new CompanyCaptureDBEntities2();
        // GET: Home

        public ActionResult Login1(string button)
        {

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login1(User_tbl objUser)
        {
            if (ModelState.IsValid)
            {
                using (CompanyCaptureDBEntities2 db = new CompanyCaptureDBEntities2())
                {
                    var obj = db.User_tbl.Where(a => a.employeeNo.Equals(objUser.employeeNo) && a.password.Equals(objUser.password)).FirstOrDefault();
                    if (obj != null)
                    {
                        Session["employeeNo"] = obj.employeeNo.ToString();
                        Session["password"] = obj.password.ToString();
                        return RedirectToAction("Dashboard");
                    }
                }
            }
            return View(objUser);
        }
        public ActionResult Dashboard()
        {
            if (Session["employeeNo"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login1");
            }
        }
        public ActionResult ForgotPassword()
        {

            return View();
        }
    }
}