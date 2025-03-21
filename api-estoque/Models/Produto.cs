namespace api_estoque.Models
{
    public class Produto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public int QuantTotal { get; set; }
        public double Preco { get; set; }

        public int CategoriaId { get; set; }
        public Categoria Categoria { get; set; }
        public virtual List<Validade> Validades { get; set; } = new List<Validade>();
        public virtual List<EstoqueProduto> EstoqueProdutos { get; set; } = new List<EstoqueProduto>();


    }
}
