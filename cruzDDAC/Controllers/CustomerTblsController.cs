using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using cruzDDAC.Models;

namespace cruzDDAC.Controllers
{
    public class CustomerTblsController : Controller
    {
        private DDACdbEntities db = new DDACdbEntities();

        // GET: CustomerTbls
        public ActionResult Index(string searchString)
        {
            var cus = from s in db.CustomerTbls
                       select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                cus = cus.Where(s => s.Customer_Name.Contains(searchString));
            }
            return View(cus.ToList());
        }

        // GET: CustomerTbls/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomerTbl customerTbl = db.CustomerTbls.Find(id);
            if (customerTbl == null)
            {
                return HttpNotFound();
            }
            return View(customerTbl);
        }

        // GET: CustomerTbls/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CustomerTbls/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Customer_ID,Customer_Name,Customer_IC,Customer_DateBirth,Customer_Address,Customer_Contact,Customer_Vessel")] CustomerTbl customerTbl)
        {
            if (ModelState.IsValid)
            {
                db.CustomerTbls.Add(customerTbl);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(customerTbl);
        }

        // GET: CustomerTbls/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomerTbl customerTbl = db.CustomerTbls.Find(id);
            if (customerTbl == null)
            {
                return HttpNotFound();
            }
            return View(customerTbl);
        }

        // POST: CustomerTbls/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Customer_ID,Customer_Name,Customer_IC,Customer_DateBirth,Customer_Address,Customer_Contact,Customer_Vessel")] CustomerTbl customerTbl)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customerTbl).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(customerTbl);
        }

        // GET: CustomerTbls/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomerTbl customerTbl = db.CustomerTbls.Find(id);
            if (customerTbl == null)
            {
                return HttpNotFound();
            }
            return View(customerTbl);
        }

        // POST: CustomerTbls/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CustomerTbl customerTbl = db.CustomerTbls.Find(id);
            db.CustomerTbls.Remove(customerTbl);
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
