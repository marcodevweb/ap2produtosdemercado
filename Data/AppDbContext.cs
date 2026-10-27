using ApiProdutosMercado.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiProdutosMercado.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Produto> Produtos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Produto>(entity =>
        {
            entity.HasKey(p => p.Id);

            entity.Property(p => p.Nome)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(p => p.Categoria)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(p => p.Marca)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(p => p.Preco)
                .IsRequired()
                .HasColumnType("decimal(10,2)");

            entity.Property(p => p.Estoque)
                .IsRequired();
        });
    }
}
