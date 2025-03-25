using api_estoque.Interface;

namespace api_estoque.EntityConfig
{
    public interface IRepositoryFactory
    {
        IProdutoRepository CriarProdutoRepository();
        ICategoriaRepository CategoriaRepository();
    }
}
