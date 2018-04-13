using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using cruzDDAC.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace cruzDDAC.Controllers
{
    public class VesselTblsController : Controller
    {
        private DDACdbEntities db = new DDACdbEntities();

        // GET: VesselTbls
        public ActionResult Index()
        {
            var vesselTbls = db.VesselTbls.Include(v => v.ScheduleTbl);
            return View(vesselTbls.ToList());
        }

        public ActionResult VesselBooking(string sortOrder)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            ApplicationDbContext context = new ApplicationDbContext();
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            ApplicationUser currentUser = UserManager.FindById(User.Identity.GetUserId());
            var vessel = from s in db.VesselTbls
                         where s.Vessel_Agent== currentUser.Email
                         select s;
            switch (sortOrder)
            {
                case "name_desc":
                    vessel = vessel.OrderByDescending(s => s.Vessel_ID);
                    break;
                case "Date":
                    vessel = vessel.OrderBy(s => s.Vessel_Name);
                    break;
                case "date_desc":
                    vessel = vessel.OrderByDescending(s => s.Vessel_Type);
                    break;
                default:
                    vessel = vessel.OrderByDescending(s => s.Vessel_ID);
                    break;
            }

            //var vesselTbls = db.VesselTbls.Include(v => v.CustomerTbls);
            //var vesselTbls = db.VesselTbls.Include(v => v.CustomerTbls).Where(v => v.Vessel_Approval == "Pending");

            return View(vessel.ToList());
        }

        public ActionResult VesselNotify()
        {
            var vessel = from s in db.VesselTbls
                         where s.Vessel_Approval=="Pending"
                         select s;
            vessel = vessel.OrderByDescending(v=>v.Vessel_ID);
            return View(vessel.ToList());
        }

        public ActionResult Notify(int id, string status)
        {

            var ves = db.VesselTbls.Find(id);
            if (ves != null)
            {
                if (status == "A")
                {
                ves.Vessel_Approval = "Approved";
                    db.Entry(ves).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["notice"] = "This booking has Approved!";
                    return RedirectToAction("VesselNotify", "VesselTbls");
                }
                else
                {
                    ves.Vessel_Approval = "Declined";
                    db.Entry(ves).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["notice"] = "This booking has declined successfully";
                    return RedirectToAction("VesselNotify", "VesselTbls");
                }
            }


            return RedirectToAction("VesselNotify", "VesselTbls");
        }
        public ActionResult VesselType(int id)
        {
            ViewBag.Vessel_ScheduleID = id;
            var sche = db.ScheduleTbls.Find(id);
            ViewBag.Remaining = sche.Space_Available;
            ViewBag.Route = sche.Sailing_Route;


            return View();
        }
        // GET: VesselTbls/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VesselTbl vesselTbl = db.VesselTbls.Find(id);
            if (vesselTbl == null)
            {
                return HttpNotFound();
            }
            return View(vesselTbl);
        }

        // GET: VesselTbls/Create
        public ActionResult Create(int a, string b)
        {
            var sche = db.ScheduleTbls.Find(a);
            int? num = sche.Space_Available;
            if (num != null)
            {
                if (b=="A")
                {
                    num = num - 10;
                }
                else if (b == "B")
                {
                    num = num - 40;
                }
                else if (b == "C")
                {
                    num = num - 80;
                }
                else if (b == "D")
                {
                    num = num - 120;
                }
         
            }
            if (num>0)
                {
                    ViewBag.Vessel_ScheduleID = a;
            ViewBag.Vessel_VesselType = b;
                ViewBag.RemainingSpace = num;
               
                ViewBag.Vessel_Customer = new SelectList(db.CustomerTbls, "Customer_ID", "Customer_Name");
   
                return View(); 
                }
                else if (num<=0)
                {
                    TempData["notice"] = "Space is not enough for the schedule, Please Reselect";
                return RedirectToAction("VesselSchedule","ScheduleTbls");

            }
            else
            {
                return View();
            }

           
        }

        // POST: VesselTbls/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Vessel_ID,Vessel_ScheduleID,Vessel_Type,Vessel_Size,Vessel_Name,Vessel_Approval,Vessel_Customer")] VesselTbl vesselTbl)
        {
            if (ModelState.IsValid)
            {
                var sche = db.ScheduleTbls.Find(vesselTbl.Vessel_ScheduleID);
                var cus = db.CustomerTbls.Find(vesselTbl.Vessel_Customer);
                var item = db.ItemTbls.Include(i => i.Item_Customer).Where(i => i.Item_Customer == cus.Customer_ID);
                var itemq = from a in db.CustomerTbls
                             join a1 in db.ItemTbls on a.Customer_ID equals a1.Item_Customer
                                       where a.Customer_ID.Equals(a1.Item_Customer)
                                       select  a1;
              

                if (cus.Customer_Vessel!=null)
                {
                    TempData["notice"] = "Sorry this Customer has already occupied";
                    return RedirectToAction("VesselSchedule","ScheduleTbls");
                }

               

                if (sche != null)
                {
                    sche.Space_Available = vesselTbl.Vessel_Size;
                    db.Entry(sche).State = EntityState.Modified;
                    
                }
              
                if (cus != null)
                {
                    cus.Customer_Vessel = vesselTbl.Vessel_ID;
                    db.Entry(cus).State = EntityState.Modified;
                  
                }
 foreach (var it in itemq)
                {
                    if (it.Item_Customer==cus.Customer_ID)
                    {
 it.Item_Vessel = vesselTbl.Vessel_ID;
                    db.Entry(it).State = EntityState.Modified;
                    }
                   
                }
                vesselTbl.Vessel_Approval = "Pending";
                ApplicationDbContext context = new ApplicationDbContext();
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                ApplicationUser currentUser = UserManager.FindById(User.Identity.GetUserId());
                vesselTbl.Vessel_Agent = currentUser.Email;
                db.VesselTbls.Add(vesselTbl);
                db.SaveChanges();
                TempData["notice"] = "Your Booking has been sent to Admin for Approval";
                return RedirectToAction("Index");
            }

            ViewBag.Vessel_ScheduleID = new SelectList(db.ScheduleTbls, "Schedule_ID", "Sailing_Route", vesselTbl.Vessel_ScheduleID);
            return View(vesselTbl);
        }

        // GET: VesselTbls/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VesselTbl vesselTbl = db.VesselTbls.Find(id);
            if (vesselTbl == null)
            {
                return HttpNotFound();
            }
            ViewBag.Vessel_ScheduleID = new SelectList(db.ScheduleTbls, "Schedule_ID", "Sailing_Route", vesselTbl.Vessel_ScheduleID);
            return View(vesselTbl);
        }

        // POST: VesselTbls/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Vessel_ID,Vessel_ScheduleID,Vessel_Type,Vessel_Size,Vessel_Name,Vessel_Approval,Vessel_Agent")] VesselTbl vesselTbl)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vesselTbl).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Vessel_ScheduleID = new SelectList(db.ScheduleTbls, "Schedule_ID", "Sailing_Route", vesselTbl.Vessel_ScheduleID);
            return View(vesselTbl);
        }

        // GET: VesselTbls/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VesselTbl vesselTbl = db.VesselTbls.Find(id);
            if (vesselTbl == null)
            {
                return HttpNotFound();
            }
            return View(vesselTbl);
        }

        // POST: VesselTbls/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            VesselTbl vesselTbl = db.VesselTbls.Find(id);
            db.VesselTbls.Remove(vesselTbl);
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
