namespace AutoPartsWebSite.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web.Mvc;
    [Table("Order")]
    public partial class Order
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Order()
        {
            OrderItems = new HashSet<OrderItem>();
        }
        [Key]

        [Display(Name = "Номер")]
        public int Id { get; set; }
        
        [Required]
        public string UserId { get; set; }
                
        [Column(TypeName = "money")]
        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        [Display(Name = "Сумма")]
        public decimal? Summary { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Дата")]
        public DateTime? Data { get; set; }

        [Display(Name = "Статус")]
        public int State { get; set; }
        

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderItem> OrderItems { get; set; }

        public List<SelectListItem> getOrderStates()
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
    }
}
