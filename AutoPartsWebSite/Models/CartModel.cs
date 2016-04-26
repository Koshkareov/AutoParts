namespace AutoPartsWebSite.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class CartModel : DbContext
    {
        public CartModel()
            : base("name=AutoPartsDB")
        {
        }

        public virtual DbSet<Part> Parts { get; set; }

        public virtual DbSet<Cart> Carts { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
