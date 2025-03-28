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

        public IProdutoRepository ProdutoRepository() => new ProdutoRepository(_context);
        public ICategoriaRepository CategoriaRepository() => new CategoriaRepository(_context);
        public IEstoqueRepository EstoqueRepository() => new EstoqueRepository(_context);
        public IUserRepository UserRepository() => new UserRepository(_context, EstoqueRepository());
        public IMovimentacaoRepository MovimentacaoRepository() => new MovimentacaoRepository(_context);

    }
}
