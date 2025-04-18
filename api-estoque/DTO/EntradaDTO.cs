using api_estoque.Models;

namespace api_estoque.DTO
{
    public class EntradaDTO
    {

        public int? Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public int Quantidade { get; set; }
        public double Preco { get; set; }
        public int CategoriaId { get; set; }
        public int TipoProduto { get; set; }
        public DateTime DataValidade { get; set; }
    }
}
