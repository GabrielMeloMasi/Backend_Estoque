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


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=app.db");
        }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EstoqueProduto>().HasKey(ep => new { ep.EstoqueId, ep.ProdutoId });
            modelBuilder.Entity<User>().HasKey(ep => ep.Id);
            modelBuilder.Entity<Estoque>().HasKey(ep => ep.Id);
            modelBuilder.Entity<Produto>().HasKey(ep => ep.Id);
            modelBuilder.Entity<Validade>().HasKey(ep => ep.Id);
            modelBuilder.Entity<Categoria>().HasKey(ep => ep.Id);
            modelBuilder.Entity<Movimentacao>().HasKey(ep => ep.Id);

            modelBuilder.Entity<Validade>()
                .HasOne(v => v.Produto)
                .WithMany(p => p.Validades)
                .HasForeignKey(v => v.ProdutoId)
                .OnDelete(DeleteBehavior.Cascade); 

            modelBuilder.Entity<EstoqueProduto>()
                .HasOne(ep => ep.Estoque)
                .WithMany(e => e.EstoqueProdutos)
                .HasForeignKey(ep => ep.EstoqueId);

            modelBuilder.Entity<EstoqueProduto>()
                .HasOne(ep => ep.Produto)
                .WithMany(p => p.EstoqueProdutos)
                .HasForeignKey(ep => ep.ProdutoId);

            modelBuilder.Entity<Estoque>()
                .HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Movimentacao>()
                .HasOne(m => m.Produto)
                .WithMany()
                .HasForeignKey(m => m.ProdutoId);

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

