using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AutoPartsWebSite.Models;
using System.IO;
using Microsoft.AspNet.Identity;
using IdentityAutoPart.Models;
using Microsoft.AspNet.Identity.Owin;

namespace AutoPartsWebSite.Controllers
{
    public class PartsController : Controller
    {
        private PartModel db = new PartModel();

        // GET: Parts
        public ActionResult Index()
        {
            return View(db.Parts.Take(100).ToList());
        }

        // GET: Parts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Part part = db.Parts.Find(id);
            if (part == null)
            {
                return HttpNotFound();
            }
            return View(part);
        }

        // GET: Parts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Parts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ImportId,Brand,Number,Name,Details,Size,Weight,Quantity,Price,Supplier,DeliveryTime")] Part part)
        {
            if (ModelState.IsValid)
            {
                db.Parts.Add(part);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(part);
        }

        // GET: Parts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Part part = db.Parts.Find(id);
            if (part == null)
            {
                return HttpNotFound();
            }
            return View(part);
        }

        // POST: Parts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ImportId,Brand,Number,Name,Details,Size,Weight,Quantity,Price,Supplier,DeliveryTime")] Part part)
        {
            if (ModelState.IsValid)
            {
                db.Entry(part).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(part);
        }

        // GET: Parts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Part part = db.Parts.Find(id);
            if (part == null)
            {
                return HttpNotFound();
            }
            return View(part);
        }

        // POST: Parts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Part part = db.Parts.Find(id);
            db.Parts.Remove(part);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult SearchPart(string autopartNumber)
        {
            var autoparts = (from s in db.Parts
                             select s).Take(0);
            if (!String.IsNullOrEmpty(autopartNumber))
            {
                autoparts = (from s in db.Parts
                                 select s).Take(100);
                autoparts = autoparts.Where(c => c.Number.Contains(autopartNumber));
            }
            return View(autoparts);
        }

        //[Authorize(Roles = "RegistredUser")]
        public ActionResult SearchParts(string autopartNumbers) //, int? maxItemCount)
        {
            string[] autopartNumbersList = new string[] { };
            int maxItemCount = GetSearchLimit(); // get info from db about curren user search limit
            ViewBag.SearchLimit = maxItemCount;
            var autoparts = (from s in db.Parts
                             select s).Take(0);

            if (!String.IsNullOrEmpty(autopartNumbers))
            {
                // string txt = TextBox1.Text;
                string[] delimiter = { Environment.NewLine }; //new Char[] { '\n', '\r' }
                autopartNumbersList = autopartNumbers.Split(delimiter, StringSplitOptions.RemoveEmptyEntries); // StringSplitOptions.None

                var numbersList = new List<string>(autopartNumbersList);

                // two digits for search item message
                ViewBag.SearchLimit = maxItemCount;
                ViewBag.ItemsToSearch = numbersList.Count;

                if (maxItemCount < numbersList.Count)
                {
                    numbersList.RemoveRange((int)maxItemCount, numbersList.Count()- (int)maxItemCount);
                }                
                autopartNumbersList = numbersList.ToArray();

                //foreach (string autopartNumber in autopartNumbersList)
                //{
                //    if (!String.IsNullOrEmpty(autopartNumber))                 
                //    {
                //        autoparts = autoparts.Where(c => c.Number.Contains(autopartNumbersList));                       
                //    }
                //}


                //assemble an array of ID values
                //int[] customerIds = new int[] { 1, 2, 3 };
                //string autopartNumbersString = autopartNumbersList.ToString(", ");

                autoparts = (from s in db.Parts
                             where autopartNumbersList.Contains(s.Number)
                             select s).Take(1000);

                //autoparts = autoparts.Where(c => c.Number.Contains(autopartNumbersList));

            }
            foreach (Part part in autoparts)
            {
                part.Price = CalcUserPrice(part.Id);
            }
            //Session["AutopartsSearchResult"] = autoparts; //.ToList();
            Session["AutopartNumbersList"] = autopartNumbersList;            
            return View(autoparts);
        }


        public ActionResult Excel()
        {
            string[] autopartNumbersList = (string[])Session["AutopartNumbersList"];
            var autoparts = (from s in db.Parts
                             where autopartNumbersList.Contains(s.Number)
                             select s).Take(1000);
                             //select new { s.Id, s.Brand, s.Name, s.Details, s.Size, s.Weight, s.Quantity, s.Price, s.Supplier, s.DeliveryTime}).Take(1000);
            foreach (Part part in autoparts)
            {
                part.Price = CalcUserPrice(part.Id);
            }
            using (ExcelPackage pck = new ExcelPackage())
            {
                ExcelWorksheet ws = pck.Workbook.Worksheets.Add("ALFAPARTS-SearchResult");
                ws.Cells["A1"].LoadFromCollection(autoparts, true);
                // Загружаю коллекцию  "autoparts"
                // ToDo: еще нужно будет добавить русские хидеры
                Byte[] fileBytes = pck.GetAsByteArray();
                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=ALFAPARTS.xlsx");
                // Заменяю имя выходного Эксель файла

                Response.Charset = "";
                Response.ContentType = "application/vnd.ms-excel";
                StringWriter sw = new StringWriter();
                Response.BinaryWrite(fileBytes);
                Response.End();
            }

            return RedirectToAction("Index");
        }

        public int GetSearchLimit()
        {
            string currentUserId = User.Identity.GetUserId();
            ApplicationUserManager UserManager = HttpContext.GetOwinContext()
                                           .GetUserManager<ApplicationUserManager>();
            var user = UserManager.FindById(currentUserId);
            if (user == null)
            {
                return 1;
            }
            return user.SearchLimit;
        }

        public string CalcUserPrice(int PartId)
        {
            decimal defaultRate = 10;

            Part part = db.Parts.Find(PartId);
            if (part == null) // if part not exists - return 0
            {
                return "0";
            }

            Supplier supplier = db.Suppliers.Find(part.SupplierId);
            if (supplier == null)  // if supplier not exists - return price + defaultRate
            {
                return ((100 + defaultRate) * Convert.ToDecimal(part.Price)/100).ToString();
            }

            string currentUserId = User.Identity.GetUserId();
            ApplicationUserManager UserManager = HttpContext.GetOwinContext()
                                           .GetUserManager<ApplicationUserManager>();
            var user = UserManager.FindById(currentUserId);
            if (user == null)  // if not defined user - return price + SuppliersRate
            {
                return ((100 + supplier.Rate) * Convert.ToDecimal(part.Price) / 100).ToString(); 
            }

            var rate = (from r in db.Rates
                where r.UserId.Equals(currentUserId) && r.SupplierId.Equals(part.SupplierId) 
                select r).FirstOrDefault();
            if (rate == null)  // if rate not exists - return price +  SuppliersRate
            {
                return ((100 + supplier.Rate) * Convert.ToDecimal(part.Price) / 100).ToString();
            }

            return ((100 + rate.Value) * Convert.ToDecimal(part.Price) / 100).ToString();
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
