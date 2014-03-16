using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class FieldContext : DbContext
    {

        public DbSet<Well> Wells { get; set; }
        public DbSet<Field> Fields { get; set; }
        public DbSet<FactTable> Facts { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
