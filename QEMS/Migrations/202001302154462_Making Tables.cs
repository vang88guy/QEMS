namespace QEMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MakingTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Kins",
                c => new
                    {
                        KinId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Relation = c.String(),
                        PhoneNumber = c.String(),
                        Address = c.String(),
                        City = c.String(),
                        State = c.String(),
                        ZipCode = c.String(),
                        PersonId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.KinId)
                .ForeignKey("dbo.People", t => t.PersonId, cascadeDelete: true)
                .Index(t => t.PersonId);
            
            CreateTable(
                "dbo.People",
                c => new
                    {
                        PersonId = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        MiddleName = c.String(),
                        LastName = c.String(),
                        DateOfBirth = c.String(),
                        PhoneNumber = c.String(),
                        Addresss = c.String(),
                        City = c.String(),
                        State = c.String(),
                        ZipCode = c.Int(nullable: false),
                        LicenseNumber = c.String(),
                        ApplicationId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.PersonId)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationId)
                .Index(t => t.ApplicationId);
            
            CreateTable(
                "dbo.Operators",
                c => new
                    {
                        OperatorId = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Addresss = c.String(),
                        City = c.String(),
                        State = c.String(),
                        ZipCode = c.String(),
                    })
                .PrimaryKey(t => t.OperatorId);
            
            CreateTable(
                "dbo.Situations",
                c => new
                    {
                        SituationId = c.Int(nullable: false, identity: true),
                        Message = c.String(),
                        Time = c.String(),
                        Date = c.String(),
                        Severity = c.String(),
                        CallPoliceStation = c.Boolean(nullable: false),
                        CallFireStation = c.Boolean(nullable: false),
                        CallAmbulance = c.Boolean(nullable: false),
                        InProcess = c.Boolean(nullable: false),
                        Complete = c.Boolean(nullable: false),
                        PersonId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.SituationId)
                .ForeignKey("dbo.People", t => t.PersonId, cascadeDelete: true)
                .Index(t => t.PersonId);
            
            CreateTable(
                "dbo.SituationsOperators",
                c => new
                    {
                        SituationId = c.Int(nullable: false),
                        OperatorId = c.Int(nullable: false),
                        Time = c.String(),
                        Date = c.String(),
                    })
                .PrimaryKey(t => new { t.SituationId, t.OperatorId })
                .ForeignKey("dbo.Operators", t => t.OperatorId, cascadeDelete: true)
                .ForeignKey("dbo.Situations", t => t.SituationId, cascadeDelete: true)
                .Index(t => t.SituationId)
                .Index(t => t.OperatorId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SituationsOperators", "SituationId", "dbo.Situations");
            DropForeignKey("dbo.SituationsOperators", "OperatorId", "dbo.Operators");
            DropForeignKey("dbo.Situations", "PersonId", "dbo.People");
            DropForeignKey("dbo.Kins", "PersonId", "dbo.People");
            DropForeignKey("dbo.People", "ApplicationId", "dbo.AspNetUsers");
            DropIndex("dbo.SituationsOperators", new[] { "OperatorId" });
            DropIndex("dbo.SituationsOperators", new[] { "SituationId" });
            DropIndex("dbo.Situations", new[] { "PersonId" });
            DropIndex("dbo.People", new[] { "ApplicationId" });
            DropIndex("dbo.Kins", new[] { "PersonId" });
            DropTable("dbo.SituationsOperators");
            DropTable("dbo.Situations");
            DropTable("dbo.Operators");
            DropTable("dbo.People");
            DropTable("dbo.Kins");
        }
    }
}
