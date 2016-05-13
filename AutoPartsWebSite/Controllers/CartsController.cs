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
using IdentityAutoPart.Models;
using Microsoft.AspNet.Identity.Owin;

namespace AutoPartsWebSite.Controllers
{
    public class CartSummaryResult
    {
        public string Items { get; set; }
        public string Cart { get; set; }
        public string Limit { get; set; }
        public string Payments { get; set; }
        public string Orders { get; set; }
        public string Balans { get; set; }
    }

    [Authorize(Roles = "RegistredUser")]
    public class CartsController : Controller
    {
        private CartModel db = new CartModel();

        //public decimal GetTotal()
        //{
        //    string currentUserId = User.Identity.GetUserId();
        //    var total = (from cartItems in db.Carts
        //                      where cartItems.UserId.Equals(currentUserId)
        //                      select (Convert.ToDecimal(cartItems.Amount) * Convert.ToDecimal(cartItems.Price)))
        //                      .Sum();

        //    return total; // ?? decimal.Zero;
        //}

        public int GetCount()
        {
            string currentUserId = User.Identity.GetUserId();
            int? count = (from cartItems in db.Carts
                          where cartItems.UserId == currentUserId
                          select (int?)cartItems.Amount).Sum();
            return count ?? 0;
        }

        public decimal GetTotal()
        {
            string currentUserId = User.Identity.GetUserId();
            //var total  = (from cartItems in db.Carts
            //             where cartItems.UserId.Equals(currentUserId)
            //             select (Convert.ToDecimal(cartItems.Amount) * Convert.ToDecimal(cartItems.Price)))
            //                  .Sum();
            decimal? total = 5;

            return total ?? decimal.Zero;
        }

        public decimal GetUserLimit()
        {
            string currentUserId = User.Identity.GetUserId();
            ApplicationUserManager UserManager = HttpContext.GetOwinContext()
                                           .GetUserManager<ApplicationUserManager>();
            var user = UserManager.FindById(currentUserId);
            if (user == null)
            {
                return 0;
            }
            return user.MoneyLimit;
        }

        public decimal GetUserPayments()
        {
            string currentUserId = User.Identity.GetUserId();            
            AutoPartsWebSite.Controllers.PaymentsController PaymentsCtrl = new PaymentsController();
            return Convert.ToDecimal(PaymentsCtrl.GetUserPayments(currentUserId).Sum(x => x.Amount));            
        }

        public decimal GetUserOrders()
        {
            string currentUserId = User.Identity.GetUserId();
            AutoPartsWebSite.Controllers.OrdersController OrdersCtrl = new OrdersController();
            return Convert.ToDecimal(OrdersCtrl.GetUserOrders(currentUserId).Sum(x => x.Summary));
        }

        public decimal GetUserCart()
        {
            string currentUserId = User.Identity.GetUserId();
            var userCart = (from s in db.Carts
                            select s).Take(1000);
            userCart = userCart.Where(s => s.UserId.Equals(currentUserId));
            return Convert.ToDecimal(userCart.ToList().Sum(x => x.Total));
        }

        public decimal GetUserBalans()
        {
            return GetUserPayments() + GetUserLimit() - GetUserOrders();
        }

        public bool PositiveUserBalans()
        {
            if ((GetUserPayments() + GetUserLimit() - GetUserOrders()) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool PositiveUserCartBalans()
        {
            if ((GetUserPayments() + GetUserLimit() - GetUserOrders() - GetUserCart()) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        [Authorize(Roles = "RegistredUser")]
        public PartialViewResult CartSummary()
        {
            string currentUserId = User.Identity.GetUserId();
            var userCart = (from s in db.Carts
                            select s).Take(1000);
            userCart = userCart.Where(s => s.UserId.Equals(currentUserId));
            //userCart.ToList().Sum(x => x.Total).ToString()
            CartSummaryResult resModel = new CartSummaryResult { Items = Convert.ToDecimal(userCart.ToList().Sum(x => x.Amount)).ToString("N"),
                                            Cart = Convert.ToDecimal(userCart.ToList().Sum(x => x.Total)).ToString("N"),
                                            Limit = GetUserLimit().ToString("N"),
                                            Payments = GetUserPayments().ToString("N"),
                                            Orders = GetUserOrders().ToString("N"),
                                            Balans = GetUserBalans().ToString("N") };

            return PartialView(resModel);
        }

        // GET: Carts        
        public ActionResult Index()
        {
            string currentUserId = User.Identity.GetUserId();
            var userCart = (from s in db.Carts
                            select s).Take(1000);
            userCart = userCart.Where(s => s.UserId.Equals(currentUserId));
            ViewBag.CartTotal = GetTotal();
            ViewBag.CartCount = Convert.ToString(GetCount());

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
        public ActionResult Create([Bind(Include = "Id,PartId,UserId,Brand,Number,Name,Details,Size,Weight,Quantity,Price,Supplier,DeliveryTime,Amount,Data")] Cart cart)
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
        public ActionResult Edit([Bind(Include = "Id,PartId,UserId,Brand,Number,Name,Details,Size,Weight,Quantity,Price,Supplier,DeliveryTime,Amount,Data")] Cart cart)
        {
            if (ModelState.IsValid)
            {
                cart.Data = DateTime.Now;
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

        public void AddToCartItem(int PartId, int Amount)
        {
            string currentUserId = User.Identity.GetUserId();
            var userCart = (from s in db.Carts
                             select s).Take(1000);
            userCart = userCart.Where(s => s.UserId.Equals(currentUserId));

            Part autopart = db.Parts.Find(PartId);
            Cart cartpart = userCart.Where(s => s.PartId == PartId).FirstOrDefault();
             if ((cartpart != null) && (autopart != null) && (Amount != 0))
             {
                        if (Convert.ToInt32(cartpart.Quantity) >= (cartpart.Amount + Amount))
                        {
                            cartpart.Amount = cartpart.Amount + Amount;
                            if (ModelState.IsValid)
                            {
                                cartpart.Data = DateTime.Now;
                                db.Entry(cartpart).State = EntityState.Modified;
                                db.SaveChanges();
                            }
                        }
             }
             else
             {
                        if ((autopart != null) && (Amount != 0))
                        {
                            Cart cart = new Cart();
                            cart.PartId = autopart.Id;
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
        
       }

        //public RedirectToRouteResult AddToCart(int? PartId, int? Amount, string returnUrl)
        //{            
        //    if (PartId != null)
        //    {
        //        //var autoparts = (from s in db.Parts
        //        //                 select s).Take(100);
        //        //autoparts = autoparts.Where(s => s.Id.Equals(PartId)).Take(1);
        //        Part autopart = db.Parts.Find(PartId); // (Part)autoparts;
        //        Cart cartpart = db.Carts.Where(s => s.PartId == PartId).FirstOrDefault();
        //        if ((cartpart != null) && (autopart != null) && (Amount != null) && (Amount != 0))
        //        {
        //            if (Convert.ToInt32(cartpart.Quantity) >= (cartpart.Amount + Amount))
        //            {
        //                cartpart.Amount = cartpart.Amount + Amount;
        //                if (ModelState.IsValid)
        //                {
        //                    cartpart.Data = DateTime.Now;
        //                    db.Entry(cartpart).State = EntityState.Modified;
        //                    db.SaveChanges();
        //                }
        //            }
        //        }
        //        else
        //        {
        //            if ((autopart != null) && (Amount != null) && (Amount != 0))
        //            {
        //                Cart cart = new Cart();
        //                cart.PartId = autopart.Id;
        //                cart.UserId = User.Identity.GetUserId();

        //                cart.Brand = autopart.Brand;
        //                cart.Number = autopart.Number;
        //                cart.Name = autopart.Name;
        //                cart.Details = autopart.Details;
        //                cart.Size = autopart.Size;
        //                cart.Weight = autopart.Weight;
        //                cart.Quantity = autopart.Quantity;
        //                cart.Supplier = autopart.Supplier;
        //                cart.Price = autopart.Price;
        //                cart.DeliveryTime = autopart.DeliveryTime;
        //                cart.Amount = Amount;
        //                cart.Data = DateTime.Now;


        //                if (ModelState.IsValid)
        //                {
        //                    db.Carts.Add(cart);
        //                    db.SaveChanges();
        //                }
        //            }
        //        }
        //    }
        //    return RedirectToAction("Index", new { returnUrl });

        //}
        public RedirectToRouteResult AddToCart(int? PartId, int? Amount, string returnUrl)
        {
            if ((PartId != null)  && (Amount != null))
            {
                AddToCartItem((int)PartId, (int)Amount);
            }
            return RedirectToAction("Index", new { returnUrl });

        }
                
        public RedirectToRouteResult AddToCartMulti(FormCollection form, string returnUrl)
        {

            int PartId;
            int Amount;
            List<int> listValues = new List<int>();
            foreach (string key in Request.Form.AllKeys)
            {
                if (key.StartsWith("Amount"))
                {
                    PartId = Convert.ToInt32(key.Remove(0, 6));
                    Amount = Convert.ToInt32(Request.Form[key]);
                    if(Amount != 0) 
                    {
                        AddToCartItem(PartId, Amount);
                    }
                }
            }

            return RedirectToAction("Index", new { returnUrl });

        }


        public ActionResult AddNewOrder()
        {
            string currentUserId = User.Identity.GetUserId();
            var userCart = (from s in db.Carts
                            select s).Take(1000);
            userCart = userCart.Where(s => s.UserId.Equals(currentUserId));

            // TempData["Cart"] = userCart.ToList();
            // Call CreatUserOrder action from Order controller

            return RedirectToAction("CreateUserOrder", "Orders");
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
