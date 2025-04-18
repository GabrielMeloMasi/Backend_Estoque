using api_estoque.DTO;
using api_estoque.EntityConfig;
using api_estoque.Interface;
using Microsoft.AspNetCore.Mvc;

namespace api_estoque.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProdutoController : ControllerBase
    {
        private readonly IProdutoRepository _produtoRepository;

        public ProdutoController(IRepositoryFactory repositoryFactory)
        {
            _produtoRepository = repositoryFactory.ProdutoRepository();
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
        public IActionResult EntradaProduto([FromBody] ProdutoDTO produto)
        {
            try
            {
                _produtoRepository.EntradaProduto(produto);
                return Ok("Entrada realizada com sucesso.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("saida")]
        public IActionResult SaidaProduto([FromBody] ProdutoDTO produto)
        {
            try
            {
                _produtoRepository.SaidaProduto(produto);
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
                _produtoRepository.EditProduto(produto);
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
