namespace Data.DAL
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class aisdasd : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Transaction", "Percent");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Transaction", "Percent", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
