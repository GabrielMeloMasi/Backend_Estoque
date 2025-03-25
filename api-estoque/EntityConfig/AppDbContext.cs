using api_estoque.Models;
using Microsoft.EntityFrameworkCore;

namespace api_estoque.EntityConfig
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; } 
        public DbSet<Estoque> Estoque { get; set; }
        public DbSet<Produto> Produto { get; set; }
        public DbSet<EstoqueProduto> EstoqueProdutos { get; set; }
        public DbSet<Categoria> Categoria { get; set; }
        public DbSet<Validade> Validade { get; set; }
        public DbSet<Movimentacao> Movimentacao { get; set; }




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EstoqueProduto>().HasKey(ep => ep.Id);
            modelBuilder.Entity<User>().HasKey(ep => ep.Id);
            modelBuilder.Entity<Estoque>().HasKey(ep => ep.Id);
            modelBuilder.Entity<Produto>().HasKey(ep => ep.Id);
            modelBuilder.Entity<Validade>().HasKey(ep => ep.Id);
            modelBuilder.Entity<Categoria>().HasKey(ep => ep.Id);
            modelBuilder.Entity<Movimentacao>().HasKey(ep => ep.Id);

            modelBuilder.Entity<Produto>()
            .HasDiscriminator<int>("TipoProduto")
            .HasValue<ProdutoBasic>(0)
            .HasValue<ProdutoPerecivel>(1);

            modelBuilder.Entity<Validade>()
                .HasOne(p => p.EstoqueProduto)
                .WithMany(v => v.Validades)
                .HasForeignKey(v => v.EstoqueProdutoId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<EstoqueProduto>()
                .HasOne(ep => ep.Estoque)
                .WithMany(e => e.EstoqueProdutos)
                .HasForeignKey(ep => ep.EstoqueId);

            modelBuilder.Entity<EstoqueProduto>()
                .HasOne(ep => ep.Produto)
                .WithMany()
                .HasForeignKey(ep => ep.ProdutoId);

            modelBuilder.Entity<Estoque>()
                .HasOne(e => e.User)
                .WithOne()
                .HasForeignKey<Estoque>(e => e.UserId) 
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Movimentacao>()
                .HasOne(m => m.EstoqueProduto)
                .WithMany()
                .HasForeignKey(m => m.EstoqueProdutoId);

            modelBuilder.Entity<Movimentacao>()
                .HasOne(m => m.User)
                .WithMany()
                .HasForeignKey(m => m.UserId);

            modelBuilder.Entity<Produto>() 
                .HasOne(p => p.Categoria)
                .WithMany()
                .HasForeignKey(p => p.CategoriaId);

        }
    }
}

