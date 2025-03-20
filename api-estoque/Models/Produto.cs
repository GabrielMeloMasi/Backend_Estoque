namespace api_estoque.Models
{
    public abstract class Produto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public int QuantTotal { get; set; }
        public double PrecoCompra { get; set; }
        public double PrecoVenda { get; set; }

        public abstract bool existeProduto();

    }
}
