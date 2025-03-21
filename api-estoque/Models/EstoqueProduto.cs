namespace api_estoque.Models
{
    public class EstoqueProduto
    {
        public int EstoqueId { get; set; }
        public Estoque Estoque { get; set; }

        public int ProdutoId { get; set; }
        public Produto Produto { get; set; }
    }
}
