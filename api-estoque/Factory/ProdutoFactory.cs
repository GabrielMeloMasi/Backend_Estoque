using api_estoque.Models;

namespace api_estoque.Factory
{
    public static  class ProdutoFactory
    {

        public static Produto CriarProduto(string tipo)
        {
            return tipo switch
            {
                "perecivel" => new ProdutoPerecivel(),
                "duravel" => new ProdutoBasic(),
                _ => throw new ArgumentException("Tipo inválido")
            };
        }
    }
}
