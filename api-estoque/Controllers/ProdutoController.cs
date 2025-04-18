using api_estoque.DTO;
using api_estoque.EntityConfig;
using api_estoque.Interface;
using api_estoque.Padroes.Facade;
using Microsoft.AspNetCore.Mvc;

namespace api_estoque.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProdutoController : ControllerBase
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly IProdutoFacade _produtoFacade;

        public ProdutoController(IRepositoryFactory repositoryFactory, IServiceFactory serviceFactory)
        {
            _produtoRepository = repositoryFactory.ProdutoRepository();
            _produtoFacade = serviceFactory.ProdutoFacade();
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var produtos = _produtoRepository.GetAllProduto();
            return Ok(produtos);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var produto = _produtoRepository.GetById(id);
            if (produto == null)
                return NotFound();

            return Ok(produto);
        }

        [HttpGet("reposicao")]
        public IActionResult GetProdutosParaReposicao()
        {
            var produtos = _produtoRepository.GetProdutoParaReposicao();
            return Ok(produtos);
        }

        [HttpGet("categoria/{idCategoria}")]
        public IActionResult GetProdutosPorCategoria(int idCategoria)
        {
            var produtos = _produtoRepository.ProdutoPorCategoria(idCategoria);
            return Ok(produtos);
        }

        [HttpPost("entrada")]
        public IActionResult EntradaProduto([FromBody] EntradaDTO produto)
        {
            try
            {
                return Created("Produto/{id}", _produtoFacade.EntradaProduto(produto));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("saida")]
        public IActionResult SaidaProduto([FromBody] SaidaDTO produto)
        {
            try
            {
                _produtoFacade.SaidaProduto(produto);
                return Ok("Saída realizada com sucesso.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public IActionResult EditarProduto([FromBody] ProdutoDTO produto)
        {
            try
            {
                _produtoFacade.Edit(produto);
                return Ok("Produto editado com sucesso.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeletarProduto(int id)
        {
            try
            {
                _produtoRepository.DeleteProduto(id);
                return Ok("Produto deletado com sucesso.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
