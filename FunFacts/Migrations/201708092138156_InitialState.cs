namespace FunFacts.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialState : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ChuckNorrisFunFacts",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Fact = c.String(nullable: false),
                        Rating = c.Int(nullable: false),
                        ModifiedWhen = c.DateTime(nullable: false),
                        ModifiedBy = c.String(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Rating);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.ChuckNorrisFunFacts", new[] { "Rating" });
            DropTable("dbo.ChuckNorrisFunFacts");
        }
    }
}
