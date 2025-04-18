using api_estoque.DTO;
using api_estoque.EntityConfig;
using api_estoque.Interface;
using api_estoque.Models;
using api_estoque.Padroes.Factory;
using api_estoque.Padroes.Singleton;
using api_estoque.Padroes.Strategy;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace api_estoque.Padroes.Facade
{
    public class ProdutoFacade : IProdutoFacade
    {
            private readonly IProdutoRepository _produtoRepository;
            private readonly IEstoqueProdutoRepository _estoqueProdutoRepository;
            private readonly IValidadeRepository _validadeRepository;
            private readonly ICategoriaRepository _categoriaRepository;
            private AppDbContext _context;
        public ProdutoFacade( AppDbContext context, IProdutoRepository produto, IEstoqueProdutoRepository estoque, IValidadeRepository validade) {
            _context = context;
            _produtoRepository = produto;
            _estoqueProdutoRepository = estoque;
            _validadeRepository = validade;
        }
        public ProdutoDTO Edit(ProdutoDTO produto)
        {
            try
            {
                Produto editProd = produto.TipoProduto == 1 ? ProdutoFactory.CriarProduto("perecivel") : ProdutoFactory.CriarProduto("basic");

                editProd.Id = produto.Id;
                editProd.Descricao = produto.Descricao;
                editProd.TipoProduto = produto.TipoProduto;
                editProd.CategoriaId = produto.CategoriaId;
                editProd.Nome = produto.Nome;

                Produto produtoBanco = _produtoRepository.EditProduto(editProd);
                EstoqueProduto estoqueprod = _estoqueProdutoRepository.Edit(produtoBanco.Id, produto.QuantTotal, produto.Preco);

                List<Validade> validades = null;
                if(produto.TipoProduto == 1)
                {
                    validades = _validadeRepository.Editar(estoqueprod.Id, produto.Validades);
                }

                return new ProdutoDTO
                {
                    Id = editProd.Id,
                    Nome = editProd.Nome,
                    Descricao = editProd.Descricao,
                    CategoriaId = editProd.CategoriaId,
                    Preco = estoqueprod.Preco,
                    QuantTotal = estoqueprod.Quantidade,
                    TipoProduto = editProd.TipoProduto,
                    Validades = validades
                };
            }
            catch (Exception ex) {
                throw ex;
            }

        }

        public ProdutoDTO EntradaProduto(EntradaDTO entrada)
        {

            try
            {
                Produto newProduto = entrada.TipoProduto == 1 ? ProdutoFactory.CriarProduto("perecivel") : ProdutoFactory.CriarProduto("basic");

                newProduto.Id = entrada.Id;
                newProduto.Descricao = entrada.Descricao;
                newProduto.TipoProduto = entrada.TipoProduto;
                newProduto.CategoriaId = entrada.CategoriaId;
                newProduto.Nome = entrada.Nome;


                Produto prodcriado = _produtoRepository.EntradaProduto(newProduto);
                EstoqueProduto estoqueProduto = _estoqueProdutoRepository.Entrada(prodcriado.Id, entrada.Quantidade, entrada.Preco);

                Validade val = null;
                if (entrada.TipoProduto == 1)
                {
                    val = _validadeRepository.Save(estoqueProduto.Id, entrada.DataValidade, entrada.Quantidade);
                }


                var movimentacao = new MovimentacaoContext();
                movimentacao.SetStrategy(new MovimentacaoEntradaStrategy(_context));
                movimentacao.SalvarMovimentacao(estoqueProduto.Id, entrada.Quantidade);

                return new ProdutoDTO
                {
                    Id = newProduto.Id,
                    Nome = newProduto.Nome,
                    Descricao = newProduto.Descricao,
                    CategoriaId = newProduto.CategoriaId,
                    Preco = estoqueProduto.Preco,
                    QuantTotal = estoqueProduto.Quantidade,
                    TipoProduto = newProduto.TipoProduto,
                    Validades = newProduto.TipoProduto == 1 ? _validadeRepository.GetValidadeList(estoqueProduto.Id) : null
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        public bool SaidaProduto(SaidaDTO saida)
        {
            try
            {
                EstoqueProduto estoqProd = _context.EstoqueProdutos.FirstOrDefault(e => e.ProdutoId == saida.Id && e.EstoqueId == EstoqueSingleton.Instance.Estoque.Id);

                if (estoqProd != null && estoqProd.Quantidade >= saida.Quantidade) {

                    EstoqueProduto estprodAtt = _estoqueProdutoRepository.Saida(saida.Id, saida.Quantidade);
                    bool val = _validadeRepository.Saida(estoqProd.Id, saida.Quantidade);

                    return val;                
                }
                return false;
            }
            catch (Exception e) 
            {
                throw new Exception("Quantidade de Saida maior que quantidade no estoque", e);
            }
        }
    }
}
