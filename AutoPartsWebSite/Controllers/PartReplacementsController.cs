using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AutoPartsWebSite.Models;
using PagedList;
using OfficeOpenXml;
using System.IO;

namespace AutoPartsWebSite.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PartReplacementsController : Controller
    {
        private PartModel db = new PartModel();

        // GET: PartReplacements
        //public ActionResult Index()
        //{
        //    return View(db.PartReplacement.ToList());
        //}

        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NumberSortParm = String.IsNullOrEmpty(sortOrder) ? "number_desc" : "";
            ViewBag.ReplacementSortParm = sortOrder == "Replacement" ? "replacement_desc" : "Replacement";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var partReplacements = from s in db.PartReplacement
                           select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                partReplacements = partReplacements.Where(s => s.Number.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "number_desc":
                    partReplacements = partReplacements.OrderByDescending(s => s.Number);
                    break;
                case "Replacement":
                    partReplacements = partReplacements.OrderBy(s => s.Replacement);
                    break;
                case "replacement_desc":
                    partReplacements = partReplacements.OrderByDescending(s => s.Replacement);
                    break;
                default:
                    partReplacements = partReplacements.OrderBy(s => s.Number);
                    break;
            }
            //return View(partReplacements.ToList());
            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(partReplacements.ToPagedList(pageNumber, pageSize));
        }

        // GET: PartReplacements/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PartReplacement partReplacement = db.PartReplacement.Find(id);
            if (partReplacement == null)
            {
                return HttpNotFound();
            }
            return View(partReplacement);
        }

        // GET: PartReplacements/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PartReplacements/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Number,Replacement")] PartReplacement partReplacement)
        {
            if (ModelState.IsValid)
            {
                db.PartReplacement.Add(partReplacement);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(partReplacement);
        }

        // GET: PartReplacements/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PartReplacement partReplacement = db.PartReplacement.Find(id);
            if (partReplacement == null)
            {
                return HttpNotFound();
            }
            return View(partReplacement);
        }

        // POST: PartReplacements/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Number,Replacement")] PartReplacement partReplacement)
        {
            if (ModelState.IsValid)
            {
                db.Entry(partReplacement).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(partReplacement);
        }

        // GET: PartReplacements/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PartReplacement partReplacement = db.PartReplacement.Find(id);
            if (partReplacement == null)
            {
                return HttpNotFound();
            }
            return View(partReplacement);
        }

        // POST: PartReplacements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PartReplacement partReplacement = db.PartReplacement.Find(id);
            db.PartReplacement.Remove(partReplacement);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        public ActionResult ExcelExport()
        {            
            var partReplacements = from s in db.PartReplacement
                             select new { s.Number, s.Replacement };            
            using (ExcelPackage pck = new ExcelPackage())
            {
                ExcelWorksheet ws = pck.Workbook.Worksheets.Add("ALFAPARTS-PartReplacement");
                ws.Cells["A1"].LoadFromCollection(partReplacements, true);
                // Загружаю коллекцию  "partReplacements"
                // ToDo: еще нужно будет добавить русские хидеры
                Byte[] fileBytes = pck.GetAsByteArray();
                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=ALFAPARTS-PartReplacement.xlsx");
                // Заменяю имя выходного Эксель файла

                Response.Charset = "";
                Response.ContentType = "application/vnd.ms-excel";
                StringWriter sw = new StringWriter();
                Response.BinaryWrite(fileBytes);
                Response.End();
            }
            return RedirectToAction("Index");
        }

        public ActionResult ExcelImport()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ExcelImport(HttpPostedFileBase upload)
        {
            int firstDataRow = 2;
            try
            {
                if (upload != null && upload.ContentLength > 0)
                {
                    // clear the table
                    var all = from c in db.PartReplacement select c;
                    db.PartReplacement.RemoveRange(all);
                    // load from stream
                    using (ExcelPackage package = new ExcelPackage(upload.InputStream))
                    {
                        ExcelWorksheet worksheet = package.Workbook.Worksheets.FirstOrDefault();
                        for (int i = firstDataRow; i <= worksheet.Dimension.End.Row; i++)
                        {
                            PartReplacement partReplacement = new PartReplacement
                            {
                                Number = worksheet.Cells["A" + i.ToString()].Value.ToString(),
                                Replacement = worksheet.Cells["B" + i.ToString()].Value.ToString()
                            };
                            db.PartReplacement.Add(partReplacement);
                        }
                    }
                    db.SaveChanges();
                    ViewBag.Message = "Импорт завершен.";
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Ошибка импорта:" + ex.Message.ToString();

                return RedirectToAction("Index"); 
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
