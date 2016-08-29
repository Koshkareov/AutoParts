namespace AutoPartsWebSite.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("InvoiceItemOrderItem")]
    public partial class InvoiceItemOrderItem
    {
        [Key]
        [ForeignKey("InvoiceItem")]
        [Column(Order = 1)]
        public int InvoiceItemId { get; set; }

        [Key]
        [ForeignKey("OrderItem")]
        [Column(Order = 2)]
        public int OrderItemId { get; set; }
        
        //[Display(Name = "Количество")]
        //public int Quantity { get; set; }

        public virtual InvoiceItem InvoiceItem { get; set; }

        public virtual OrderItem OrderItem { get; set; }
    }
}
