using api_estoque.EntityConfig;
using api_estoque.Interface;

namespace api_estoque.Repository
{
    public class ProdutoRepository : IProdutoRepository
    {

        private readonly AppDbContext _context;

        public ProdutoRepository(AppDbContext context)
        {
            _context = context;
        }
    }
}
