namespace DLLMobileAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Annotation : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.LoginActivities", "IdUser");
        }
        
        public override void Down()
        {
            DropIndex("dbo.LoginActivities", new[] { "IdUser" });
        }
    }
}
