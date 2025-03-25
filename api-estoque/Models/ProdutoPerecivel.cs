using System.ComponentModel.DataAnnotations.Schema;

namespace api_estoque.Models
{
    public class ProdutoPerecivel : Produto
    {

        public ProdutoPerecivel()
        {
            TipoProduto = 1;
        }

        public override string Tipo() => "Produto Perecível";
    }
}
