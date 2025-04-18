using api_estoque.Models;

namespace api_estoque.Interface
{
    public interface IEstoqueProdutoRepository
    {
        EstoqueProduto SaveNew(int IdProduto, int quantidade, double preco);
        EstoqueProduto Edit(int IdProduto, int quantidade, double preco);
        EstoqueProduto Entrada(int IdProduto, int quantidade, double preco);
        EstoqueProduto Saida (int IdProduto, int quantidade);

    }
}
