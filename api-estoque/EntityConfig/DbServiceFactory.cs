using api_estoque.Padroes.Facade;

namespace api_estoque.EntityConfig
{
    public class DbServiceFactory : DbRepositoryFactory, IServiceFactory
    {
        private IProdutoFacade _produtoFacade;

        public DbServiceFactory(AppDbContext context) : base(context) { }

        public IProdutoFacade ProdutoFacade()
        {
            return _produtoFacade ??= new ProdutoFacade(ProdutoRepository(), EstoqueProdutoRepository(), ValidadeRepository());
        }

    }
}
