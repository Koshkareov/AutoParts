using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoPartsImport
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openImportFileDialog.Filter = "XSLT Files (.xlsx)|*.xlsx|All Files (*.*)|*.*";
            openImportFileDialog.FilterIndex = 1;
            if (openImportFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //System.IO.StreamReader sr = new
                //   System.IO.StreamReader(openImportFileDialog.FileName);
                //MessageBox.Show(sr.ReadToEnd());
                //sr.Close();
              
                buttonImport.Enabled = true;

            }
        }

        private static void LoadFileData(string fileName)
        {
            Dictionary<string, string> dic = PartsInfo.PartDic();
            
            int firstDataRow = Convert.ToInt32(ConfigurationManager.AppSettings["FirstDataRow"]);

            FileInfo autopartsFile = new FileInfo(fileName);
            //FileInfo autopartsFile = new FileInfo(@"C:\\ALFA-PARTS\\ALFA-PARTS-TOYOTA.xlsx");
            // FileInfo autopartsFile = new FileInfo(@"C:\\ALFA-PARTS\\ALFA-PARTS-HYUNDAI.xlsx");
            // FileInfo autopartsFile = new FileInfo(@"C:\\ALFA-PARTS\\ALFA-PARTS-ISUZU.xlsx");
            using (ExcelPackage package = new ExcelPackage(autopartsFile))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.FirstOrDefault();
                Console.WriteLine("База данных содержит: " + worksheet.Dimension.End.Row);
                
                // Подготавливаю словарь для строчки данных
                Dictionary<string, string> dicPart = new Dictionary<string, string>();
                string importGuid = System.Guid.NewGuid().ToString();
                int importId = AddImportData();
                for (int i = firstDataRow; i < worksheet.Dimension.End.Row; i++)
                {
                    dicPart.Clear();
                    dicPart.Add("Id", Convert.ToString(importId));   // import ID
                    //dicPart.Add("Guid", importGuid);   // import GUID
                    dicPart.Add("Brand", Convert.ToString(worksheet.Cells[dic["Brand"] + i.ToString()].Value));   //  Brand = 1
                    dicPart.Add("Number", Convert.ToString(worksheet.Cells[dic["Number"] + i.ToString()].Value));   //  Number = 2
                    dicPart.Add("Name", Convert.ToString(worksheet.Cells[dic["Name"] + i.ToString()].Value));   //  Name = 4
                    dicPart.Add("Details", Convert.ToString(worksheet.Cells[dic["Details"] + i.ToString()].Value));   //  Details = 3
                    dicPart.Add("Size", Convert.ToString(worksheet.Cells[dic["Size"] + i.ToString()].Value));   //  Size = 6
                    dicPart.Add("Weight", Convert.ToString(worksheet.Cells[dic["Weight"] + i.ToString()].Value));   //  Weight = 5
                    dicPart.Add("Quantity", Convert.ToString(worksheet.Cells[dic["Quantity"] + i.ToString()].Value));   //  Quantity = 10
                    dicPart.Add("Price", Convert.ToString(worksheet.Cells[dic["Price"] + i.ToString()].Value));   //  Price = 8
                    dicPart.Add("Supplier", Convert.ToString(worksheet.Cells[dic["Supplier"] + i.ToString()].Value));   //  Supplier = 7
                    dicPart.Add("DeliveryTime", Convert.ToString(worksheet.Cells[dic["DeliveryTime"] + i.ToString()].Value));   //  DeliveryTime

                    Console.WriteLine(i + " from " + worksheet.Dimension.End.Row);
                    AddPartData(ref dicPart);
                }
            }
        }

        public static void AddPartData(ref Dictionary<string, string> dicPartData)
        {
            using (var db = new Model()) //AutoPartsDBEntities())
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
                    Supplier = dicPartData["Supplier"],
                    DeliveryTime = dicPartData["DeliveryTime"]
                };

                db.Parts.Add(autopart);
                db.SaveChanges();
            }
        }

        public static int AddImportData()
        {
            using (var db = new Model()) //AutoPartsDBEntities())
            {
                var imp = new Import
                {
                    Date = DateTime.Now
                };

                db.Imports.Add(imp);
                db.SaveChanges();
                var id = imp.Id;
                return id;
            }
           
        }

        private void buttonImport_Click(object sender, EventArgs e)
        {
            LoadFileData(openImportFileDialog.FileName);
        }
    }
}
