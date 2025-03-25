namespace api_estoque.DTO
{
    public class EstoqueDTO
    {
        public int EstoqueId { get; set; } 
        public List<ProdutoDTO> Produtos { get; set; }
    }
}
