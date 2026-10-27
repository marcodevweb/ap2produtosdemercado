namespace ApiProdutosMercado.DTOs;

public record ProdutoInputDto(
    string Nome,
    string Categoria,
    string Marca,
    decimal Preco,
    int Estoque
);