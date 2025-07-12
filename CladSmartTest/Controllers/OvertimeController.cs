using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CladSmartTest.DataLayer;

namespace CladSmartTest.Controllers
{
    public class OvertimeController : Controller
    {
        private cladsmartEntities db = new cladsmartEntities();

        // GET: Overtime
        public ActionResult Index()
        {
            var overtimes = db.overtimes.Include(o => o.employee);
            return View(overtimes.ToList());
        }

        // GET: Overtime/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            overtime overtime = db.overtimes.Find(id);
            if (overtime == null)
            {
                return HttpNotFound();
            }
            return View(overtime);
        }

        // GET: Overtime/Create
        public ActionResult Create()
        {
            ViewBag.employee_nik = new SelectList(db.employees, "nik", "name");
            return View();
        }

        // POST: Overtime/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,employee_nik,start_date_time,end_date_time")] overtime overtime)
        {
            var diff = overtime.end_date_time - overtime.start_date_time;

            if (diff.TotalHours < 0.00)
            {
                ViewBag.Alert = "Finish Time Overlapped Start Time";
            }

            if (diff.TotalHours > 3.00 )
            {
                ViewBag.Alert = "Overtime Exceeded Maximum Hours";
            }
            if (ModelState.IsValid && diff.TotalHours <= 3.00 && diff.TotalHours > 0.00)
            {
                db.overtimes.Add(overtime);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.employee_nik = new SelectList(db.employees, "nik", "name", overtime.employee_nik);
            return View(overtime);
        }

        // GET: Overtime/Edit/5
        public ActionResult Edit(long? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            overtime overtime = db.overtimes.Find(id);


            if (overtime == null)
            {
                return HttpNotFound();
            }
            ViewBag.employee_nik = new SelectList(db.employees, "nik", "name", overtime.employee_nik);
            return View(overtime);
        }

        // POST: Overtime/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,employee_nik,start_date_time,end_date_time")] overtime overtime)
        {

            var diff = overtime.end_date_time - overtime.start_date_time;

            if (diff.TotalHours < 0.00)
            {
                ViewBag.Alert = "Finish Time Overlapped Start Time";
            }

            if (diff.TotalHours > 3.00)
            {
                ViewBag.Alert = "Overtime Exceeded Maximum Hours";
            }
            if (ModelState.IsValid && diff.TotalHours <= 3.00 && diff.TotalHours > 0.00)
            {
                db.Entry(overtime).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.employee_nik = new SelectList(db.employees, "nik", "name", overtime.employee_nik);
            return View(overtime);
        }

        // GET: Overtime/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            overtime overtime = db.overtimes.Find(id);
            if (overtime == null)
            {
                return HttpNotFound();
            }
            return View(overtime);
        }

        // POST: Overtime/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            overtime overtime = db.overtimes.Find(id);
            db.overtimes.Remove(overtime);
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
