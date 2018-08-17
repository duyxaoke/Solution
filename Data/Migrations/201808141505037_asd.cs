namespace Data.DAL
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class asd : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Config", "Currency", c => c.String());
            AddColumn("dbo.Config", "ReferalBonus", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Config", "ReferalBonus");
            DropColumn("dbo.Config", "Currency");
        }
    }
}
