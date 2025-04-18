using api_estoque.DTO;
using api_estoque.EntityConfig;
using api_estoque.Interface;
using api_estoque.Models;
using api_estoque.Padroes.Factory;
using api_estoque.Padroes.Singleton;
using api_estoque.Padroes.Strategy;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace api_estoque.Repository
{
    public class ProdutoRepository : IProdutoRepository
    {

        private readonly AppDbContext _context;
        private readonly IMovimentacaoRepository _movimentacaoRepository;
        private readonly IValidadeRepository _validateRepository;

        public ProdutoRepository(AppDbContext context, IMovimentacaoRepository movimentacaoRepository, IValidadeRepository validadeRepository)
        {
            _context = context;
            _movimentacaoRepository = movimentacaoRepository;
            _validateRepository = validadeRepository;

        }

        public void DeleteProduto(int id)
        {
            var produto = _context.Produto
                .Include(p => p.EstoqueProduto)
                    .ThenInclude(e => e.Validades)
                .FirstOrDefault(p => p.Id == id);

            if (produto == null)
                throw new Exception("Produto não encontrado.");
           
            _context.Validade.RemoveRange(produto.EstoqueProduto.Validades);

            _context.EstoqueProdutos.Remove(produto.EstoqueProduto);

            _context.Produto.Remove(produto);

            
            _context.SaveChanges();
        }


        //public void EditProduto(ProdutoDTO produto)
        //{
        //    try
        //    {
        //        Produto newProduto;
        //        if (produto.Validades.Any())
        //        {
        //            newProduto = ProdutoFactory.CriarProduto("perecivel");

        //            newProduto.TipoProduto = 1;
        //            foreach (Validade val in produto.Validades)
        //            {
        //                val.EstoqueProduto.ProdutoId = produto.Id;
        //                val.EstoqueProduto.EstoqueId = EstoqueSingleton.Instance.Estoque.Id;
        //                _validateRepository.Editar(val);
        //            }
        //        }
        //        else
        //        {
        //            newProduto = ProdutoFactory.CriarProduto("basic");
        //            newProduto.TipoProduto = 0;
        //        }

        //        newProduto.Id = produto.Id;
        //        newProduto.CategoriaId = produto.CategoriaId;
        //        newProduto.Nome = produto.Nome;
        //        newProduto.Descricao = produto.Descricao;

        //        _context.Entry(newProduto).State = EntityState.Modified;
        //        _context.SaveChanges();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Erro ao editar o produto ", ex);
        //    }

        //}


        public Produto EditProduto(Produto produto)
        {
            try
            {
               Produto prod =  _context.Produto.FirstOrDefault(p => p.Id == produto.Id);

                if(prod != null)
                {
                    prod.Descricao = produto.Descricao;
                    prod.CategoriaId = produto.CategoriaId;
                    prod.Nome = produto.Nome;

                    _context.Entry(prod).State = EntityState.Modified;
                    _context.SaveChanges();
                }


                return prod;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao editar o produto ", ex);
            }

        }


        public Produto EntradaProduto(Produto produto)
        {
            try
            {
                var estoqueId = EstoqueSingleton.Instance.Estoque.Id;

                var produtoExistente = _context.Produto.FirstOrDefault(p => p.Id == produto.Id);



                if (produtoExistente != null)
                {
                    produtoExistente.Descricao = produto.Descricao;
                    produtoExistente.Nome = produto.Nome;
                    produtoExistente.TipoProduto = produto.TipoProduto;
                    produtoExistente.CategoriaId = produto.CategoriaId;

                    _context.Entry(produtoExistente).State = EntityState.Modified;
                    _context.SaveChanges();
                    return produtoExistente;
                }
                else
                {
                    produto.Id = null;
                    _context.Produto.Add(produto);
                    _context.SaveChanges();

                    return produto;
                }

                return null;
            }
            catch (Exception e)
            {
                throw new Exception("Falha ao realizar a entrada de produto.", e);
            }
        }



        public List<ProdutoDTO> GetAllProduto()
        {
            return _context.Produto
            .Include(p => p.Categoria)
            .Include(p => p.EstoqueProduto)
            .ThenInclude(e => e.Validades)
            .Select(p => new ProdutoDTO
            {
             Id = p.Id,
             Nome = p.Nome,
             Descricao = p.Descricao,
             CategoriaId = p.CategoriaId,
             Categoria = p.Categoria,
             TipoProduto = p.TipoProduto,
             Preco = p.EstoqueProduto != null ? p.EstoqueProduto.Preco : 0,
             QuantTotal = p.EstoqueProduto.Quantidade,
             Validades = p.TipoProduto == 1
                 ? p.EstoqueProduto.Validades.ToList()
                 : null
            })
            .ToList();

        }

        public ProdutoDTO GetById(int id)
        {
            return _context.Produto
                .Include(p => p.Categoria)
                .Include(p => p.EstoqueProduto)
                    .ThenInclude(e => e.Validades)
                .Where(p => p.Id == id)
                .Select(p => new ProdutoDTO
                {
                    Id = p.Id,
                    Nome = p.Nome,
                    Descricao = p.Descricao,
                    CategoriaId = p.CategoriaId,
                    Categoria = p.Categoria,
                    TipoProduto = p.TipoProduto,
                    Preco = p.EstoqueProduto != null ? p.EstoqueProduto.Preco : 0,
                    QuantTotal = p.EstoqueProduto.Quantidade,
                    Validades = p.TipoProduto == 1
                      ? p.EstoqueProduto.Validades.ToList()
                        : null
                })
                .FirstOrDefault();
        }


        public List<ProdutoDTO> GetProdutoParaReposicao()
        {
            return _context.Produto
                .Include(p => p.EstoqueProduto)
                    .ThenInclude(e => e.Validades)
                .Where(p => p.EstoqueProduto.Quantidade < 2)
                .Select(p => new ProdutoDTO
                {
                    Id = p.Id,
                    Nome = p.Nome,
                    Descricao = p.Descricao,
                    CategoriaId = p.CategoriaId,
                    TipoProduto = p.TipoProduto,
                    Preco = p.EstoqueProduto != null ? p.EstoqueProduto.Preco : 0,
                    QuantTotal = p.EstoqueProduto.Quantidade,
                    Validades = p.TipoProduto == 1
                        ? p.EstoqueProduto.Validades.ToList()
                        : null
                })
                .ToList();
        }


        public List<ProdutoDTO> ProdutoPorCategoria(int idCategoria)
        {
            return _context.Produto
                .Include(p => p.Categoria)
                .Include(p => p.EstoqueProduto)
                    .ThenInclude(e => e.Validades)
                .Where(p => p.CategoriaId == idCategoria)
                .Select(p => new ProdutoDTO
                {
                    Id = p.Id,
                    Nome = p.Nome,
                    Descricao = p.Descricao,
                    CategoriaId = p.CategoriaId,
                    Categoria = p.Categoria,
                    TipoProduto = p.TipoProduto,
                    Preco = p.EstoqueProduto != null ? p.EstoqueProduto.Preco : 0,
                    QuantTotal = p.EstoqueProduto.Quantidade,
                    Validades = p.TipoProduto == 1
                        ? p.EstoqueProduto.Validades.ToList()
                        : null
                })
                .ToList();
        }
    }
}
