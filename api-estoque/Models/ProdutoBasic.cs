namespace api_estoque.Models
{
    public class ProdutoBasic : Produto
    {
        public ProdutoBasic()
        {
            TipoProduto = 0;
        }
        public override string Tipo() => "Produto Básico";
    }
}
