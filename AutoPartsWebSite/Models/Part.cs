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

        public string Brand { get; set; }

        public string Number { get; set; }

        public string Name { get; set; }

        public string Details { get; set; }

        public string Size { get; set; }

        public string Weight { get; set; }

        public string Quantity { get; set; }

        public string Price { get; set; }

        public string Supplier { get; set; }

        public string DeliveryTime { get; set; }
    }
}
