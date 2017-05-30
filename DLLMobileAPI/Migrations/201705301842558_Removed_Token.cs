namespace DLLMobileAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Removed_Token : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.LoginActivities", "Token");
        }
        
        public override void Down()
        {
            AddColumn("dbo.LoginActivities", "Token", c => c.String());
        }
    }
}
