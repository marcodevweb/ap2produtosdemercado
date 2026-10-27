using ApiProdutosMercado.Data;
using ApiProdutosMercado.DTOs;
using ApiProdutosMercado.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add DbContext with SQLite
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Ensure database is created and migrations are applied on startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.MigrateAsync();

    // Seed initial data if table is empty
    if (!await db.Produtos.AnyAsync())
    {
        db.Produtos.AddRange(
            new Produto { Nome = "Arroz Branco 5kg", Categoria = "Alimentos", Marca = "Tio João", Preco = 29.90m, Estoque = 35 },
            new Produto { Nome = "Leite Integral 1L", Categoria = "Laticínios", Marca = "Elegê", Preco = 5.49m, Estoque = 80 }
        );
        await db.SaveChangesAsync();
    }
}

// Enable static files for frontend in wwwroot
app.UseDefaultFiles();
app.UseStaticFiles();

// API Endpoints
app.MapGet("/api/produtos", async (AppDbContext db) =>
{
    var produtos = await db.Produtos.ToListAsync();
    var produtosDto = produtos.Select(p => new ProdutoDto(p.Id, p.Nome, p.Categoria, p.Marca, p.Preco, p.Estoque));
    return Results.Ok(produtosDto);
});

app.MapGet("/api/produtos/{id}", async (int id, AppDbContext db) =>
{
    var p = await db.Produtos.FindAsync(id);
    if (p is null) return Results.NotFound();

    var dto = new ProdutoDto(p.Id, p.Nome, p.Categoria, p.Marca, p.Preco, p.Estoque);
    return Results.Ok(dto);
});

app.MapPost("/api/produtos", async (ProdutoInputDto entrada, AppDbContext db) =>
{
    if (string.IsNullOrWhiteSpace(entrada.Nome) ||
        string.IsNullOrWhiteSpace(entrada.Categoria) ||
        string.IsNullOrWhiteSpace(entrada.Marca) ||
        entrada.Preco < 0 ||
        entrada.Estoque < 0)
    {
        return Results.BadRequest(new { mensagem = "Dados inválidos para o produto." });
    }

    var novoProduto = new Produto
    {
        Nome = entrada.Nome.Trim(),
        Categoria = entrada.Categoria.Trim(),
        Marca = entrada.Marca.Trim(),
        Preco = entrada.Preco,
        Estoque = entrada.Estoque
    };

    db.Produtos.Add(novoProduto);
    await db.SaveChangesAsync();

    var dto = new ProdutoDto(novoProduto.Id, novoProduto.Nome, novoProduto.Categoria, novoProduto.Marca, novoProduto.Preco, novoProduto.Estoque);
    return Results.Created($"/api/produtos/{novoProduto.Id}", dto);
});

app.MapPut("/api/produtos/{id}", async (int id, ProdutoInputDto input, AppDbContext db) =>
{
    if (string.IsNullOrWhiteSpace(input.Nome) ||
        string.IsNullOrWhiteSpace(input.Categoria) ||
        string.IsNullOrWhiteSpace(input.Marca) ||
        input.Preco < 0 ||
        input.Estoque < 0)
    {
        return Results.BadRequest(new { mensagem = "Dados inválidos para atualização." });
    }

    var produtoExistente = await db.Produtos.FindAsync(id);
    if (produtoExistente is null) return Results.NotFound();

    produtoExistente.Nome = input.Nome.Trim();
    produtoExistente.Categoria = input.Categoria.Trim();
    produtoExistente.Marca = input.Marca.Trim();
    produtoExistente.Preco = input.Preco;
    produtoExistente.Estoque = input.Estoque;

    await db.SaveChangesAsync();

    var dto = new ProdutoDto(produtoExistente.Id, produtoExistente.Nome, produtoExistente.Categoria, produtoExistente.Marca, produtoExistente.Preco, produtoExistente.Estoque);
    return Results.Ok(dto);
});

app.MapDelete("/api/produtos/{id}", async (int id, AppDbContext db) =>
{
    var produto = await db.Produtos.FindAsync(id);
    if (produto is null) return Results.NotFound();

    db.Produtos.Remove(produto);
    await db.SaveChangesAsync();

    return Results.NoContent();
});

// Fallback to index.html for SPA frontend routing if needed
app.MapFallbackToFile("index.html");

app.Run();
