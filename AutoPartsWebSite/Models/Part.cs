namespace AutoPartsWebSite.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Part")]
    public partial class Part
    {
        public int Id { get; set; }

        public int? ImportId { get; set; }

         [Display(Name = "Марка")]
        public string Brand { get; set; }

        [Display(Name = "Номер")]
        public string Number { get; set; }

        [Display(Name = "Название")]
        public string Name { get; set; }

        [Display(Name = "Описание")]
        public string Details { get; set; }

        [Display(Name = "Объем")]
        public string Size { get; set; }

        [Display(Name = "Вес")]
        public string Weight { get; set; }

        [Display(Name = "Количество")]
        public string Quantity { get; set; }

        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        [Display(Name = "Цена")]
        public string Price { get; set; }

        [Display(Name = "Поставщик")]        
        public string Supplier
        { get
            {
                SupplierModel db = new SupplierModel();
                Supplier supplier = db.Suppliers.Find(SupplierId);
                if (supplier == null)
                {
                    return "";
                }
                return supplier.Code.ToString();
                
            }
        }

        [Display(Name = "Номер поставщика")]
        public int SupplierId { get; set; }

        [Display(Name = "Срок поставки")]
        public string DeliveryTime
        {
            get
            {
                SupplierModel db = new SupplierModel();
                Supplier supplier = db.Suppliers.Find(SupplierId);
                if (supplier == null)
                {
                    return "";
                }
                return supplier.DeliveryTime.ToString();

            }
        }
    }
}
