namespace AutoPartsWebSite.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PartAlias")]
    public partial class PartAlias
    {
        public int Id { get; set; }

        [Display(Name = "Номер")]
        public string Number { get; set; }

        [Display(Name = "Название")]
        public string Name { get; set; }       

        [Display(Name = "Объем")]
        public string Size { get; set; }

        [Display(Name = "Вес")]
        public string Weight { get; set; }       
        
    }
}
