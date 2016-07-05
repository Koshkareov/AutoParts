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
using OfficeOpenXml;
using System.IO;


namespace AutoPartsWebSite.Controllers
{
    public class ImportsController : Controller
    {
        private ImportModel db = new ImportModel();

        // GET: Imports
        public ActionResult Index()
        {
            return View(db.Imports.ToList());
        }

        // GET: Imports/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Import import = db.Imports.Find(id);
            if (import == null)
            {
                return HttpNotFound();
            }
            return View(import);
        }

        // GET: Imports/Create
        public ActionResult Create()
        {
            ViewBag.UserId = User.Identity.GetUserId();
            ViewBag.Data = System.DateTime.Now.ToString("dd-MM-yyyy");

           
            Import import = new Import();
            ViewBag.SuppliersList = from supplier in db.Suppliers
                               select new SelectListItem { Text = supplier.Name, Value = supplier.Id.ToString() };
            //import.Suppliers = from supplier in db.Suppliers
            //                   select new SelectListItem { Text = supplier.Name, Value = supplier.Id.ToString() };                     
            return View(import);           
        }

        // POST: Imports/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Date,UserId,SupplierId,DeliveryTime,FileName")] Import import, HttpPostedFileBase upload)
        {                  
            if (ModelState.IsValid)
            {
                if (upload != null && upload.ContentLength > 0)
                {
                    try
                    {
                        // get filename
                        string fileName = System.IO.Path.GetFileName(upload.FileName);
                        // save file into ImportFiles folder                       
                        upload.SaveAs(Server.MapPath("~/ImportFiles/" + fileName));
                        import.FileName = fileName;
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Message = "ERROR:" + ex.Message.ToString();
                    }
                    
                }
                import.UserId = User.Identity.GetUserId();
                import.Date = System.DateTime.Now;
                // store data into DB
                db.Imports.Add(import);
                db.SaveChanges();
                // toDo: parse data from XSLT file and store it into DB
                if (LoadImportData(import.Id))
                {
                    return RedirectToAction("Index");
                }                
            }            
            return View(import);
        }

        // GET: Imports/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Import import = db.Imports.Find(id);
            if (import == null)
            {
                return HttpNotFound();
            }
            return View(import);
        }

        // POST: Imports/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Date,UserId,SupplierId,DeliveryTime,FileName")] Import import)
        {
            if (ModelState.IsValid)
            {
                db.Entry(import).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(import);
        }

        // GET: Imports/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Import import = db.Imports.Find(id);
            if (import == null)
            {
                return HttpNotFound();
            }
            return View(import);
        }

        // POST: Imports/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Import import = db.Imports.Find(id);
            db.Imports.Remove(import);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public List<SelectListItem> GetSuplliersList()
        {
            var suppliers = (from s in db.Suppliers
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
        private bool LoadImportData(int ImportId)
        {
            try
            {
                int firstDataRow = 3;
            Import import = db.Imports.Find(ImportId);
            FileInfo autopartsFile = new FileInfo(Server.MapPath("~/ImportFiles/" + import.FileName));
            
            using (ExcelPackage package = new ExcelPackage(autopartsFile))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.FirstOrDefault();
                for (int i = firstDataRow; i < worksheet.Dimension.End.Row; i++)
                {
                    Part part = new Part
                    {
                        ImportId = import.Id,
                        Brand = worksheet.Cells["A" + i.ToString()].Value.ToString(),
                        Number = worksheet.Cells["B" + i.ToString()].Value.ToString(),
                        Name = worksheet.Cells["D" + i.ToString()].Value.ToString(),
                        Details = worksheet.Cells["C" + i.ToString()].Value.ToString(),
                        Size = worksheet.Cells["F" + i.ToString()].Value.ToString(),
                        Weight = worksheet.Cells["E" + i.ToString()].Value.ToString(),
                        Quantity = worksheet.Cells["J" + i.ToString()].Value.ToString(),
                        Price = worksheet.Cells["H" + i.ToString()].Value.ToString(),
                        SupplierId = Convert.ToInt32(import.SupplierId)
                    };
                    db.Parts.Add(part);
                }
            }
            db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Ошибка:" + ex.Message.ToString();
                return false;
            }
        }


        private bool LoadImportDataNew(int ImportId)
        {
            int firstDataRow = 3;
            Import import = db.Imports.Find(ImportId);
            FileInfo autopartsFile = new FileInfo(Server.MapPath("~/ImportFiles/" + import.FileName));

            ExcelPackage package = new ExcelPackage();
            
            package.Load(autopartsFile.OpenRead());
            
            //using (ExcelPackage package = new ExcelPackage(autopartsFile))
            { 
                ExcelWorksheet worksheet = package.Workbook.Worksheets.FirstOrDefault();
                for (int i = firstDataRow; i < worksheet.Dimension.End.Row; i++)
                {
                    Part part = new Part
                    {
                        ImportId = import.Id,
                        Brand = worksheet.Cells["A" + i.ToString()].Value.ToString(),
                        Number = worksheet.Cells["B" + i.ToString()].Value.ToString(),
                        Name = worksheet.Cells["D" + i.ToString()].Value.ToString(),
                        Details = worksheet.Cells["C" + i.ToString()].Value.ToString(),
                        Size = worksheet.Cells["F" + i.ToString()].Value.ToString(),
                        Weight = worksheet.Cells["E" + i.ToString()].Value.ToString(),
                        Quantity = worksheet.Cells["J" + i.ToString()].Value.ToString(),
                        Price = worksheet.Cells["H" + i.ToString()].Value.ToString(),
                        SupplierId = Convert.ToInt32(import.SupplierId)
                    };
                    db.Parts.Add(part);
                }
            }
            db.SaveChanges();
            return true;
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
