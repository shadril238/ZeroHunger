namespace ZeroHunger.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FoodItemTable_FixStringLengthOfNameV11 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.FoodItems", "Name", c => c.String(nullable: false, maxLength: 50));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.FoodItems", "Name", c => c.String(nullable: false));
        }
    }
}
