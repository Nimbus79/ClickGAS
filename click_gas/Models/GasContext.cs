namespace click_gas.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class GasContext : DbContext
    {
        public GasContext()
            : base("name=GasContext")
        {
        }

        public virtual DbSet<Cliente> Clientes { get; set; }
        public virtual DbSet<Funcionario> Funcionarios { get; set; }
        public virtual DbSet<Pedido> Pedidoes { get; set; }
        public virtual DbSet<Pedido_tem_Servico> Pedido_tem_Servico { get; set; }
        public virtual DbSet<Servico> Servicoes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cliente>()
                .Property(e => e.nome)
                .IsUnicode(false);

            modelBuilder.Entity<Cliente>()
                .Property(e => e.palavraPasse)
                .IsUnicode(false);

            modelBuilder.Entity<Cliente>()
                .Property(e => e.email)
                .IsUnicode(false);

            modelBuilder.Entity<Cliente>()
                .Property(e => e.rua)
                .IsUnicode(false);

            modelBuilder.Entity<Cliente>()
                .Property(e => e.codPostal)
                .IsUnicode(false);

            modelBuilder.Entity<Cliente>()
                .Property(e => e.freguesia)
                .IsUnicode(false);

            modelBuilder.Entity<Cliente>()
                .Property(e => e.concelho)
                .IsUnicode(false);

            modelBuilder.Entity<Cliente>()
                .Property(e => e.role)
                .IsUnicode(false);

            modelBuilder.Entity<Cliente>()
                .HasMany(e => e.Pedidoes)
                .WithRequired(e => e.Cliente)
                .HasForeignKey(e => e.emailCliente)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Funcionario>()
                .Property(e => e.palavraPasse)
                .IsUnicode(false);

            modelBuilder.Entity<Funcionario>()
                .Property(e => e.email)
                .IsUnicode(false);

            modelBuilder.Entity<Funcionario>()
                .Property(e => e.role)
                .IsUnicode(false);

            modelBuilder.Entity<Funcionario>()
                .HasMany(e => e.Pedidoes)
                .WithRequired(e => e.Funcionario)
                .HasForeignKey(e => e.emailFuncionario)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Pedido>()
                .Property(e => e.emailCliente)
                .IsUnicode(false);

            modelBuilder.Entity<Pedido>()
                .Property(e => e.emailFuncionario)
                .IsUnicode(false);

            modelBuilder.Entity<Pedido>()
                .Property(e => e.observacoes)
                .IsUnicode(false);

            modelBuilder.Entity<Pedido>()
                .HasMany(e => e.Pedido_tem_Servico)
                .WithRequired(e => e.Pedido)
                .HasForeignKey(e => e.idPedido)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Servico>()
                .Property(e => e.tipo)
                .IsUnicode(false);

            modelBuilder.Entity<Servico>()
                .Property(e => e.descricao)
                .IsUnicode(false);

            modelBuilder.Entity<Servico>()
                .HasMany(e => e.Pedido_tem_Servico)
                .WithRequired(e => e.Servico)
                .HasForeignKey(e => e.idServico)
                .WillCascadeOnDelete(false);
        }
    }
}
