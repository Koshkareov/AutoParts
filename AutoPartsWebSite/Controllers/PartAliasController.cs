using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AutoPartsWebSite.Models;
using PagedList;

namespace AutoPartsWebSite.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PartAliasController : Controller
    {
        private PartModel db = new PartModel();

        // GET: PartAlias
        //public ActionResult Index()
        //{
        //    return View(db.PartAliases.ToList());
        //}

        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.NumberSortParm = sortOrder == "Number" ? "number_desc" : "Number";

            if(searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var partAliases = from s in db.PartAliases
                           select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                partAliases = partAliases.Where(s => s.Number.Contains(searchString)
                                       || s.Name.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "number_desc":
                    partAliases = partAliases.OrderByDescending(s => s.Number);
                    break;
                case "Name":
                    partAliases = partAliases.OrderBy(s => s.Name);
                    break;
                case "name_desc":
                    partAliases = partAliases.OrderByDescending(s => s.Name);
                    break;
                default:
                    partAliases = partAliases.OrderBy(s => s.Number);
                    break;
            }
            //return View(partAliases.ToList());
            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(partAliases.ToPagedList(pageNumber, pageSize));
        }

        // GET: PartAlias/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PartAlias partAlias = db.PartAliases.Find(id);
            if (partAlias == null)
            {
                return HttpNotFound();
            }
            return View(partAlias);
        }

        // GET: PartAlias/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PartAlias/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Number,Name,Details,Size,Weight")] PartAlias partAlias)
        {
            if (ModelState.IsValid)
            {
                db.PartAliases.Add(partAlias);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(partAlias);
        }

        // GET: PartAlias/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PartAlias partAlias = db.PartAliases.Find(id);
            if (partAlias == null)
            {
                return HttpNotFound();
            }
            return View(partAlias);
        }

        // POST: PartAlias/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Number,Name,Details,Size,Weight")] PartAlias partAlias)
        {
            if (ModelState.IsValid)
            {
                db.Entry(partAlias).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(partAlias);
        }

        // GET: PartAlias/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PartAlias partAlias = db.PartAliases.Find(id);
            if (partAlias == null)
            {
                return HttpNotFound();
            }
            return View(partAlias);
        }

        // POST: PartAlias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PartAlias partAlias = db.PartAliases.Find(id);
            db.PartAliases.Remove(partAlias);
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
