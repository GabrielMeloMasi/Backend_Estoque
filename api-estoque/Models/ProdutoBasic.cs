namespace api_estoque.Models
{
    public class ProdutoBasic : Produto
    {
        public ProdutoBasic()
        {
            TipoProduto = "basic";
        }
        public override string Tipo() => "Produto Básico";
    }
}
