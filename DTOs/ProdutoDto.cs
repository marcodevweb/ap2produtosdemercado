namespace ApiProdutosMercado.DTOs;

public record ProdutoDto(
    int Id,
    string Nome,
    string Categoria,
    string Marca,
    decimal Preco,
    int Estoque
);