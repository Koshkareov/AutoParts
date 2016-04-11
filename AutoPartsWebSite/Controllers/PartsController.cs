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
        public ActionResult SearchParts(string autopartNumbers)
        {
            string[] autopartNumbersList = new string[] { };
            var autoparts = (from s in db.Parts
                             select s).Take(100);

            if (!String.IsNullOrEmpty(autopartNumbers))
            {
                // string txt = TextBox1.Text;
                string[] delimiter = { Environment.NewLine }; //new Char[] { '\n', '\r' }
                autopartNumbersList = autopartNumbers.Split(delimiter, StringSplitOptions.RemoveEmptyEntries); // StringSplitOptions.None

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


            using (ExcelPackage pck = new ExcelPackage())
            {
                ExcelWorksheet ws = pck.Workbook.Worksheets.Add("AutoParts");
                ws.Cells["A1"].LoadFromCollection(autoparts, true);
                // Загружаю коллекцию  "autoparts"
                // ToDo: еще нужно будет добавить русские хидеры
                Byte[] fileBytes = pck.GetAsByteArray();
                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=AutoParts.xlsx");
                // Заменяю имя выходного Эксель файла

                Response.Charset = "";
                Response.ContentType = "application/vnd.ms-excel";
                StringWriter sw = new StringWriter();
                Response.BinaryWrite(fileBytes);
                Response.End();
            }

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
