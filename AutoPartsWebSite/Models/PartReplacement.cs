namespace AutoPartsWebSite.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PartReplacement")]
    public partial class PartReplacement
    {
        public int Id { get; set; }

        [Display(Name = "Номер")]
        public string Number { get; set; }

        [Display(Name = "Замена")]
        public string Replacement { get; set; }
    }
}
