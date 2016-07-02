using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AutoPartsWebSite.Models;

namespace AutoPartsWebSite.Controllers
{
    public class ImportsController : Controller
    {
        private ImportModel db = new ImportModel();

        // GET: Imports
        public ActionResult Index()
        {
            return View(db.Imports.ToList());
        }

        // GET: Imports/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Import import = db.Imports.Find(id);
            if (import == null)
            {
                return HttpNotFound();
            }
            return View(import);
        }

        // GET: Imports/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Imports/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Date,UserId,SupplierId,DeliveryTime,FileName")] Import import)
        {
            if (ModelState.IsValid)
            {
                db.Imports.Add(import);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(import);
        }

        // GET: Imports/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Import import = db.Imports.Find(id);
            if (import == null)
            {
                return HttpNotFound();
            }
            return View(import);
        }

        // POST: Imports/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Date,UserId,SupplierId,DeliveryTime,FileName")] Import import)
        {
            if (ModelState.IsValid)
            {
                db.Entry(import).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(import);
        }

        // GET: Imports/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Import import = db.Imports.Find(id);
            if (import == null)
            {
                return HttpNotFound();
            }
            return View(import);
        }

        // POST: Imports/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Import import = db.Imports.Find(id);
            db.Imports.Remove(import);
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
