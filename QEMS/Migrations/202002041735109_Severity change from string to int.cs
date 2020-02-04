namespace QEMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Severitychangefromstringtoint : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Situations", "Severity", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Situations", "Severity", c => c.String());
        }
    }
}
