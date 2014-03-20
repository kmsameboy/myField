namespace Models.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Models.FieldContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Models.FieldContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            var ktl = new Field { MapImage = "Field A.png", Name = "KTL" };
            var sgp = new Field { MapImage = "Field B.png", Name = "SGP" };

            var w1 = new Well
            {
                Name = "A1",
                Field = ktl,
                DrillDate = new DateTime(2014, 1, 1),
                ProductionDate = new DateTime(2014, 1, 1),
                CoordinateX = 100,
                CoordinateY = 100
            };

            var w2 = new Well
            {
                Name = "A2",
                Field = ktl,
                DrillDate = new DateTime(2014, 1, 1),
                ProductionDate = new DateTime(2014, 1, 1),
                CoordinateX = 300,
                CoordinateY = 100
            };

            var w3 = new Well
            {
                Name = "A3",
                Field = ktl,
                DrillDate = new DateTime(2014, 1, 1),
                ProductionDate = new DateTime(2014, 1, 1),
                CoordinateX = 100,
                CoordinateY = 300
            };

            var w4 = new Well
            {
                Name = "B1",
                Field = sgp,
                DrillDate = new DateTime(2014, 1, 1),
                ProductionDate = new DateTime(2014, 1, 1),
                CoordinateX = 100,
                CoordinateY = 100
            };

            var w5 = new Well
            {
                Name = "B2",
                Field = sgp,
                DrillDate = new DateTime(2014, 1, 1),
                ProductionDate = new DateTime(2014, 1, 1),
                CoordinateX = 250,
                CoordinateY = 100
            };

            var f1 = new FactTable
            {
                Well = w1,
                Production = 1000,
                Pressure = 299,
                Date = DateTime.Now.AddMonths(-2)
            };

            var f2 = new FactTable
            {
                Well = w1,
                Production = 3000,
                Pressure = 112,
                Date = DateTime.Now.AddMonths(-2).AddDays(10)
            };

            var f3 = new FactTable
            {
                Well = w3,
                Production = 1000,
                Pressure = 299,
                Date = DateTime.Now.AddMonths(-1)
            };

            var f4 = new FactTable
            {
                Well = w2,
                Production = 1000,
                Pressure = 299,
                Date = DateTime.Now.AddMonths(-2).AddDays(5)
            };

            var f5 = new FactTable
            {
                Well = w1,
                Production = 1040,
                Pressure = 339,
                Date = DateTime.Now.AddMonths(-1)
            };

            var f6 = new FactTable
            {
                Well = w3,
                Production = 1500,
                Pressure = 566,
                Date = DateTime.Now.AddMonths(-1).AddDays(10)
            };

            context.Fields.AddOrUpdate(
                ktl,
                sgp
                );

            context.Wells.AddOrUpdate(w1, w2, w3, w4, w5);

            context.Facts.AddOrUpdate(f1, f2, f3, f4, f5, f6);

            context.SaveChanges();
        }
    }
}
