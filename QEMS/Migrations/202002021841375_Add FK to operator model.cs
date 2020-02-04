namespace QEMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFKtooperatormodel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Operators", "ApplicationId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Operators", "ApplicationId");
            AddForeignKey("dbo.Operators", "ApplicationId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Operators", "ApplicationId", "dbo.AspNetUsers");
            DropIndex("dbo.Operators", new[] { "ApplicationId" });
            DropColumn("dbo.Operators", "ApplicationId");
        }
    }
}
