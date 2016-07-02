namespace AutoPartsWebSite.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Import")]
    public partial class Import
    {
        [Key]
        [Display(Name = "Номер")]
        public int Id { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Дата")]
        public DateTime? Date { get; set; }

        [StringLength(128)]
        public string UserId { get; set; }

        [Display(Name = "Поставщик")]
        public string Supplier
        {
            get
            {
                SupplierModel db = new SupplierModel();
                Supplier supplier = db.Suppliers.Find(SupplierId);
                if (supplier == null)
                {
                    return "";
                }
                return supplier.Name.ToString();

            }
        }

        [Display(Name = "Номер поставщика")]
        public int? SupplierId { get; set; }

        [Display(Name = "Срок поставки")]
        [StringLength(10)]
        public string DeliveryTime { get; set; }

        [Display(Name = "Имя файла")]
        public string FileName { get; set; }
    }
}
