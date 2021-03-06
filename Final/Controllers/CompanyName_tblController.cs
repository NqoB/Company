﻿using System;

using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Final.Models;

namespace Final.Controllers
{
    public class CompanyName_tblController : Controller
    {
        private CompanyCaptureDBEntities2 db = new CompanyCaptureDBEntities2();
        // GET: CompanyName_tbl

        [HttpGet]
        public ActionResult Index(string Search_Data, string exchangeCodeID, string companyTypeID,string companyNameList,string exchangeCodeName, int? countryID)
        {

            var exchange = db.Exchange_tbl
            .OrderBy(q => q.countryID)
            .ToDictionary(q => q.exchangeName, q => q.exchangeCodeID + " " + q.exchangeName);
            ViewBag.exchangeCodeName = new SelectList(exchange, "Key", "Value");


            ViewBag.exchangeCodeID = new SelectList(db.Exchange_tbl, "exchangeCodeID", "exchangeName");
            ViewBag.companyTypeID = new SelectList(db.CompanyType_tbl, "companyTypeID", "companyTypeDesc");

            //Country_tbl rvm = new Country_tbl();

            //Country_tbl Report = db.Country_tbl.Find(countryID);
            //List<Country_tbl> tablesinreport = new List<Country_tbl>();


            //IQueryable<Country_tbl> ReportTables = from t1 in db.CountryCompanyName_tbl
            //                                       join t2 in db.Country_tbl on t1.companyID equals t2.countryID
            //                                       where t1.countryID == countryID
            //                                       select t2;
            //tablesinreport = ReportTables.ToList();

            
            ViewBag.country = new SelectList(db.Country_tbl, "countryID", "countryName");

            var companyList = new List<String> { "Company Name", "Business Sector", "Country" };
            ViewBag.companyNameList = new SelectList(companyList);

            var companyName1 = from s in db.CompanyName_tbl
                               //join c in db.Country_tbl on s.countryID equals c.countryID
                               //where s.countryID == countryID
                               select s;


            if (!String.IsNullOrEmpty(Search_Data))
            {
                if (companyNameList == "Company Name" || companyNameList == null)
                {
                    companyName1 = companyName1.Where(s => s.companyName.Contains(Search_Data));
                }
                else if (companyNameList == "Business Sector" || companyNameList == null)
                {
                    companyName1 = companyName1.Where(s => s.BusinessSector_tbl.businessSectorDesc.Contains(Search_Data));
                }
                else
                {
                    var countries = db.Country_tbl.ToList();
                    List<int> countryIds = countries.Where(x => x.countryName.ToLower().Contains(Search_Data.ToLower())).Select(x => x.countryID).ToList();
                    List<int> companyIds = db.countrycompviews.Where(x => countryIds.Contains(x.countryID)).Select(x => x.companyID).ToList();
                    companyName1 = companyName1.Where(s => companyIds.Contains(s.companyID));
                }

            }

            if (!String.IsNullOrEmpty(exchangeCodeID))
            {
                companyName1 = companyName1.Where(s => s.exchangeCodeID == exchangeCodeID);
            }
            if (!String.IsNullOrEmpty(companyTypeID))
            {
                companyName1 = companyName1.Where(s => s.companyTypeID.ToString() == companyTypeID);
            }




        return View(companyName1.ToList());

        }


        public ActionResult Search(string searchBy, string list)
        {

            var ola = new object[] { "Exchange", "Business Sector", "Country", "Company Name" };
            ViewBag.list = new SelectList(ola);

            ViewBag.companyNameID = new SelectList(db.CompanyName_tbl, "companyID", "companyName");
            ViewBag.businessSectorID = new SelectList(db.BusinessSector_tbl, "businessSectorID", "businessSectorDesc");
            ViewBag.companyTypeID = new SelectList(db.CompanyType_tbl, "companyTypeID", "companyTypeDesc");
            ViewBag.exchangeCodeID = new SelectList(db.Exchange_tbl, "exchangeCodeID", "exchangeName");

            var comName = from g in db.CompanyName_tbl
                          select g;

            if (!String.IsNullOrEmpty(searchBy))
            {

                if (list.Equals("Company Name"))
                {

                    comName = comName.Where(s => s.companyName.Contains(searchBy));
                }

                else if (list.Equals("Exchange"))
                {

                    comName = comName.Where(s => s.exchangeCodeID.Contains(searchBy));
                }

                else if (list.Equals("Business Sector"))
                {
                    comName = comName.Where(s => s.BusinessSector_tbl.businessSectorDesc.Contains(searchBy));
                }
                else 
                {
                    comName = comName.Where(s => s.countryID.ToString().Contains(searchBy));
                }
            }   
            return View(comName.ToList());
        }
        // GET: CompanyName_tbl/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CompanyName_tbl companyName_tbl = db.CompanyName_tbl.Find(id);
            if (companyName_tbl == null)
            {
                return HttpNotFound();
            }
            return View(companyName_tbl);
        }

        // GET: CompanyName_tbl/Create

        public ActionResult Create()
        {
            ViewBag.businessSectorID = new SelectList(db.BusinessSector_tbl, "businessSectorID", "businessSectorDesc");
            ViewBag.companyTypeID = new SelectList(db.CompanyType_tbl, "companyTypeID", "companyTypeDesc");
            ViewBag.exchangeCodeID = new SelectList(db.Exchange_tbl, "exchangeCodeID", "exchangeName");
            return View();
        }

        // POST: CompanyName_tbl/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "companyID,companyName,shortCode,corpInfo,updateDate,countryID,exchangeCodeID,companyTypeID,businessSectorID")] CompanyName_tbl companyName_tbl)
        {
            if (ModelState.IsValid)
            {
                db.CompanyName_tbl.Add(companyName_tbl);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.businessSectorID = new SelectList(db.BusinessSector_tbl, "businessSectorID", "businessSectorDesc", companyName_tbl.businessSectorID);
            ViewBag.companyTypeID = new SelectList(db.CompanyType_tbl, "companyTypeID", "companyTypeDesc", companyName_tbl.companyTypeID);
            ViewBag.exchangeCodeID = new SelectList(db.Exchange_tbl, "exchangeCodeID", "exchangeName", companyName_tbl.exchangeCodeID);
            return View(companyName_tbl);
        }

        // GET: CompanyName_tbl/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CompanyName_tbl companyName_tbl = db.CompanyName_tbl.Find(id);
            if (companyName_tbl == null)
            {
                return HttpNotFound();
            }
            ViewBag.businessSectorID = new SelectList(db.BusinessSector_tbl, "businessSectorID", "businessSectorDesc", companyName_tbl.businessSectorID);
            ViewBag.companyTypeID = new SelectList(db.CompanyType_tbl, "companyTypeID", "companyTypeDesc", companyName_tbl.companyTypeID);
            ViewBag.exchangeCodeID = new SelectList(db.Exchange_tbl, "exchangeCodeID", "exchangeName", companyName_tbl.exchangeCodeID);
            return View(companyName_tbl);
        }

        // POST: CompanyName_tbl/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "companyID,companyName,shortCode,corpInfo,updateDate,countryID,exchangeCodeID,companyTypeID,businessSectorID")] CompanyName_tbl companyName_tbl)
        {
            if (ModelState.IsValid)
            {
                db.Entry(companyName_tbl).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.businessSectorID = new SelectList(db.BusinessSector_tbl, "businessSectorID", "businessSectorDesc", companyName_tbl.businessSectorID);
            ViewBag.companyTypeID = new SelectList(db.CompanyType_tbl, "companyTypeID", "companyTypeDesc", companyName_tbl.companyTypeID);
            ViewBag.exchangeCodeID = new SelectList(db.Exchange_tbl, "exchangeCodeID", "exchangeName", companyName_tbl.exchangeCodeID);
            return View(companyName_tbl);
        }

        // GET: CompanyName_tbl/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CompanyName_tbl companyName_tbl = db.CompanyName_tbl.Find(id);
            if (companyName_tbl == null)
            {
                return HttpNotFound();
            }
            return View(companyName_tbl);
        }

        // POST: CompanyName_tbl/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CompanyName_tbl companyName_tbl = db.CompanyName_tbl.Find(id);
            db.CompanyName_tbl.Remove(companyName_tbl);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
