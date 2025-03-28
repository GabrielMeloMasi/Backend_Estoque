using api_estoque.Interface;

namespace api_estoque.EntityConfig
{
    public interface IRepositoryFactory
    {
        IProdutoRepository ProdutoRepository();
        ICategoriaRepository CategoriaRepository();
        IEstoqueRepository EstoqueRepository();
        IUserRepository UserRepository();
        IMovimentacaoRepository MovimentacaoRepository();
    }
}
