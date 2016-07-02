namespace AutoPartsWebSite.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class ImportModel : DbContext
    {
        public ImportModel()
            : base("name=AutoPartsDB")
        {
        }

        public virtual DbSet<Import> Imports { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
