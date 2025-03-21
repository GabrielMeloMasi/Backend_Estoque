using System.ComponentModel.DataAnnotations.Schema;

namespace api_estoque.Models
{
    public class Estoque
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }

        [NotMapped]
        public List<EstoqueProduto> EstoqueProdutos { get; set; }


    }
}
