using api_estoque.DTO;
using api_estoque.EntityConfig;
using api_estoque.Interface;
using api_estoque.Models;
using System.Text.Json.Nodes;

namespace api_estoque.Repository
{
    public class MovimentacaoRepository : IMovimentacaoRepository
    {
        private readonly AppDbContext _context;
        public MovimentacaoRepository(AppDbContext context)
        {
            _context = context;
        }
        public List<FilterDTO> Filter(JsonObject filterDTO)
        {
            throw new NotImplementedException();
        }

        public List<Movimentacao> GetAll()
        {
            throw new NotImplementedException();
        }

        public List<Movimentacao> GetProduto(int idProduto)
        {
            throw new NotImplementedException();
        }

        public List<Movimentacao> GetTipo(string tipoMovimentacao)
        {
            throw new NotImplementedException();
        }
    }
}
