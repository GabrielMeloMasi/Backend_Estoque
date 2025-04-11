using api_estoque.DTO;
using api_estoque.EntityConfig;
using api_estoque.Interface;
using api_estoque.Models;
using api_estoque.Padroes.Singleton;
using Microsoft.EntityFrameworkCore;
using System.Linq;
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
        public List<Movimentacao> GetAll()
        {
            return _context.Movimentacao
                .Where(m => m.UserId == UserSingleton.Instance.Usuario.Id)
                .Include(m => m.EstoqueProduto)
                .ThenInclude(ep => ep.Produto)
                .Include(m => m.User).ToList();
        }

        public List<Movimentacao> GetProduto(int idProduto)
        {
            return _context.Movimentacao
                .Where(m => m.EstoqueProduto.ProdutoId == idProduto)
                .Include(m => m.EstoqueProduto)
                    .ThenInclude(ep => ep.Produto)
                .Include(m => m.User)
                .ToList();
        }


        public List<Movimentacao> GetTipo(string tipoMovimentacao)
        {
            return _context.Movimentacao
            .Where(m => m.Tipo == tipoMovimentacao)
            .Include(m => m.EstoqueProduto)
            .ThenInclude(ep => ep.Produto)
            .Include(m => m.User)
            .ToList();
        }

        //adequar de alguma forma ao GOF
        private void SaveEntrada(int estoqueProdutoId, int quantidade)
        {
            try
            {
                Movimentacao movimentacao = new Movimentacao
                {
                    UserId = UserSingleton.Instance.Usuario.Id,
                    Tipo = "E",
                    EstoqueProdutoId = estoqueProdutoId,
                    Quantidade = quantidade

                };

                _context.Movimentacao.Add(movimentacao);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao buscar as movimentações de entrada.", ex);
            }
        }

        private void SaveSaida(int estoqueProdutoId, int quantidade)
        {
            try
            {
                Movimentacao movimentacao = new Movimentacao
                {
                    UserId = UserSingleton.Instance.Usuario.Id,
                    Tipo = "S",
                    EstoqueProdutoId = estoqueProdutoId,
                    Quantidade = quantidade

                };

                _context.Add(movimentacao);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao buscar as movimentações de saída.", ex);
            }
        }
    }
}
