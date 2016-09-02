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
using System.IO;
using OfficeOpenXml;
using PagedList;

namespace AutoPartsWebSite.Controllers
{
    public class InvoicesController : Controller
    {
        private InvoiceModel db = new InvoiceModel();

        // GET: Invoices
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NumberSortParm = String.IsNullOrEmpty(sortOrder) ? "number_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;

            var invoices = from s in db.Invoices
                           select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                invoices = invoices.Where(s => s.Number.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "number_desc":
                    invoices = invoices.OrderByDescending(s => s.Number);
                    break;
                case "Date":
                    invoices = invoices.OrderBy(s => s.Date);
                    break;
                case "date_desc":
                    invoices = invoices.OrderByDescending(s => s.Date);
                    break;
                default:
                    invoices = invoices.OrderBy(s => s.Number);
                    break;
            }
            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(invoices.ToPagedList(pageNumber, pageSize));
            //return View(db.Invoices.ToList());
        }

        // GET: Invoices/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invoice invoice = db.Invoices.Find(id);            
            if (invoice == null)
            {
                return HttpNotFound();
            }
            return View(invoice);
        }

        // GET: Invoices/Create
        public ActionResult Create()
        {
            ViewBag.UserId = User.Identity.GetUserId();
            ViewBag.Date = System.DateTime.Now.ToString("yyyy-MM-dd");


            Invoice invoice = new Invoice();          
            ViewBag.SuppliersList = from supplier in db.Suppliers
                                    select new SelectListItem { Text = supplier.Name, Value = supplier.Id.ToString() };
            return View();
        }

        // POST: Invoices/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,UserId,Data,Number,State,SupplierId,FileName")] Invoice invoice, HttpPostedFileBase upload)
        {
            ViewBag.SuppliersList = from supplier in db.Suppliers
                                    select new SelectListItem { Text = supplier.Name, Value = supplier.Id.ToString() };
            if (ModelState.IsValid)
            {
                try
                {                    
                    invoice.UserId = User.Identity.GetUserId();
                    invoice.Date = System.DateTime.Now;
                    invoice.FileName = System.IO.Path.GetFileName(upload.FileName);
                    invoice.State = 1;
                    // store data into DB
                    db.Invoices.Add(invoice);
                    db.SaveChanges();
                    // parse data from XSLT file and store it into DB
                    if (ExcelImport(invoice.Id, upload))
                    {
                        TempData["shortMessage"] = "Загружено.";
                        return RedirectToAction("Index");
                    }
                    //TempData["shortMessage"] = "Загружено.";
                    //return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    TempData["shortMessage"] = "Ошибка загрузки:" + ex.Message.ToString();
                    return View(invoice);
                }
            }
            return View(invoice);
        }

        // GET: Invoices/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invoice invoice = db.Invoices.Find(id);
            if (invoice == null)
            {
                return HttpNotFound();
            }
            ViewBag.SuppliersList = from supplier in db.Suppliers
                                    select new SelectListItem { Text = supplier.Name, Value = supplier.Id.ToString() };
            return View(invoice);
        }

        // POST: Invoices/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UserId,Data,Number,State,SupplierId,FileName")] Invoice invoice, HttpPostedFileBase upload)
        {
            ViewBag.SuppliersList = from supplier in db.Suppliers
                                    select new SelectListItem { Text = supplier.Name, Value = supplier.Id.ToString() };
            if (ModelState.IsValid)
            {
                invoice.UserId = User.Identity.GetUserId();
                invoice.Date = System.DateTime.Now;
                if (upload != null && upload.ContentLength > 0)
                {
                    invoice.FileName = System.IO.Path.GetFileName(upload.FileName);
                }
                    if (ExcelImport(invoice.Id, upload))
                {                    
                    TempData["shortMessage"] = "Загружено.";
                    return RedirectToAction("Index");
                }
                db.Entry(invoice).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(invoice);
        }

        // GET: Invoices/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invoice invoice = db.Invoices.Find(id);
            if (invoice == null)
            {
                return HttpNotFound();
            }
            return View(invoice);
        }

        // POST: Invoices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Invoice invoice = db.Invoices.Find(id);
            db.Invoices.Remove(invoice);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        
        private bool ExcelImport(int invoiceId, HttpPostedFileBase upload)
        {
            int firstDataRow = 4;
            try
            {
                if (upload != null && upload.ContentLength > 0)
                {
                    // clear the table InvoiceItem by InvoiceId
                    var all = from c in db.InvoiceItems where c.InvoiceId.Equals(invoiceId) select c;
                    db.InvoiceItems.RemoveRange(all);
                    // load from stream
                    using (ExcelPackage package = new ExcelPackage(upload.InputStream))
                    {
                        ExcelWorksheet worksheet = package.Workbook.Worksheets.FirstOrDefault();
                        for (int i = firstDataRow; i <= worksheet.Dimension.End.Row; i++)
                        {
                            InvoiceItem invoiceItem = new InvoiceItem
                            {
                                InvoiceId = invoiceId,
                                Date = System.DateTime.Now,
                                Number = worksheet.Cells["A" + i.ToString()].Value.ToString(),
                                Quantity = worksheet.Cells["B" + i.ToString()].Value.ToString()
                            };
                            db.InvoiceItems.Add(invoiceItem);
                        }
                    }
                    db.SaveChanges();
                    ViewBag.Message = "Загружено.";
                    return true;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Ошибка загрузки:" + ex.Message.ToString();

                return false;
            }
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
