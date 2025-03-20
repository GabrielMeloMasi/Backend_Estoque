using System.ComponentModel.DataAnnotations.Schema;

namespace api_estoque.Models
{
    public class ProdutoPerecivel : Produto
    {
        [NotMappedAttribute]
        List<Validade> validades = new List<Validade>();
        public override bool existeProduto()
        {
            throw new NotImplementedException();
        }
    }
}
