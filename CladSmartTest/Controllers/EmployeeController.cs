﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CladSmartTest.DataLayer;

namespace CladSmartTest.Controllers
{
    public class EmployeeController : Controller
    {
        private cladsmartEntities db = new cladsmartEntities();

        // GET: Employee
        public ActionResult Index()
        {
            var employees = db.employees.Include(e => e.department);
            return View(employees.ToList());
        }

        // GET: Employee/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            employee employee = db.employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // GET: Employee/Create
        public ActionResult Create()
        {
            List<SelectListItem> positions = new List<SelectListItem>()
            {
                new SelectListItem(){ Text = "Operator", Value= "Operator"},
                new SelectListItem(){ Text = "Technician", Value= "Technician"},
                new SelectListItem(){ Text = "Leader", Value= "Leader"},
                new SelectListItem(){ Text = "Supervisor", Value= "Supervisor"},
                new SelectListItem(){ Text = "Manager", Value= "Manager"},
            };

            ViewBag.position = new SelectList(positions,"Value", "Text");
            ViewBag.department_id = new SelectList(db.departments, "id", "department_name");
            return View();
        }

        // POST: Employee/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "nik,name,department_id,position,bpjs_allowance,meal_allowance,laptop_allowance")] employee employee)
        {
            bool idExist = db.employees.Any(emp => emp.nik.Equals(employee.nik));
            if (idExist)
            {
                ViewBag.Alert = "Exception: NIK already exists in database.";
            }
            if (ModelState.IsValid && !idExist)
            {
                db.employees.Add(employee);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            List<SelectListItem> positions = new List<SelectListItem>()
            {
                new SelectListItem(){ Text = "Operator", Value= "Operator"},
                new SelectListItem(){ Text = "Technician", Value= "Technician"},
                new SelectListItem(){ Text = "Leader", Value= "Leader"},
                new SelectListItem(){ Text = "Supervisor", Value= "Supervisor"},
                new SelectListItem(){ Text = "Manager", Value= "Manager"},
            };

            ViewBag.position = new SelectList(positions, "Value", "Text", employee.position);

            ViewBag.department_id = new SelectList(db.departments, "id", "department_name", employee.department_id);
            return View(employee);
        }

        // GET: Employee/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            employee employee = db.employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }

            List<SelectListItem> positions = new List<SelectListItem>()
            {
                new SelectListItem(){ Text = "Operator", Value= "Operator"},
                new SelectListItem(){ Text = "Technician", Value= "Technician"},
                new SelectListItem(){ Text = "Leader", Value= "Leader"},
                new SelectListItem(){ Text = "Supervisor", Value= "Supervisor"},
                new SelectListItem(){ Text = "Manager", Value= "Manager"},
            };

            ViewBag.position = new SelectList(positions, "Value", "Text", employee.position);

            ViewBag.department_id = new SelectList(db.departments, "id", "department_name", employee.department_id);
            return View(employee);
        }

        // POST: Employee/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "nik,name,department_id,position,bpjs_allowance,meal_allowance,laptop_allowance")] employee employee)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employee).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.department_id = new SelectList(db.departments, "id", "department_name", employee.department_id);
            return View(employee);
        }

        // GET: Employee/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            employee employee = db.employees.Find(id);
      
            if (employee == null)
            {
                return HttpNotFound();
            }

            var overtimes = db.overtimes.Include(o => o.employee);

            bool overtimeExist = overtimes.ToList().Find(o => o.employee_nik == id) != null;

            if(overtimeExist)
            {
                ViewBag.Alert = "Employee Has Overtime Data";
            }
            return View(employee);
        }

        // POST: Employee/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            var overtimes = db.overtimes.Include(o => o.employee);

            bool overtimeExist = overtimes.ToList().Find(o => o.employee_nik == id) != null;

            if (overtimeExist)
            {
                return RedirectToAction("Index");
            }
            employee employee = db.employees.Find(id);
            db.employees.Remove(employee);
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
