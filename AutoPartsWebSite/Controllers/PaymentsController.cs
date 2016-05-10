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
using System.Threading.Tasks;
using IdentityAutoPart.Models;
using Microsoft.AspNet.Identity.Owin;

namespace AutoPartsWebSite.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PaymentsController : Controller
    {
        private PaymentModel db = new PaymentModel();
        private  string UserId = "";

        // GET: Payments
        public ActionResult Index()
        {
            return RedirectToAction("Index", "UsersAdmin");
            //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //return View(db.Payments.ToList());
        }

        // GET: Payments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Payment payment = db.Payments.Find(id);
            if (payment == null)
            {
                return HttpNotFound();
            }
            return View(payment);
        }

        // GET: Payments/Create
        public ActionResult Create(string UserId)
        {
            if (UserId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ViewBag.UserId = UserId;
            ViewBag.Data = System.DateTime.Now.ToString("yyyy-MM-dd") ;

            return View();
        }

        // POST: Payments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,UserId,Data,Amount")] Payment payment)
        {            
            if (ModelState.IsValid)
            {                
                db.Payments.Add(payment);
                db.SaveChanges();
                return RedirectToAction("IndexUser", "Payments", new { id = payment.UserId });
            }
            //ViewBag.UserId = UserId;
            //ViewBag.Data = System.DateTime.Now;

            return View(payment);
        }

        // GET: Payments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Payment payment = db.Payments.Find(id);
            if (payment == null)
            {
                return HttpNotFound();
            }
            return View(payment);
        }

        // POST: Payments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UserId,Data,Amount")] Payment payment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(payment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("IndexUser", "Payments", new { id = payment.UserId });
            }
            return View(payment);
        }

        // GET: Payments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Payment payment = db.Payments.Find(id);
            if (payment == null)
            {
                return HttpNotFound();
            }
            return View(payment);
        }

        // POST: Payments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Payment payment = db.Payments.Find(id);
            db.Payments.Remove(payment);
            db.SaveChanges();
            return RedirectToAction("IndexUser", "Payments", new { id = payment.UserId });
        }

        public async Task<ActionResult> IndexUser (string id)
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
            
            var userPayment = (from s in db.Payments
                            select s).Take(1000);
            userPayment = userPayment.Where(s => s.UserId.Equals(id));
            ViewBag.UserId = id;
            ViewBag.UserFullName = user.FullName;
            ViewBag.UserName = user.UserName;


            return View(userPayment.ToList());
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
