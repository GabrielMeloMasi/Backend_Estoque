using api_estoque.EntityConfig;
using api_estoque.Interface;
using api_estoque.Models;
using api_estoque.Padroes.Singleton;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace api_estoque.Repository
{
    public class EstoqueProdutoRepository : IEstoqueProdutoRepository
    {
        private readonly AppDbContext _context;
        public EstoqueProdutoRepository(AppDbContext context)
        {
            _context = context;
        }

        public EstoqueProduto Edit(int? IdProduto, int quantidade, double preco)
        {
            try
            {
                EstoqueProduto estprod = _context.EstoqueProdutos.FirstOrDefault(e => e.ProdutoId == IdProduto);

                if (estprod != null)
                {
                    estprod.Quantidade = quantidade;
                    estprod.Preco = preco;

                    _context.Entry(estprod).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                }

                return estprod;
            }
            catch (Exception ex)
            {
               throw ex;
            }
        }

        public EstoqueProduto Entrada(int? IdProduto, int quantidade, double preco)
        {
            try
            {
                EstoqueProduto estprod = _context.EstoqueProdutos.FirstOrDefault(e => e.ProdutoId == IdProduto && e.EstoqueId == EstoqueSingleton.Instance.Estoque.Id);

                if (estprod != null)
                {
                    estprod.Quantidade += quantidade;
                    estprod.Preco = preco;

                    _context.Entry(estprod).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                }
                else
                {
                    estprod = SaveNew(IdProduto, quantidade, preco);
                }

                return estprod;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public EstoqueProduto Saida(int? IdProduto, int quantidade)
        {
            try
            {
                EstoqueProduto estprod = _context.EstoqueProdutos.FirstOrDefault(e => e.ProdutoId == IdProduto);

                if (estprod != null)
                {
                    estprod.Quantidade -= quantidade;

                    _context.Entry(estprod).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                }

                return estprod;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public EstoqueProduto SaveNew(int? IdProduto, int quantidade, double preco)
        {
            try
            {
                EstoqueProduto estoqueprod = new EstoqueProduto
                {
                    EstoqueId = EstoqueSingleton.Instance.Estoque.Id,
                    ProdutoId = (int) IdProduto,
                    Quantidade = quantidade,
                    Preco = preco
                };

                _context.EstoqueProdutos.Add(estoqueprod);
                _context.SaveChanges();

                return estoqueprod;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }
}
