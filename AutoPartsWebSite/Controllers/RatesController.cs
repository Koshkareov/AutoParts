using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AutoPartsWebSite.Models;
using System.Threading.Tasks;
using IdentityAutoPart.Models;
using Microsoft.AspNet.Identity.Owin;

namespace AutoPartsWebSite.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RatesController : Controller
    {
        private RateModel db = new RateModel();
        private SupplierModel db_supplier = new SupplierModel();
        
        private string UserId = "";

        // GET: Rates
        public ActionResult Index()
        {
            return RedirectToAction("Index", "UsersAdmin");
            //return View(db.Rates.ToList());
        }

        // GET: Rates/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rate rate = db.Rates.Find(id);
            if (rate == null)
            {
                return HttpNotFound();
            }
            return View(rate);
        }

        // GET: Rates/Create
        public ActionResult Create(string UserId)
        {
            if (UserId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ViewBag.UserId = UserId;
            ViewBag.Data = System.DateTime.Now.ToString("yyyy-MM-dd");
            ViewBag.SuppliersList = new SelectList(GetSuplliersList(), "Value", "Text");

            var rate = new Rate();
            rate.Suppliers = from supplier in db_supplier.Suppliers
                             select new SelectListItem { Text = supplier.Name, Value = supplier.Id.ToString() };

            return View(rate);
        }

        // POST: Rates/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,UserId,SupplierId,Data,Value")] Rate rate)
        {
            if (ModelState.IsValid)
            {
                db.Rates.Add(rate);
                db.SaveChanges();
                return RedirectToAction("IndexUser", "Rates", new { id = rate.UserId });
            }
            //rate.Suppliers = new SelectList(GetSuplliersList(), "Value", "Text");
            return View(rate);
        }

        // GET: Rates/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rate rate = db.Rates.Find(id);
            if (rate == null)
            {
                return HttpNotFound();
            }
            rate.Suppliers = from supplier in db_supplier.Suppliers
                             select new SelectListItem { Text = supplier.Name, Value = supplier.Id.ToString() };
            return View(rate);
        }

        // POST: Rates/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UserId,SupplierId,Data,Value")] Rate rate)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rate).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("IndexUser", "Rates", new { id = rate.UserId });
            }
            return View(rate);
        }

        // GET: Rates/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rate rate = db.Rates.Find(id);
            if (rate == null)
            {
                return HttpNotFound();
            }
            return View(rate);
        }

        // POST: Rates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Rate rate = db.Rates.Find(id);
            db.Rates.Remove(rate);
            db.SaveChanges();
            return RedirectToAction("IndexUser", "Rates", new { id = rate.UserId });
        }


        public async Task<ActionResult> IndexUser(string id)
        {
            ApplicationUserManager UserManager = HttpContext.GetOwinContext()
                                            .GetUserManager<ApplicationUserManager>();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            // set class var
            UserId = id;
            var user = await UserManager.FindByIdAsync(id);
            if (user == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var userRate = (from s in db.Rates
                               select s).Take(1000);
            userRate = userRate.Where(s => s.UserId.Equals(id));
            
            ViewBag.UserId = id;
            ViewBag.UserFullName = user.FullName;
            ViewBag.UserName = user.UserName;
            ViewBag.SuppliersList = GetSuplliersList();


            return View(userRate.ToList());
        }

        public List<Rate> GetUserRates(string id)
        {
            var userRates = (from s in db.Rates
                                select s).Take(1000);
            userRates = userRates.Where(s => s.UserId.Equals(id));
            return userRates.ToList();
        }
        
        public List<SelectListItem> GetSuplliersList()
        {
            var suppliers = (from s in db_supplier.Suppliers
                             select s).Take(1000);

            List<SelectListItem> suplliersList = new List<SelectListItem>();
            foreach (var supplier in suppliers.ToList())
            {
                suplliersList.Add(new SelectListItem()
                {                    
                    Text = supplier.Name.ToString(),
                    Value = supplier.Id.ToString()
                });
            }            
            return suplliersList;
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
