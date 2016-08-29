namespace AutoPartsWebSite.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class InvoiceModel : DbContext
    {
        public InvoiceModel()
            : base("name=AutoPartsDB")
        {
        }

        public virtual DbSet<Invoice> Invoices { get; set; }
        public virtual DbSet<InvoiceItem> InvoiceItems { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderItem> OrderItems { get; set; }
        //public virtual DbSet<InvoiceItemOrderItem> InvoiceItemOrderItems { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Invoice>()
                .HasMany(e => e.InvoiceItems)
                .WithRequired(e => e.Invoice)
                .WillCascadeOnDelete(false);

            //modelBuilder.Entity<InvoiceItem>()
            //    .HasMany(e => e.InvoiceItemOrderItems)
            //    .WithRequired(e => e.InvoiceItem)
            //    .WillCascadeOnDelete(false);

            modelBuilder.Entity<InvoiceItem>()
               .HasMany(c => c.OrderItems)
               .WithMany(i => i.InvoiceItems)
               .Map(t =>
                   { t.MapLeftKey("InvoiceItemId");
                     t.MapRightKey("OrderItemId");
                     t.ToTable("InvoiceItemOrderItem");
                   });

            modelBuilder.Entity<Order>()
                .Property(e => e.Summary)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Order>()
                .HasMany(e => e.OrderItems)
                .WithRequired(e => e.Order)
                .WillCascadeOnDelete(false);

            //modelBuilder.Entity<OrderItem>()
            //    .HasMany(e => e.InvoiceItems)
            //    .WithRequired(e => e.OrderItem)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<OrderItem>()
            //   .HasMany(c => c.InvoiceItems)
            //   .WithMany(i => i.OrderItems)
            //   .Map(t => t.MapLeftKey("InvoiceItemId")
            //   .MapRightKey("OrderItemId")
            //   .ToTable("InvoiceItemOrderItem"));
        }
    }
}
