namespace click_gas
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using click_gas.Models;

    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=User")
        {
        }


        public virtual DbSet<Cliente> Cliente { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cliente>().Property(e => e.nome).IsUnicode(false);
        }
    }
}
