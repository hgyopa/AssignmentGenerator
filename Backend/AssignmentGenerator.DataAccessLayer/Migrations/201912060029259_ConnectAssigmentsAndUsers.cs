namespace AssignmentGenerator.DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ConnectAssigmentsAndUsers : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Assignments", "UserId", c => c.String(maxLength: 128));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Assignments", "UserId");
        }
    }
}
