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
                string[] delimiters = { Environment.NewLine, ".", ",", ";", " " }; //new Char[] { '\n', '\r' }
                autopartNumbersList = autopartNumbers.Split(delimiters, StringSplitOptions.RemoveEmptyEntries); // StringSplitOptions.None

                var numbersList = new List<string>(autopartNumbersList);

                // two digits for search item message
                ViewBag.SearchLimit = maxItemCount;
                ViewBag.ItemsToSearch = numbersList.Count;

                if (maxItemCount < numbersList.Count)
                {
                    numbersList.RemoveRange((int)maxItemCount, numbersList.Count() - (int)maxItemCount);
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
                part.Quantity = CalcUserQuantity(part.Id);
            }
            //Session["AutopartsSearchResult"] = autoparts; //.ToList();
            Session["AutopartNumbersList"] = autopartNumbersList;
            return View(autoparts);
        }

        //[Authorize(Roles = "RegistredUser")]
        public ActionResult Search(string autopartNumbers) 
        {
            string[] autopartNumbersList = new string[] { };
            int maxItemCount = GetSearchLimit(); // get info from db about curren user search limit
            ViewBag.SearchLimit = maxItemCount;

            if (String.IsNullOrEmpty(autopartNumbers))
            {
                TempData["shortMessage"] = "Данных не обнаружено, уточните запрос."; //"Тут рыбы нет !";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                string[] delimiters = { Environment.NewLine, ".", ",", ";", " " }; //new Char[] { '\n', '\r' }
                autopartNumbersList = autopartNumbers.Split(delimiters, StringSplitOptions.RemoveEmptyEntries); // StringSplitOptions.None

                var numbersList = new List<string>(autopartNumbersList);

                // two digits for search item message
                ViewBag.SearchLimit = maxItemCount;
                ViewBag.ItemsToSearch = numbersList.Count;

                if (maxItemCount < numbersList.Count)
                {
                    numbersList.RemoveRange((int)maxItemCount, numbersList.Count() - (int)maxItemCount);
                }
                // find and add replacement to numbers list
                var autopartsReplacement = (from s in db.PartReplacement
                                            where numbersList.Contains(s.Number)
                                            select s.Replacement).ToList();
                numbersList.AddRange(autopartsReplacement);
                // create array from list
                autopartNumbersList = numbersList.ToArray();

                // read about ".Select(x => new Part"  by link  
                // http://stackoverflow.com/questions/5325797/the-entity-cannot-be-constructed-in-a-linq-to-entities-query
                var autoparts = (from p in db.Parts
                                 join a in db.PartAliases on p.Number equals a.Number into ps
                                 from a in ps.DefaultIfEmpty()
                                 where autopartNumbersList.Contains(p.Number)
                                 //select p
                                 select new 
                                 {
                                     Id = p.Id,
                                     ImportId = p.ImportId,
                                     Brand = p.Brand,
                                     Number = p.Number,
                                     Name = !string.IsNullOrEmpty(a.Name) ? a.Name : p.Name,
                                     Details = p.Details,
                                     Size = !string.IsNullOrEmpty(a.Size) ? a.Size : p.Size,
                                     Weight = !string.IsNullOrEmpty(a.Weight) ? a.Weight : p.Weight,
                                     Quantity = p.Quantity,
                                     Price = p.Price,
                                     //Supplier = p.Supplier,
                                     //DeliveryTime = p.DeliveryTime,
                                     SupplierId = p.SupplierId
                                 }
                                 ).ToList()
                                 .Select(x => new Part
                                 {
                                     Id = x.Id,
                                     ImportId = x.ImportId,
                                     Brand = x.Brand,
                                     Number = x.Number,
                                     Name = x.Name,
                                     Details = x.Details,
                                     Size = x.Size,
                                     Weight = x.Weight,
                                     Quantity = x.Quantity,
                                     Price = x.Price,
                                     //Supplier = x.Supplier,
                                     //DeliveryTime = x.DeliveryTime,
                                     SupplierId = x.SupplierId
                                 }).Take(1000); 
                if (autoparts.Count() <= 0) // not found any numbers
                {
                    TempData["shortMessage"] = "Данных не обнаружено, уточните запрос."; //"Тут рыбы нет !";
                    return RedirectToAction("Index", "Home");
                }
                    
                foreach (Part part in autoparts)
                {
                    part.Price = CalcUserPrice(part.Id);
                    part.Quantity = CalcUserQuantity(part.Id);
                }

                Session["AutopartNumbersList"] = autopartNumbersList;                
                return View(autoparts);
            }
        }

        private ActionResult SearchTEMP(string autopartNumbers) //, int? maxItemCount)
        {
            string[] autopartNumbersList = new string[] { };
            int maxItemCount = GetSearchLimit(); // get info from db about curren user search limit
            ViewBag.SearchLimit = maxItemCount;
            //var autoparts = (IQueryable<Part>)(from p in db.Parts
            //                 select new
            //                 {
            //                     p.Id,
            //                     p.ImportId,
            //                     p.Brand,
            //                     p.Number,
            //                     p.Name,
            //                     p.Details,
            //                     p.Size,
            //                     p.Weight,
            //                     p.Quantity,
            //                     p.Price,
            //                     p.Supplier,
            //                     p.DeliveryTime,
            //                     p.SupplierId
            //                 }
            //                 ).Take(0);

            //var autoparts = (from p in db.Parts
            //                 select p).Take(0);

            if (!String.IsNullOrEmpty(autopartNumbers))
            {
                // string txt = TextBox1.Text;
                string[] delimiters = { Environment.NewLine, ".", ",", ";", " " }; //new Char[] { '\n', '\r' }
                autopartNumbersList = autopartNumbers.Split(delimiters, StringSplitOptions.RemoveEmptyEntries); // StringSplitOptions.None

                var numbersList = new List<string>(autopartNumbersList);

                // two digits for search item message
                ViewBag.SearchLimit = maxItemCount;
                ViewBag.ItemsToSearch = numbersList.Count;

                if (maxItemCount < numbersList.Count)
                {
                    numbersList.RemoveRange((int)maxItemCount, numbersList.Count() - (int)maxItemCount);
                }
                // find and add replacement to numbers list
                var autopartsReplacement = (from s in db.PartReplacement
                                            where numbersList.Contains(s.Number)
                                            select s.Replacement).ToList();
                numbersList.AddRange(autopartsReplacement);

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

                //autoparts = autoparts.Where(c => c.Number.Contains(autopartNumbersList));

                //autoparts = (from s in db.Parts
                //             where autopartNumbersList.Contains(s.Number)
                //             select s).Take(1000);

                var autoparts = (IEnumerable<AutoPartsWebSite.Models.Part>)(from p in db.Parts
                                                                            join a in db.PartAliases on p.Number equals a.Number into ps
                                                                            from a in ps.DefaultIfEmpty()
                                                                            where autopartNumbersList.Contains(p.Number)
                                                                            select new
                                                                            {
                                                                                p.Id,
                                                                                p.ImportId,
                                                                                p.Brand,
                                                                                p.Number,
                                                                                p.Name,
                                                                                p.Details,
                                                                                p.Size,
                                                                                p.Weight,
                                                                                p.Quantity,
                                                                                p.Price,
                                                                                p.Supplier,
                                                                                p.DeliveryTime,
                                                                                p.SupplierId
                                                                            }
                             ).Take(1000);

                foreach (Part part in autoparts)
                {
                    part.Price = CalcUserPrice(part.Id);
                    part.Quantity = CalcUserQuantity(part.Id);
                }
                //Session["AutopartsSearchResult"] = autoparts; //.ToList();
                Session["AutopartNumbersList"] = autopartNumbersList;
                return View(autoparts);
            }
            return View();
        }


        public ActionResult Excel()
        {
            string[] autopartNumbersList = (string[])Session["AutopartNumbersList"];
            //var autoparts = (from s in db.Parts
            //                 where autopartNumbersList.Contains(s.Number)
            //                 select s).Take(1000);
            var autoparts = (from p in db.Parts
                             join a in db.PartAliases on p.Number equals a.Number into ps
                             from a in ps.DefaultIfEmpty()
                             where autopartNumbersList.Contains(p.Number)
                             //select p
                             select new
                             {
                                 Id = p.Id,
                                 ImportId = p.ImportId,
                                 Brand = p.Brand,
                                 Number = p.Number,
                                 Name = !string.IsNullOrEmpty(a.Name) ? a.Name : p.Name,
                                 Details = p.Details,
                                 Size = !string.IsNullOrEmpty(a.Size) ? a.Size : p.Size,
                                 Weight = !string.IsNullOrEmpty(a.Weight) ? a.Weight : p.Weight,
                                 Quantity = p.Quantity,
                                 Price = p.Price,
                                 //Supplier = p.Supplier,
                                 //DeliveryTime = p.DeliveryTime,
                                 SupplierId = p.SupplierId
                             }
                                 ).ToList()
                                 .Select(x => new Part
                                 {
                                     Id = x.Id,
                                     ImportId = x.ImportId,
                                     Brand = x.Brand,
                                     Number = x.Number,
                                     Name = x.Name,
                                     Details = x.Details,
                                     Size = x.Size,
                                     Weight = x.Weight,
                                     Quantity = x.Quantity,
                                     Price = x.Price,
                                     //Supplier = x.Supplier,
                                     //DeliveryTime = x.DeliveryTime,
                                     SupplierId = x.SupplierId
                                 }).Take(1000);

            foreach (Part part in autoparts)
            {
                part.Price = CalcUserPrice(part.Id);
                part.Quantity = CalcUserQuantity(part.Id);
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

        private int GetSearchLimit()
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

        private string CalcUserPrice(int PartId)
        {
            decimal defaultRate = 10;

            // Random rnd = new Random();
            Random rnd = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);
            decimal anRegistredUserRate = rnd.Next(5, 8); // issue #24


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
                // return ((100 + supplier.Rate) * Convert.ToDecimal(part.Price) / 100).ToString();
                return ((100 + anRegistredUserRate) * Convert.ToDecimal(part.Price) / 100).ToString(); 
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

        private string CalcUserQuantity(int PartId)
        {
            Part part = db.Parts.Find(PartId);
            if (part == null) // if part not exists - return 0
            {
                return "0";
            }
            if (Convert.ToInt32(part.Quantity) > 10)
            {
                int operand1 = Convert.ToInt32(part.Quantity);
                int operand2 = Convert.ToInt32(System.Math.Pow(10, (part.Quantity.Length - 1)));                
                return ((operand1 / operand2) * operand2).ToString();
            }
            if (Convert.ToInt32(part.Quantity) > 5)
            {
                return "5";
            }
            if (Convert.ToInt32(part.Quantity) <= 5)
            {
                return part.Quantity;
            }
            return "0"; //something going wrong
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
