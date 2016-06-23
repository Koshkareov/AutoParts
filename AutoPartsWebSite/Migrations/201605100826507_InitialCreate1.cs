namespace AutoPartsWebSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate1 : DbMigration
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
                    SupplierId = c.Int(),
                    DeliveryTime = c.String(),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Import",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    ImportId = c.Int(),
                    Date = c.DateTime(),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Cart",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    PartId = c.Int(nullable: false),
                    UserId = c.String(nullable: false),
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

            CreateTable(
                "dbo.Payment",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    UserId = c.String(nullable: false),
                    Data = c.DateTime(),
                    Amount = c.Int(),
                })
                .PrimaryKey(t => t.Id);

            AddColumn("dbo.AspNetUsers", "FirstName", c => c.String());
            AddColumn("dbo.AspNetUsers", "LastName", c => c.String());
            AddColumn("dbo.AspNetUsers", "Phone", c => c.String());
            AddColumn("dbo.AspNetUsers", "MoneyLimit", c => c.Int(nullable: false, defaultValue: 0, defaultValueSql: "0"));
            AddColumn("dbo.AspNetUsers", "SearchLimit", c => c.Int(nullable: false, defaultValue: 1, defaultValueSql: "1"));
        }

        public override void Down()
        {
            DropTable("dbo.Part");
            DropTable("dbo.Import");
            DropTable("dbo.Cart");
            DropTable("dbo.Payment");

            DropColumn("dbo.AspNetUsers", "FirstName");
            DropColumn("dbo.AspNetUsers", "LastName");
            DropColumn("dbo.AspNetUsers", "Phone");
            DropColumn("dbo.AspNetUsers", "MoneyLimit");
            DropColumn("dbo.AspNetUsers", "SearchLimit");
        }
    }
}
