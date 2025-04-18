using api_estoque.DTO;

namespace api_estoque.Padroes.Facade
{
    public interface IProdutoFacade
    {
        ProdutoDTO EntradaProduto(EntradaDTO entrada);
        bool SaidaProduto(SaidaDTO saida);
        ProdutoDTO Edit(ProdutoDTO produto);
    }
}
