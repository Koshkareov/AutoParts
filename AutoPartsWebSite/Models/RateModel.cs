namespace AutoPartsWebSite.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class RateModel : DbContext
    {
        public RateModel()
            : base("name=AutoPartsDB")
        {
        }

        public virtual DbSet<Rate> Rates { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Rate>()
                .Property(e => e.Value)
                .HasPrecision(7, 2);
        }
    }
}
