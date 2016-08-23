namespace AutoPartsWebSite.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web.Mvc;

    [Table("Invoice")]
    public partial class Invoice
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Invoice()
        {
            InvoiceItems = new HashSet<InvoiceItem>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(128)]
        public string UserId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Дата")]
        public DateTime? Data { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Номер")]
        public string Number { get; set; }

        [Display(Name = "Статус")]
        public int? State { get; set; }

        public List<SelectListItem> getInvoiceStates()
        {
            List<SelectListItem> StateItems = new List<SelectListItem>();
            StateItems.Add(new SelectListItem
            {
                Text = "Статус 1",
                Value = "1"
            });
            StateItems.Add(new SelectListItem
            {
                Text = "Статус 2",
                Value = "2",
                Selected = true
            });
            StateItems.Add(new SelectListItem
            {
                Text = "Статус 3",
                Value = "3"
            });
            return StateItems;
        }

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
                return supplier.Code.ToString();

            }
        }

        [Required]
        [Display(Name = "Номер поставщика")]
        public int SupplierId { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InvoiceItem> InvoiceItems { get; set; }
    }
}
