namespace DLLMobileAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Removed_IdUser : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.LoginActivities", "IdUser");
        }
        
        public override void Down()
        {
            AddColumn("dbo.LoginActivities", "IdUser", c => c.Int(nullable: false));
        }
    }
}
