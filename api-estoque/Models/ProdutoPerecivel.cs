using System.ComponentModel.DataAnnotations.Schema;

namespace api_estoque.Models
{
    public class ProdutoPerecivel : Produto
    {
        
        public override List<Validade> Validades { get; set; } = new List<Validade>();

        public override bool PossuiValidade()
        {
            return true;
        }
    }
}
