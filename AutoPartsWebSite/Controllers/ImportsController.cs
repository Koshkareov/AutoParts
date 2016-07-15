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
            ViewBag.SuppliersList = from supplier in db.Suppliers
                                    select new SelectListItem { Text = supplier.Name, Value = supplier.Id.ToString() };
            if (ModelState.IsValid)
            {
                try
                {
                    if (upload != null && upload.ContentLength > 0)
                    {                        
                        // get filename
                        string fileName = System.IO.Path.GetFileName(upload.FileName);
                        // save file into ImportFiles folder                       
                        upload.SaveAs(Server.MapPath("~/ImportFiles/" + fileName));
                        import.FileName = fileName;                                            
                    }
                    import.UserId = User.Identity.GetUserId();
                    import.Date = System.DateTime.Now;
                    // store data into DB
                    db.Imports.Add(import);
                    db.SaveChanges();
                    // toDo: parse data from XSLT file and store it into DB
                    if (LoadImportDataTEST(import.Id))
                    {
                        ViewBag.Message = "Импорт завершен.";
                        return RedirectToAction("Index");                     
                    }                
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "Ошибка импорта:" + ex.Message.ToString();
                    
                    return View(import);
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
        public async System.Threading.Tasks.Task<ActionResult> DeleteConfirmed(int id)
        {
            // Delete imported Parts by ImportId
            db.Parts.RemoveRange(db.Parts.Where(x => x.ImportId == id));

            // Delete Import by Id
            Import import = db.Imports.Find(id);
            db.Imports.Remove(import);
            await db.SaveChangesAsync();
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
                Import import = db.Imports.Find(ImportId);
                Supplier supplier = db.Suppliers.Find(import.SupplierId);
                ImportTemplate importTemplate = db.ImportTemplates.Find(supplier.ImportTemplateId);
                FileInfo autopartsFile = new FileInfo(Server.MapPath("~/ImportFiles/" + import.FileName));
                int firstDataRow = importTemplate.StartRow;

                using (ExcelPackage package = new ExcelPackage(autopartsFile))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.FirstOrDefault();
                    for (int i = firstDataRow; i < worksheet.Dimension.End.Row; i++)
                    {
                        Part part = new Part
                        {
                            ImportId = import.Id,
                            Brand = worksheet.Cells[importTemplate.BrandColumn + i.ToString()].Value.ToString(),
                            Number = worksheet.Cells[importTemplate.NumberColumn + i.ToString()].Value.ToString(),
                            Name = worksheet.Cells[importTemplate.NameColumn + i.ToString()].Value.ToString(),
                            Details = worksheet.Cells[importTemplate.DetailsColumn + i.ToString()].Value.ToString(),
                            Size = worksheet.Cells[importTemplate.SizeColumn + i.ToString()].Value.ToString(),
                            Weight = worksheet.Cells[importTemplate.WeightColumn + i.ToString()].Value.ToString(),
                            Quantity = worksheet.Cells[importTemplate.QuantityColumn + i.ToString()].Value.ToString(),
                            Price = worksheet.Cells[importTemplate.PriceColumn + i.ToString()].Value.ToString(),
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


        private bool LoadImportDataTEST(int ImportId)
        {
            try
            {                
                Import import = db.Imports.Find(ImportId);
                int importId = import.Id;
                int supplierId = Convert.ToInt32(import.SupplierId);
                int linesNumber = 0;

                Supplier supplier = db.Suppliers.Find(import.SupplierId);
                ImportTemplate importTemplate = db.ImportTemplates.Find(supplier.ImportTemplateId);                
                int firstDataRow = importTemplate.StartRow;

                FileInfo autopartsFile = new FileInfo(Server.MapPath("~/ImportFiles/" + import.FileName));

                using (ExcelPackage package = new ExcelPackage(autopartsFile))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.FirstOrDefault();
                    int maxRow = worksheet.Dimension.End.Row;
                    Dictionary<string, string> dicPart = new Dictionary<string, string>();
                    for (int i = firstDataRow; i < worksheet.Dimension.End.Row; i++)
                    {
                        dicPart.Clear();
                        dicPart.Add("Id", Convert.ToString(importId));   // import ID                        
                        dicPart.Add("Brand", Convert.ToString(worksheet.Cells[importTemplate.BrandColumn + i.ToString()].Value));   //  Brand = 1
                        dicPart.Add("Number", Convert.ToString(worksheet.Cells[importTemplate.NumberColumn + i.ToString()].Value));   //  Number = 2
                        dicPart.Add("Name", Convert.ToString(worksheet.Cells[importTemplate.NameColumn + i.ToString()].Value));   //  Name = 4
                        dicPart.Add("Details", Convert.ToString(worksheet.Cells[importTemplate.DetailsColumn + i.ToString()].Value));   //  Details = 3
                        dicPart.Add("Size", Convert.ToString(worksheet.Cells[importTemplate.SizeColumn + i.ToString()].Value));   //  Size = 6
                        dicPart.Add("Weight", Convert.ToString(worksheet.Cells[importTemplate.WeightColumn + i.ToString()].Value));   //  Weight = 5
                        dicPart.Add("Quantity", Convert.ToString(worksheet.Cells[importTemplate.QuantityColumn + i.ToString()].Value));   //  Quantity = 10
                        dicPart.Add("Price", Convert.ToString(worksheet.Cells[importTemplate.PriceColumn + i.ToString()].Value));   //  Price = 8                        
                        dicPart.Add("SupplierId", Convert.ToString(supplierId));   //  SupplierId = 7                        
                
                        AddPartData(ref dicPart);
                        linesNumber++;
                    }
                }
                ViewBag.Message = "Импорт завершен.";
                import.LinesNumber = linesNumber;
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Ошибка импорта:" + ex.Message.ToString();               
                return false;
            }
        }

        public static void AddPartData(ref Dictionary<string, string> dicPartData)
        {
            using (PartModel db_Parts = new PartModel())
            {
                var autopart = new Part
                {
                    ImportId = Convert.ToInt32(dicPartData["Id"]),
                    Brand = dicPartData["Brand"],
                    Number = dicPartData["Number"],
                    Name = dicPartData["Name"],
                    Details = dicPartData["Details"],
                    Size = dicPartData["Size"],
                    Weight = dicPartData["Weight"],
                    Quantity = dicPartData["Quantity"],
                    Price = dicPartData["Price"],
                    SupplierId = Convert.ToInt32(dicPartData["SupplierId"])
                };

                db_Parts.Parts.Add(autopart);
                db_Parts.SaveChanges();
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
