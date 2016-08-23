using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AutoPartsWebSite.Models;
using Microsoft.AspNet.Identity;

namespace AutoPartsWebSite.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ImportTemplatesController : Controller
    {
        private ImportTemplateModel db = new ImportTemplateModel();

        // GET: ImportTemplates
        public ActionResult Index()
        {
            return View(db.ImportTemplates.ToList());
        }

        // GET: ImportTemplates/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ImportTemplate importTemplate = db.ImportTemplates.Find(id);
            if (importTemplate == null)
            {
                return HttpNotFound();
            }
            return View(importTemplate);
        }

        // GET: ImportTemplates/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ImportTemplates/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Date,UserId,Name,StartRow,BrandColumn,NumberColumn,NameColumn,DetailsColumn,SizeColumn,WeightColumn,QuantityColumn,PriceColumn")] ImportTemplate importTemplate)
        {
            if (ModelState.IsValid)
            {
                importTemplate.UserId = User.Identity.GetUserId();
                importTemplate.Date = System.DateTime.Now;
                db.ImportTemplates.Add(importTemplate);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(importTemplate);
        }

        // GET: ImportTemplates/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ImportTemplate importTemplate = db.ImportTemplates.Find(id);
            if (importTemplate == null)
            {
                return HttpNotFound();
            }
            return View(importTemplate);
        }

        // POST: ImportTemplates/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Date,UserId,Name,StartRow,BrandColumn,NumberColumn,NameColumn,DetailsColumn,SizeColumn,WeightColumn,QuantityColumn,PriceColumn")] ImportTemplate importTemplate)
        {
            if (ModelState.IsValid)
            {
                importTemplate.UserId = User.Identity.GetUserId();
                importTemplate.Date = System.DateTime.Now;
                db.Entry(importTemplate).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(importTemplate);
        }

        // GET: ImportTemplates/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ImportTemplate importTemplate = db.ImportTemplates.Find(id);
            if (importTemplate == null)
            {
                return HttpNotFound();
            }
            return View(importTemplate);
        }

        // POST: ImportTemplates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ImportTemplate importTemplate = db.ImportTemplates.Find(id);

            var supplier = (from s in db.Suppliers
                            select s).Where(c => c.ImportTemplateId.Equals(id));
            if (supplier.FirstOrDefault() != null)
            {
                ModelState.AddModelError(string.Empty, "Этот шаблон нельзя удалить, - он используется.");
                return View(importTemplate);
            }

            db.ImportTemplates.Remove(importTemplate);
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
