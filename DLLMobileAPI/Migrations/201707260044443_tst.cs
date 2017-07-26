namespace DLLMobileAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tst : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ApplicationUsers", "Cpf", c => c.Long(nullable: false));
            AddColumn("dbo.ApplicationUsers", "CellPhoneNumber", c => c.Long(nullable: false));
            CreateIndex("dbo.ApplicationUsers", "Cpf", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.ApplicationUsers", new[] { "Cpf" });
            DropColumn("dbo.ApplicationUsers", "CellPhoneNumber");
            DropColumn("dbo.ApplicationUsers", "Cpf");
        }
    }
}
