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
    public class CartsController : Controller
    {
        private CartModel db = new CartModel();

        // GET: Carts
        public ActionResult Index()
        {
            string currentUserId = User.Identity.GetUserId();
            var userCart = (from s in db.Carts
                            select s).Take(100);
            userCart = userCart.Where(s => s.UserId.Equals(currentUserId));
            return View(userCart.ToList());
        }

        // GET: Carts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cart cart = db.Carts.Find(id);
            if (cart == null)
            {
                return HttpNotFound();
            }
            return View(cart);
        }

        // GET: Carts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Carts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,UserId,Brand,Number,Name,Details,Size,Weight,Quantity,Price,Supplier,DeliveryTime,Amount,Data")] Cart cart)
        {
            cart.UserId = User.Identity.GetUserId();
            cart.Data = DateTime.Now;
            if (ModelState.IsValid)
            {               
                db.Carts.Add(cart);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cart);
        }

        // GET: Carts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cart cart = db.Carts.Find(id);
            if (cart == null)
            {
                return HttpNotFound();
            }
            return View(cart);
        }

        // POST: Carts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UserId,Brand,Number,Name,Details,Size,Weight,Quantity,Price,Supplier,DeliveryTime,Amount,Data")] Cart cart)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cart).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cart);
        }

        // GET: Carts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cart cart = db.Carts.Find(id);
            if (cart == null)
            {
                return HttpNotFound();
            }
            return View(cart);
        }

        // POST: Carts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Cart cart = db.Carts.Find(id);
            db.Carts.Remove(cart);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public RedirectToRouteResult AddToCart(int? PartId, int? Amount, string returnUrl)
        {
            if (PartId != null)
            {
                //var autoparts = (from s in db.Parts
                //                 select s).Take(100);
                //autoparts = autoparts.Where(s => s.Id.Equals(PartId)).Take(1);
                Part autopart = db.Parts.Find(PartId); // (Part)autoparts;

                if ((autopart != null) && (Amount != null) && (Amount != 0))
                {
                    Cart cart = new Cart();
                    cart.UserId = User.Identity.GetUserId();

                    cart.Brand = autopart.Brand;
                    cart.Number = autopart.Number;
                    cart.Name = autopart.Name;
                    cart.Details = autopart.Details;
                    cart.Size = autopart.Size;
                    cart.Weight = autopart.Weight;
                    cart.Quantity = autopart.Quantity;
                    cart.Supplier = autopart.Supplier;
                    cart.Price = autopart.Price;
                    cart.DeliveryTime = autopart.DeliveryTime;
                    cart.Amount = Amount; 
                    cart.Data = DateTime.Now;


                    if (ModelState.IsValid)
                    {
                        db.Carts.Add(cart);
                        db.SaveChanges();

                    }
                }
            }
            return RedirectToAction("Index", new { returnUrl });

            //Game game = repository.Games
            //    .FirstOrDefault(g => g.GameId == gameId);

            //if (game != null)
            //{
            //    GetCart().AddItem(game, 1);
            //}
            //return RedirectToAction("Index", new { returnUrl });
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
