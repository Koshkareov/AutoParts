using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.IO;

namespace AutoPartsImport
{
    class PartsInfo
    {
        // Метод, реализующий словарь
        public static Dictionary<string, string> PartDic()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            // Читаем настройки конфигурации
            dic.Add("Brand", ConfigurationManager.AppSettings["Brand"]);   //  Brand = 1
            dic.Add("Number", ConfigurationManager.AppSettings["Number"]);   //  Number = 2
            dic.Add("Name", ConfigurationManager.AppSettings["Name"]);   //  Name = 4
            dic.Add("Details", ConfigurationManager.AppSettings["Details"]);   //  Details = 3
            dic.Add("Size", ConfigurationManager.AppSettings["Size"]);   //  Size = 6
            dic.Add("Weight", ConfigurationManager.AppSettings["Weight"]);   //  Weight = 5
            dic.Add("Quantity", ConfigurationManager.AppSettings["Quantity"]);   //  Quantity = 10
            dic.Add("Price", ConfigurationManager.AppSettings["Price"]);   //  Price = 8
            dic.Add("Supplier", ConfigurationManager.AppSettings["Supplier"]);   //  Supplier = 7
            dic.Add("DeliveryTime", ConfigurationManager.AppSettings["DeliveryTime"]);   //  DeliveryTime


            return dic;
        }
    }
}
