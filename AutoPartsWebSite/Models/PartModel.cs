namespace AutoPartsWebSite.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class PartModel : DbContext
    {
        public PartModel()
            : base("name=AutoPartsDB")
        {
        }

        public virtual DbSet<Part> Parts { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
