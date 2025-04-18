using api_estoque.DTO;
using api_estoque.Interface;
using api_estoque.Models;
using api_estoque.Padroes.Factory;
using Microsoft.EntityFrameworkCore.Metadata;

namespace api_estoque.Padroes.Facade
{
    public class ProdutoFacade : IProdutoFacade
    {
            private readonly IProdutoRepository _produtoRepository;
            private readonly IEstoqueProdutoRepository _estoqueProdutoRepository;
            private readonly IValidadeRepository _validadeRepository;
            private readonly ICategoriaRepository _categoriaRepository;
            
        public ProdutoFacade(IProdutoRepository produto, IEstoqueProdutoRepository estoque, IValidadeRepository validade) {
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
            throw new NotImplementedException();
        }

        public ProdutoDTO SaidaProduto(SaidaDTO saida)
        {
            throw new NotImplementedException();
        }
    }
}
