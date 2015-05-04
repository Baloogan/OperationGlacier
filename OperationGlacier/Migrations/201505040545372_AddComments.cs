namespace OperationGlacier.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddComments : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        CommentID = c.Int(nullable: false, identity: true),
                        Username = c.String(),
                        date_in_game = c.DateTime(nullable: false),
                        date_in_world = c.DateTime(nullable: false),
                        timeline_id = c.String(),
                        message = c.String(),
                        side_restriction = c.String(),
                    })
                .PrimaryKey(t => t.CommentID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Comments");
        }
    }
}
