using api_estoque.EntityConfig;
using api_estoque.Interface;
using Microsoft.AspNetCore.Mvc;

namespace api_estoque.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EstoqueController : ControllerBase
    {
        private readonly IEstoqueRepository _estoqueRepository;

        public EstoqueController(IRepositoryFactory repositoryFactory)
        {
            _estoqueRepository = repositoryFactory.EstoqueRepository();
        }

        [HttpPost("create/{userId}")]
        public IActionResult CreateEstoque(int userId)
        {
            try
            {
                var estoque = _estoqueRepository.Create(userId);
                return Ok(estoque);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao criar estoque: {ex.Message}");
            }
        }

        [HttpGet("by-user/{userId}")]
        public IActionResult GetEstoqueByUser(int userId)
        {
            var estoque = _estoqueRepository.GetById(userId);
            if (estoque == null)
                return NotFound("Estoque não encontrado para o usuário.");

            return Ok(estoque);
        }

        [HttpGet("getEstoque")]
        public IActionResult GetEstoque()
        {
            try
            {
                var estoqueDTO = _estoqueRepository.GetEstoque();
                return Ok(estoqueDTO);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao obter estoque: {ex.Message}");
            }
        }
    }
}
