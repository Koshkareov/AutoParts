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
using System.Threading.Tasks;
using Postal;

namespace AutoPartsWebSite.Controllers
{
    [Authorize(Roles = "RegistredUser")]
    public class OrdersController : Controller
    {
        private OrderModel db = new OrderModel();
        private CartModel cartdb = new CartModel();

        // GET: Orders
        public ActionResult Index()
        {
            string currentUserId = User.Identity.GetUserId();
            

            return View(GetUserOrders(currentUserId));
        }

        //public ActionResult Index()
        //{
        //    return View(db.Orders.ToList());
        //}

        // GET: Orders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
           // TempData["Order"] = id;
            // return RedirectToAction("Index", "OrderItems");
            return View(order);
        }

        // GET: Orders/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,UserId,Summary,Data,State")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Orders.Add(order);
                db.SaveChanges();
                
                return RedirectToAction("Index");
            }

            return View(order);
        }
              
        // GET: Orders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UserId,Summary,Data,State")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(order);
        }

        // GET: Orders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = db.Orders.Find(id);
            db.Orders.Remove(order);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public List<Order> GetUserOrders(string id)
        {
            var userOrders = (from s in db.Orders
                               select s).Take(1000);
            userOrders = userOrders.Where(s => s.UserId.Equals(id));
            return userOrders.ToList();
        }

       public ActionResult CreateUserOrder()
        {
            string currentUserId = User.Identity.GetUserId();            
            var userCart = (from s in cartdb.Carts
                            select s).Take(1000);
            userCart = userCart.Where(s => s.UserId.Equals(currentUserId));
            var orderItems = new List<OrderItem> { };

            Order order = new Order();
            order.UserId = currentUserId;            
            order.Data = DateTime.Now;
            order.State = 1; // new added Order           
            db.Orders.Add(order);

            foreach (Cart cartItem in userCart)
            {
                var orderItem = new OrderItem
                {
                    PartId = cartItem.PartId,
                    UserId = cartItem.UserId,
                    Brand = cartItem.Brand,
                    Number = cartItem.Number,
                    Name = cartItem.Name,
                    Details = cartItem.Details,
                    Size = cartItem.Size,
                    Weight = cartItem.Weight,
                    Quantity = cartItem.Quantity,
                    Price = cartItem.Price,
                    Supplier = cartItem.Supplier,
                    DeliveryTime = cartItem.DeliveryTime,
                    Amount = cartItem.Amount,
                    Data = cartItem.Data,
                    State = 1, // new added orderItem
                    OrderId = order.Id,
                    BasePrice = cartItem.BasePrice,
                    Reference1 = cartItem.Reference1,
                    Reference2 = cartItem.Reference2
                };
                //db.OrderItems.Add(orderItem);
                orderItems.Add(orderItem);                
            }
            // add order items into DB
            orderItems.ForEach(s => db.OrderItems.Add(s));
            // Clear user Cart
            userCart.ToList().ForEach(s => cartdb.Carts.Remove(s));

            order.Summary = orderItems.Sum(x => x.Total);
            // order.State = orderItems.Count();

            if (ModelState.IsValid)
            {                
                db.SaveChanges();
                cartdb.SaveChanges();
                SendEmail(order); // send notifications
                return RedirectToAction("Index");
            }

            return View(order);
        }

        public void SendEmail(Order neworder)
        {
            var user = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(neworder.UserId);

            // send new order e-mail to admin
            dynamic adminNewOrder = new Email("adminNewOrder");
            adminNewOrder.To = "podovzhniy@gmail.com"; // "admins@alfa-parts.com";
            adminNewOrder.Order = neworder.Id;
            adminNewOrder.Send();

            // send new order e-mail to user
            dynamic userNewOrder = new Email("userNewOrder");
            userNewOrder.To = user.Email;
            adminNewOrder.Order = neworder;
            userNewOrder.Send();
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
