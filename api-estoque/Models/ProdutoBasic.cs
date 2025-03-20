namespace api_estoque.Models
{
    public class ProdutoBasic : Produto
    {
        public override bool existeProduto()
        {
            return QuantTotal > 0;
        }
    }
}
