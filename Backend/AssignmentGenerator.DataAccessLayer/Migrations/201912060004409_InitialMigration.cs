namespace AssignmentGenerator.DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Answers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Text = c.String(nullable: false, maxLength: 255),
                        IsCorrect = c.Boolean(nullable: false),
                        QuestionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Questions", t => t.QuestionId)
                .Index(t => t.QuestionId);
            
            CreateTable(
                "dbo.Questions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Text = c.String(nullable: false, maxLength: 255),
                        QuestionTypeId = c.Int(nullable: false),
                        AssignmentId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Assignments", t => t.AssignmentId)
                .ForeignKey("dbo.QuestionTypes", t => t.QuestionTypeId)
                .Index(t => t.QuestionTypeId)
                .Index(t => t.AssignmentId);
            
            CreateTable(
                "dbo.Assignments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 255),
                        CreationDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.QuestionTypes",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Answers", "QuestionId", "dbo.Questions");
            DropForeignKey("dbo.Questions", "QuestionTypeId", "dbo.QuestionTypes");
            DropForeignKey("dbo.Questions", "AssignmentId", "dbo.Assignments");
            DropIndex("dbo.Questions", new[] { "AssignmentId" });
            DropIndex("dbo.Questions", new[] { "QuestionTypeId" });
            DropIndex("dbo.Answers", new[] { "QuestionId" });
            DropTable("dbo.QuestionTypes");
            DropTable("dbo.Assignments");
            DropTable("dbo.Questions");
            DropTable("dbo.Answers");
        }
    }
}
