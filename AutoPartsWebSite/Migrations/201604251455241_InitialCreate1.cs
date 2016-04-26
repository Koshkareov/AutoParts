namespace AutoPartsWebSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cart",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        Brand = c.String(),
                        Number = c.String(),
                        Name = c.String(),
                        Details = c.String(),
                        Size = c.String(),
                        Weight = c.String(),
                        Quantity = c.String(),
                        Price = c.String(),
                        Supplier = c.String(),
                        DeliveryTime = c.String(),
                        Amount = c.Int(),
                        Data = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Cart");
        }
    }
}
