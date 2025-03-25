namespace api_estoque.Models
{
    public class Movimentacao
    {
        public int Id { get; set; }

        public int EstoqueProdutoId { get; set; }
        public EstoqueProduto EstoqueProduto { get; set; }
        public string Tipo { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
        public int Quantidade { get; set; }
    }
}
