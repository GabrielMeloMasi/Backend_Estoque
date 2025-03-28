using api_estoque.DTO;
using api_estoque.EntityConfig;
using api_estoque.Interface;
using api_estoque.Models;
using api_estoque.Padroes.Singleton;

namespace api_estoque.Repository
{
    public class EstoqueRepository : IEstoqueRepository
    {
        private readonly AppDbContext _context;
        
        public EstoqueRepository(AppDbContext context)
        {
            _context = context;
        }

        public Estoque Create(int userId)
        {
            Estoque estoque = new Estoque { 
            UserId = userId
            };

            _context.Estoque.Add(estoque);
            _context.SaveChanges();

            return estoque;

        }

        public Estoque GetById(int userId)
        {
            return _context.Estoque.FirstOrDefault(e => e.UserId == userId);
        }

        public EstoqueDTO GetEstoque()
        {
            List<EstoqueProduto> produtos = _context.EstoqueProdutos.Where(p => p.EstoqueId == EstoqueSingleton.Instance.Estoque.Id).ToList();

            if (produtos != null)
            {
                List<ProdutoDTO> produtosDTO = new List<ProdutoDTO>();
                foreach (var produto in produtos)
                {
                    var produt = new ProdutoDTO
                    {
                        Id = produto.Id,
                        Nome = produto.Produto.Nome,
                        Descricao = produto.Produto.Descricao,
                        QuantTotal = produto.Quantidade,
                        Preco = produto.Preco,
                        CategoriaId = produto.Produto.CategoriaId,
                        TipoProduto = produto.Produto.TipoProduto,
                        Validades = produto.Validades
                    };
                    produtosDTO.Add(produt);
                }

                return new EstoqueDTO
                {
                    Produtos = produtosDTO,
                    EstoqueId = EstoqueSingleton.Instance.Estoque.Id
                };
            }

            return new EstoqueDTO
            {
                Produtos = null,
                EstoqueId = -1
            };
        }


    }
}
