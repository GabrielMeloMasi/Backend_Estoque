using api_estoque.Models;

namespace api_estoque.Padroes.Factory
{
    public static class ProdutoFactory
    {

        public static Produto CriarProduto(string tipo)
        {
            return tipo switch
            {
                "perecivel" => new ProdutoPerecivel(),
                "basic" => new ProdutoBasic(),
                _ => throw new ArgumentException("Tipo inválido")
            };
        }
    }
}
