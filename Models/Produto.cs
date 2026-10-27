using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiProdutosMercado.Models;

public class Produto
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "O nome é obrigatório.")]
    [MaxLength(200, ErrorMessage = "O nome deve ter no máximo 200 caracteres.")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "A categoria é obrigatória.")]
    [MaxLength(100, ErrorMessage = "A categoria deve ter no máximo 100 caracteres.")]
    public string Categoria { get; set; } = string.Empty;

    [Required(ErrorMessage = "A marca é obrigatória.")]
    [MaxLength(100, ErrorMessage = "A marca deve ter no máximo 100 caracteres.")]
    public string Marca { get; set; } = string.Empty;

    [Column(TypeName = "decimal(10,2)")]
    [Range(0, double.MaxValue, ErrorMessage = "O preço deve ser um valor não negativo.")]
    public decimal Preco { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "O estoque deve ser um valor não negativo.")]
    public int Estoque { get; set; }
}
