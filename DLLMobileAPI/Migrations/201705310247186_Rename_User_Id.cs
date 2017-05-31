namespace DLLMobileAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Rename_User_Id : DbMigration
    {
        public override void Up()
        {
            RenameColumn("dbo.LoginActivities", "User_Id", "IdUser");
        }
        
        public override void Down()
        {
            RenameColumn("dbo.LoginActivities", "IdUser", "User_Id");
        }
    }
}
