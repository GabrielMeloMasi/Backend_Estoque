using api_estoque.Interface;
using api_estoque.Repository;

namespace api_estoque.EntityConfig
{
    public class DbRepositoryFactory : IRepositoryFactory
    {

        private readonly AppDbContext _context;

        public DbRepositoryFactory(AppDbContext context)
        {
            _context = context;
        }

        public IProdutoRepository CriarProdutoRepository() => new ProdutoRepository(_context);
    }
}
