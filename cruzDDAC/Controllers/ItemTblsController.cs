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
    public class ItemTblsController : Controller
    {
        private DDACdbEntities db = new DDACdbEntities();

        // GET: ItemTbls
        public ActionResult Index(string searchString)
        {
           

            var item = from s in db.ItemTbls.Include(i => i.VesselTbl).Include(i => i.CustomerTbl)
                       select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                item = item.Where(s => s.Item_Name.Contains(searchString));
            }
           
            return View(item.ToList());
        }

        // GET: ItemTbls/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ItemTbl itemTbl = db.ItemTbls.Find(id);
            if (itemTbl == null)
            {
                return HttpNotFound();
            }
            return View(itemTbl);
        }

        // GET: ItemTbls/Create
        public ActionResult Create()
        {
            ViewBag.Item_Vessel = new SelectList(db.VesselTbls, "Vessel_ID", "Vessel_Type");
            ViewBag.Item_Customer = new SelectList(db.CustomerTbls, "Customer_ID", "Customer_Name");
            return View();
        }

        // POST: ItemTbls/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Item_ID,Item_Name,Item_Quantity,Item_Customer,Item_Vessel")] ItemTbl itemTbl)
        {
            if (ModelState.IsValid)
            {
                db.ItemTbls.Add(itemTbl);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Item_Vessel = new SelectList(db.VesselTbls, "Vessel_ID", "Vessel_Type", itemTbl.Item_Vessel);
            ViewBag.Item_Customer = new SelectList(db.CustomerTbls, "Customer_ID", "Customer_Name", itemTbl.Item_Customer);
            return View(itemTbl);
        }

        // GET: ItemTbls/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ItemTbl itemTbl = db.ItemTbls.Find(id);
            if (itemTbl == null)
            {
                return HttpNotFound();
            }
            ViewBag.Item_Vessel = new SelectList(db.VesselTbls, "Vessel_ID", "Vessel_Type", itemTbl.Item_Vessel);
            ViewBag.Item_Customer = new SelectList(db.CustomerTbls, "Customer_ID", "Customer_Name", itemTbl.Item_Customer);
            return View(itemTbl);
        }

        // POST: ItemTbls/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Item_ID,Item_Name,Item_Quantity,Item_Customer,Item_Vessel")] ItemTbl itemTbl)
        {
            if (ModelState.IsValid)
            {
                db.Entry(itemTbl).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Item_Vessel = new SelectList(db.VesselTbls, "Vessel_ID", "Vessel_Type", itemTbl.Item_Vessel);
            ViewBag.Item_Customer = new SelectList(db.CustomerTbls, "Customer_ID", "Customer_Name", itemTbl.Item_Customer);
            return View(itemTbl);
        }

        // GET: ItemTbls/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ItemTbl itemTbl = db.ItemTbls.Find(id);
            if (itemTbl == null)
            {
                return HttpNotFound();
            }
            return View(itemTbl);
        }

        // POST: ItemTbls/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ItemTbl itemTbl = db.ItemTbls.Find(id);
            db.ItemTbls.Remove(itemTbl);
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
