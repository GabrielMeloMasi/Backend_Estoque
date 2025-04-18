using api_estoque.Padroes.Facade;

namespace api_estoque.EntityConfig
{
    public interface IServiceFactory : IRepositoryFactory
    {
        IProdutoFacade ProdutoFacade();
        
    }
}
