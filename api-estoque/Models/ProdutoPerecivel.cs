using System.ComponentModel.DataAnnotations.Schema;

namespace api_estoque.Models
{
    public class ProdutoPerecivel : Produto
    {

        public List<Validade> Validades { get; set; } = new();

        public ProdutoPerecivel()
        {
            TipoProduto = "perecivel";
        }

        public override string Tipo() => "Produto Perecível";
    }
}
