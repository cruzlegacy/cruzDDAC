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
    public class ScheduleTblsController : Controller
    {
        private DDACdbEntities db = new DDACdbEntities();

        // GET: ScheduleTbls
        public ActionResult Index(string searchString)
        {
          
            var sche = from s in db.ScheduleTbls
                           select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                sche = sche.Where(s => s.Sailing_Route.Contains(searchString));
            }
            return View(sche.ToList());
        }

        public ActionResult VesselSchedule(string searchString)
        {

            var sche = from s in db.ScheduleTbls
                       select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                sche = sche.Where(s => s.Sailing_Route.Contains(searchString));
            }
            return View(sche.ToList());
        }
        // GET: ScheduleTbls/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ScheduleTbl scheduleTbl = db.ScheduleTbls.Find(id);
            if (scheduleTbl == null)
            {
                return HttpNotFound();
            }
            return View(scheduleTbl);
        }

        // GET: ScheduleTbls/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ScheduleTbls/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Schedule_ID,Sailing_Route,Sailing_Destination,Sailing_DepartureDate,Sailing_ArrivalDate,Space_Available,Space_Size,Sailing_Captain")] ScheduleTbl scheduleTbl)
        {
            if (ModelState.IsValid)
            {

                if (scheduleTbl.Sailing_ArrivalDate > scheduleTbl.Sailing_DepartureDate)
                {
                db.ScheduleTbls.Add(scheduleTbl);
                db.SaveChanges();
                    TempData["notice"] = "You have created a Sailing Schedule successfully";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["notice"] = "Sailing Arrival Date must be greater than Departure Date";
                    return View(scheduleTbl);
                }
                   
            }

            return View(scheduleTbl);
        }

        // GET: ScheduleTbls/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ScheduleTbl scheduleTbl = db.ScheduleTbls.Find(id);
            if (scheduleTbl == null)
            {
                return HttpNotFound();
            }
            return View(scheduleTbl);
        }

        // POST: ScheduleTbls/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Schedule_ID,Sailing_Route,Sailing_Destination,Sailing_DepartureDate,Sailing_ArrivalDate,Space_Available,Space_Size,Sailing_Captain")] ScheduleTbl scheduleTbl)
        {
            if (ModelState.IsValid)
            {
                db.Entry(scheduleTbl).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(scheduleTbl);
        }

        // GET: ScheduleTbls/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ScheduleTbl scheduleTbl = db.ScheduleTbls.Find(id);
            if (scheduleTbl == null)
            {
                return HttpNotFound();
            }
            return View(scheduleTbl);
        }

        // POST: ScheduleTbls/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ScheduleTbl scheduleTbl = db.ScheduleTbls.Find(id);
            db.ScheduleTbls.Remove(scheduleTbl);
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
