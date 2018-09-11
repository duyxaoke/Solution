namespace Data.DAL
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class adfd : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "AvatarLink", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "AvatarLink");
        }
    }
}
