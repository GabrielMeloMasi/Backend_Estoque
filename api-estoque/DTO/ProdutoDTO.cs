using api_estoque.Models;

namespace api_estoque.DTO
{
    public class ProdutoDTO
    {

        public int? Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public int QuantTotal { get; set; }
        public double Preco { get; set; }
        public int CategoriaId { get; set; }
        public Categoria? Categoria { get; set; }
        public int TipoProduto { get; set; }
        public List<Validade>? Validades { get; set; } = new();
    }
}
