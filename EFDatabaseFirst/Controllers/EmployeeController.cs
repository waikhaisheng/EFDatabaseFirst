using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EFDatabaseFirst.Models;

namespace EFDatabaseFirst.Controllers
{
    public class EmployeeController : Controller
    {
        private EmployeeContext db = new EmployeeContext();

        // GET: /Employee/
        public ActionResult Index()
        {
            var tblemployees = db.tblEmployees.Include(t => t.tblDepartment);
            return View(tblemployees.ToList());
        }

        // GET: /Employee/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblEmployee tblemployee = db.tblEmployees.Find(id);
            if (tblemployee == null)
            {
                return HttpNotFound();
            }
            return View(tblemployee);
        }

        // GET: /Employee/Create
        public ActionResult Create()
        {
            ViewBag.DepartmentId = new SelectList(db.tblDepartments, "Id", "Name");
            return View();
        }

        // POST: /Employee/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="EmployeeId,Name,Gender,City,DepartmentId")] tblEmployee tblemployee)
        {
            if (string.IsNullOrEmpty(tblemployee.Name))
                ModelState.AddModelError("Name", "The Name field is required.");
            if (string.IsNullOrEmpty(tblemployee.Gender) || tblemployee.Gender != "Male" || tblemployee.Gender!= "Female")
                ModelState.AddModelError("Gebder", "is require.");
            if (ModelState.IsValid)
            {
                db.tblEmployees.Add(tblemployee);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DepartmentId = new SelectList(db.tblDepartments, "Id", "Name", tblemployee.DepartmentId);
            return View(tblemployee);
        }

        // GET: /Employee/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblEmployee tblemployee = db.tblEmployees.Find(id);
            if (tblemployee == null)
            {
                return HttpNotFound();
            }
            ViewBag.DepartmentId = new SelectList(db.tblDepartments, "Id", "Name", tblemployee.DepartmentId);
            return View(tblemployee);
        }

        // POST: /Employee/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include="EmployeeId,Name,Gender,City,DepartmentId")] tblEmployee tblemployee)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(tblemployee).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.DepartmentId = new SelectList(db.tblDepartments, "Id", "Name", tblemployee.DepartmentId);
        //    return View(tblemployee);
        //}
        [HttpPost]
        public ActionResult Edit([Bind(Exclude = "Name")] tblEmployee employee)
        {
            tblEmployee employeeFromDB = db.tblEmployees.Single(x => x.EmployeeId == employee.EmployeeId);

            employeeFromDB.EmployeeId = employee.EmployeeId;
            employeeFromDB.Gender = employee.Gender;
            employeeFromDB.City = employee.City;
            employeeFromDB.DepartmentId = employee.DepartmentId;
            employee.Name = employeeFromDB.Name;

            if (ModelState.IsValid)
            {
                //db.ObjectStateManager.ChangeObjectState(employeeFromDB, EntityState.Modified);
                db.Entry(employeeFromDB).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DepartmentId = new SelectList(db.tblDepartments, "Id", "Name", employee.DepartmentId);
            return View(employee);
        }

        // GET: /Employee/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblEmployee tblemployee = db.tblEmployees.Find(id);
            if (tblemployee == null)
            {
                return HttpNotFound();
            }
            return View(tblemployee);
        }

        // POST: /Employee/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblEmployee tblemployee = db.tblEmployees.Find(id);
            db.tblEmployees.Remove(tblemployee);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult EmployeeByDepartment()
        {
            var dt = db.tblEmployees.Include("tblDepartment")
                                    .GroupBy(x => x.tblDepartment.Name)
                                    .Select(y => new DepartmentTotals() { 
                                        Name = y.Key, Total = y.Count()
                                    });
            return View(dt);
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
