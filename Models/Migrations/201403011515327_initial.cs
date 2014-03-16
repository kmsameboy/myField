namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FactTable",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Production = c.Double(nullable: false),
                        ProductionDate = c.DateTime(nullable: false),
                        Well_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Well", t => t.Well_Id)
                .Index(t => t.Well_Id);
            
            CreateTable(
                "dbo.Well",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        CoordinateX = c.Int(nullable: false),
                        CoordinateY = c.Int(nullable: false),
                        Field_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Field", t => t.Field_Id, cascadeDelete: true)
                .Index(t => t.Field_Id);
            
            CreateTable(
                "dbo.Field",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        MapImage = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FactTable", "Well_Id", "dbo.Well");
            DropForeignKey("dbo.Well", "Field_Id", "dbo.Field");
            DropIndex("dbo.FactTable", new[] { "Well_Id" });
            DropIndex("dbo.Well", new[] { "Field_Id" });
            DropTable("dbo.Field");
            DropTable("dbo.Well");
            DropTable("dbo.FactTable");
        }
    }
}
