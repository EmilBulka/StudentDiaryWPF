namespace Diary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EmptyMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Ratings", "SubjectId", c => c.Int(nullable: false));
            DropColumn("dbo.Ratings", "Subcject");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Ratings", "Subcject", c => c.Int(nullable: false));
            DropColumn("dbo.Ratings", "SubjectId");
        }
    }
}
