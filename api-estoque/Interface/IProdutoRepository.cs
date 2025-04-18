using api_estoque.DTO;
using api_estoque.Models;

namespace api_estoque.Interface
{
    public interface IProdutoRepository
    {

        List<ProdutoDTO> GetAllProduto();
        ProdutoDTO GetById(int id);
        List<ProdutoDTO> GetProdutoParaReposicao();
        //void SaveProduto(ProdutoDTO produto);
        Produto EditProduto(Produto produto);
        void DeleteProduto(int id);
        List<ProdutoDTO> ProdutoPorCategoria(int idCategoria);
        Produto EntradaProduto(Produto produto);


    }
}
