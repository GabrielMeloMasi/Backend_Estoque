using api_estoque.DTO;
using api_estoque.EntityConfig;
using api_estoque.Interface;
using api_estoque.Models;
using api_estoque.Padroes.Factory;
using api_estoque.Padroes.Singleton;
using api_estoque.Padroes.TemplateMethod;
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
            throw new NotImplementedException();
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

                _context.Produto.Update(newProduto);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao editar o produto ", ex);
            }

        }

        public void EntradaProduto(ProdutoDTO produto)
        {
            throw new NotImplementedException();
        }

        public List<ProdutoDTO> GetAllProduto()
        {
            throw new NotImplementedException();
        }

        public ProdutoDTO GetById(int id)
        {
            throw new NotImplementedException();
        }

        public List<ProdutoDTO> GetProdutoParaReposicao()
        {
            throw new NotImplementedException();
        }

        public List<ProdutoDTO> ProdutoPorCategoria(int idCategoria)
        {
            throw new NotImplementedException();
        }

        public void SaidaProduto(ProdutoDTO produto)
        {
            throw new NotImplementedException();
        }
    }
}
