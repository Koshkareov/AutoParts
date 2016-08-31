namespace AutoPartsWebSite.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web.Mvc;

    [Table("InvoiceItem")]
    public partial class InvoiceItem
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        //public InvoiceItem()
        //{
        //    InvoiceItemOrderItems = new HashSet<InvoiceItemOrderItem>();
        //}

        [Key]
        public int Id { get; set; }

        [ForeignKey("Invoice")]
        [Required]
        public int InvoiceId { get; set; }

        [Display(Name = "Номер")]
        public string Number { get; set; }

        [Display(Name = "Количество")]
        public string Quantity { get; set; }

        public DateTime? Date { get; set; }

        [Display(Name = "Статус")]
        public int? State { get; set; }
        public List<SelectListItem> getInvoiceItemStates()
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

        public virtual Invoice Invoice { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<InvoiceItemOrderItem> InvoiceItemOrderItems { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}
