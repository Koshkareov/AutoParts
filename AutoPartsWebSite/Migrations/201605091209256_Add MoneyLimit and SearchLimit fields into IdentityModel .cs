namespace AutoPartsWebSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMoneyLimitandSearchLimitfieldsintoIdentityModel : DbMigration
    {
        public override void Up()
        {
            //AddColumn("dbo.AspNetUsers", "MoneyLimit", c => c.Decimal(storeType:"Money"));
            //AddColumn("dbo.AspNetUsers", "SearchLimit", c => c.Int(defaultValue: 1));
        }

        public override void Down()
        {
            //DropColumn("dbo.AspNetUsers", "MoneyLimit");
            //DropColumn("dbo.AspNetUsers", "SearchLimit");
        }
    }
}
