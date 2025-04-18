using api_estoque.Padroes.Facade;

namespace api_estoque.EntityConfig
{
    public class DbServiceFactory : DbRepositoryFactory, IServiceFactory
    {
        private IProdutoFacade _produtoFacade;
        private readonly AppDbContext _context;

        public DbServiceFactory(AppDbContext context) : base(context) {
            _context = context;
        }

        public IProdutoFacade ProdutoFacade()
        {
            return _produtoFacade ??= new ProdutoFacade( _context, ProdutoRepository(), EstoqueProdutoRepository(), ValidadeRepository());
        }

    }
}
