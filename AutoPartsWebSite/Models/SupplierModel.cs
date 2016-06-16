namespace AutoPartsWebSite.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class SupplierModel : DbContext
    {
        public SupplierModel()
            : base("name=AutoPartsDB")
        {
        }

        public virtual DbSet<Supplier> Suppliers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Supplier>()
                .Property(e => e.Rate)
                .HasPrecision(7, 2);
        }
    }
}
