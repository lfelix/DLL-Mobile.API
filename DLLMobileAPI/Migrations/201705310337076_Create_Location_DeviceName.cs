namespace DLLMobileAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Create_Location_DeviceName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LoginActivities", "Location", c => c.String());
            AddColumn("dbo.LoginActivities", "FriendlyDeviceName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.LoginActivities", "FriendlyDeviceName");
            DropColumn("dbo.LoginActivities", "Location");
        }
    }
}
