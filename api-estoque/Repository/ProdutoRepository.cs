using api_estoque.DTO;
using api_estoque.EntityConfig;
using api_estoque.Interface;
using api_estoque.Models;
using api_estoque.Padroes.Factory;
using api_estoque.Padroes.Singleton;
using api_estoque.Padroes.Strategy;
using api_estoque.Padroes.TemplateMethod;
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


        public void EditProduto(ProdutoDTO produto)
        {
            try
            {
                Produto newProduto;
                if (produto.Validades.Any())
                {
                    newProduto = ProdutoFactory.CriarProduto("perecivel");

                    newProduto.TipoProduto = 1;
                    foreach (Validade val in produto.Validades)
                    {
                        val.EstoqueProduto.ProdutoId = produto.Id;
                        val.EstoqueProduto.EstoqueId = EstoqueSingleton.Instance.Estoque.Id;
                        _validateRepository.Editar(val);
                    }
                }
                else
                {
                    newProduto = ProdutoFactory.CriarProduto("basic");
                    newProduto.TipoProduto = 0;
                }

                newProduto.Id = produto.Id;
                newProduto.CategoriaId = produto.CategoriaId;
                newProduto.Nome = produto.Nome;
                newProduto.Descricao = produto.Descricao;

                _context.Entry(newProduto).State = EntityState.Modified;
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao editar o produto ", ex);
            }

        }

        public void EntradaProduto(ProdutoDTO produto)
        {
            try
            {
                // Verifica se o produto já existe
                var produtoExistente = _context.Produto
                    .Include(p => p.EstoqueProduto)
                        .ThenInclude(e => e.Validades)
                    .FirstOrDefault(p => p.Id == produto.Id);

                if (produtoExistente != null)
                {

                    var estoque = produtoExistente.EstoqueProduto;

                    if (produtoExistente.TipoProduto == 1 && produto.Validades.Any())
                    {
                        foreach (var novaValidade in produto.Validades)
                        {
                            
                            var validadeExistente = estoque.Validades
                                .FirstOrDefault(v => v.DataValidade.Date == novaValidade.DataValidade.Date);

                            if (validadeExistente != null)
                            {
                                validadeExistente.Quantidade += novaValidade.Quantidade;
                            }
                            else
                            {
                                novaValidade.EstoqueProdutoId = estoque.Id;
                                _context.Validade.Add(novaValidade);
                            }
                        }
                    }

                    estoque.Quantidade += produto.QuantTotal;

                    _context.SaveChanges();

                   
                    var movimentacao = new MovimentacaoContext();
                    movimentacao.SetStrategy(new MovimentacaoEntradaStrategy(_context));
                    movimentacao.SalvarMovimentacao(estoque.Id, produto.QuantTotal);
                }
                else
                {
                    
                    Produto novoProduto = produto.Validades.Any()
                        ? ProdutoFactory.CriarProduto("perecivel")
                        : ProdutoFactory.CriarProduto("basic");

                    novoProduto.TipoProduto = produto.Validades.Any() ? 1 : 0;
                    novoProduto.Nome = produto.Nome;
                    novoProduto.Descricao = produto.Descricao;
                    novoProduto.CategoriaId = produto.CategoriaId;

                    _context.Produto.Add(novoProduto);
                    _context.SaveChanges();

                    var novoEstoque = new EstoqueProduto
                    {
                        ProdutoId = novoProduto.Id,
                        EstoqueId = EstoqueSingleton.Instance.Estoque.Id,
                        Quantidade = produto.QuantTotal,
                        Preco = produto.Preco
                    };

                    _context.EstoqueProdutos.Add(novoEstoque);
                    _context.SaveChanges();

                    if (novoProduto.TipoProduto == 1)
                    {
                        foreach (var validade in produto.Validades)
                        {
                            validade.EstoqueProdutoId = novoEstoque.Id;
                            _context.Validade.Add(validade);
                        }
                        _context.SaveChanges();
                    }

                    var movimentacao = new MovimentacaoContext();
                    movimentacao.SetStrategy(new MovimentacaoEntradaStrategy(_context));
                    movimentacao.SalvarMovimentacao(novoEstoque.Id, produto.QuantTotal);
                }
            }
            catch (Exception e)
            {
                throw new Exception("Falha ao realizar a entrada de produto.", e);
            }
        }


        private void SetNewEstoqueProduto(int idProduto)
        {
            EstoqueProduto estoqueprod = new EstoqueProduto
            {
                EstoqueId = EstoqueSingleton.Instance.Estoque.Id,
                ProdutoId = idProduto,
            };

            _context.EstoqueProdutos.Add(estoqueprod);
            _context.SaveChanges();
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
                .Include(p => p.Categoria)
                .Include(p => p.EstoqueProduto)
                    .ThenInclude(e => e.Validades)
                .Where(p => p.EstoqueProduto.Quantidade < 2)
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


        public void SaidaProduto(ProdutoDTO produto)
        {
            var estoqueProduto = _context.EstoqueProdutos
                .Include(e => e.Validades)
                .FirstOrDefault(e => e.ProdutoId == produto.Id);

            if (estoqueProduto == null)
                throw new Exception("Produto não encontrado no estoque.");

            int quantidadeParaRetirar = produto.QuantTotal;

            var validadesOrdenadas = estoqueProduto.Validades
                .OrderBy(v => v.DataValidade)
                .ToList();

            foreach (var validade in validadesOrdenadas)
            {
                if (quantidadeParaRetirar <= 0)
                    break;

                if (validade.Quantidade <= quantidadeParaRetirar)
                {
                    quantidadeParaRetirar -= validade.Quantidade;
                    _context.Validade.Remove(validade);
                }
                else
                {
                    validade.Quantidade -= quantidadeParaRetirar;
                    quantidadeParaRetirar = 0;
                }
            }


            estoqueProduto.Quantidade = estoqueProduto.Validades.Sum(v => v.Quantidade);

            _context.SaveChanges();

            //Strategy
            var movimentacao = new MovimentacaoContext();
            movimentacao.SetStrategy(new MovimentacaoSaidaStrategy(_context));
            movimentacao.SalvarMovimentacao(estoqueProduto.Id, produto.QuantTotal);
        }



    }
}
