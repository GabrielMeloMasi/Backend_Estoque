using api_estoque.DTO;

namespace api_estoque.Interface
{
    public interface IProdutoRepository
    {

        List<ProdutoDTO> GetAllProduto();
        ProdutoDTO GetById(int id);
        List<ProdutoDTO> GetProdutoParaReposicao();
        //void SaveProduto(ProdutoDTO produto);
        void EditProduto(ProdutoDTO produto);
        void DeleteProduto(int id);
        List<ProdutoDTO> ProdutoPorCategoria(int idCategoria);
        void EntradaProduto(ProdutoDTO produto);
        void SaidaProduto(ProdutoDTO produto);


    }
}
