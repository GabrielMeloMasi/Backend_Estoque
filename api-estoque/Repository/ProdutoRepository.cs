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

       
        public void EntradaProduto(EntradaDTO produto)
        {
            try
            {
                var estoqueId = EstoqueSingleton.Instance.Estoque.Id;

                // Busca o produto existente com estoque vinculado ao estoque atual
                var produtoExistente = _context.Produto.FirstOrDefault(p => p.Id == produto.Id);



                if (produtoExistente != null )
                {
                    var estoqueProduto = _context.EstoqueProdutos
                    .Include(e => e.Validades)
                    .FirstOrDefault(e => e.ProdutoId == produto.Id && e.EstoqueId == estoqueId);

                    // Produto existente no estoque
                    if (produtoExistente.TipoProduto == 1 && produto.DataValidade != null)
                    {
                        
                            var validadeExistente = estoqueProduto.Validades
                                .FirstOrDefault(v => v.DataValidade.Date == produto.DataValidade);

                            if (validadeExistente != null)
                            {
                                validadeExistente.Quantidade += produto.Quantidade;
                            }
                            else
                            {
                                 Validade val = new Validade
                                 {
                                    EstoqueProdutoId = estoqueProduto.Id,
                                    Quantidade = produto.Quantidade,
                                    DataValidade = produto.DataValidade
                                };
                                _context.Validade.Add(val);
                            }
                        
                    }

                    estoqueProduto.Quantidade += produto.Quantidade;

                    _context.SaveChanges();

                    var movimentacao = new MovimentacaoContext();
                    movimentacao.SetStrategy(new MovimentacaoEntradaStrategy(_context));
                    movimentacao.SalvarMovimentacao(estoqueProduto.Id, produto.Quantidade);
                }
                else
                {
                    
                    Produto novoProduto = produto.DataValidade != null
                        ? ProdutoFactory.CriarProduto("perecivel")
                        : ProdutoFactory.CriarProduto("basic");

                    novoProduto.TipoProduto = produto.DataValidade != null ? 1 : 0;
                    novoProduto.Nome = produto.Nome;
                    novoProduto.Descricao = produto.Descricao;
                    novoProduto.CategoriaId = produto.CategoriaId;

                    _context.Produto.Add(novoProduto);
                    _context.SaveChanges();

                    var novoEstoqueProduto = new EstoqueProduto
                    {
                        ProdutoId = novoProduto.Id,
                        EstoqueId = estoqueId,
                        Quantidade = produto.Quantidade,
                        Preco = produto.Preco
                    };

                    _context.EstoqueProdutos.Add(novoEstoqueProduto);
                    _context.SaveChanges();

                    if (novoProduto.TipoProduto == 1 && produto.DataValidade != null)
                    {

                        Validade val = new Validade
                        {
                            EstoqueProdutoId = novoEstoqueProduto.Id,
                            Quantidade = produto.Quantidade,
                            DataValidade = produto.DataValidade
                        };
                        _context.Validade.Add(val);

                        _context.SaveChanges();
                    }

                    var movimentacao = new MovimentacaoContext();
                    movimentacao.SetStrategy(new MovimentacaoEntradaStrategy(_context));
                    movimentacao.SalvarMovimentacao(novoEstoqueProduto.Id, produto.Quantidade);
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


        public void SaidaProduto(SaidaDTO produto)
        {
            var estoqueProduto = _context.EstoqueProdutos
                .Include(e => e.Validades)
                .FirstOrDefault(e => e.ProdutoId == produto.Id && e.EstoqueId == EstoqueSingleton.Instance.Estoque.Id);

            if (estoqueProduto == null)
                throw new Exception("Produto não encontrado no estoque.");

            int quantidadeParaRetirar = produto.Quantidade;

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
            movimentacao.SalvarMovimentacao(estoqueProduto.Id, produto.Quantidade);
        }



    }
}
