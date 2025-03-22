namespace api_estoque.Models
{
    public class ProdutoBasic : Produto
    {
        public override bool PossuiValidade()
        {
            return false;
        }
    }
}
