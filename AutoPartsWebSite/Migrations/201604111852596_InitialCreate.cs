namespace AutoPartsWebSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Part",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ImportId = c.Int(),
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
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Part");
        }
    }
}
