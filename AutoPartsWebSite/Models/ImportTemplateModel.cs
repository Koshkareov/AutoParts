namespace AutoPartsWebSite.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class ImportTemplateModel : DbContext
    {
        public ImportTemplateModel()
            : base("name=AutoPartsDB")
        {
        }

        public virtual DbSet<ImportTemplate> ImportTemplates { get; set; }
        public virtual DbSet<Supplier> Suppliers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
