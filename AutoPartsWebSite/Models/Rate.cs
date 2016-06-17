namespace AutoPartsWebSite.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web.Mvc;
    [Table("Rate")]
    public partial class Rate
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(128)]
        [Display(Name = "Пользователь")]
        public string UserId { get; set; }

        [Required]
        [Display(Name = "Поставщик")]
        public int SupplierId { get; set; }       
        public IEnumerable<SelectListItem> Suppliers { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Дата")]
        public DateTime? Data { get; set; }

        [Column(TypeName = "numeric")]
        [Display(Name = "Значение %")]
        public decimal? Value { get; set; }
    }
}
