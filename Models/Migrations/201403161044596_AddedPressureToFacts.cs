namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedPressureToFacts : DbMigration
    {
        public override void Up()
        {
            RenameColumn("dbo.FactTable", "ProductionDate", "Date");
            AddColumn("dbo.FactTable", "Pressure", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            RenameColumn("dbo.FactTable", "Date", "ProductionDate");
            DropColumn("dbo.FactTable", "Pressure");
        }
    }
}
