using api_estoque.EntityConfig;
using api_estoque.Interface;
using api_estoque.Models;
using Microsoft.AspNetCore.Mvc;

namespace api_estoque.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriaController : ControllerBase
    {
        private readonly ICategoriaRepository _categoriaRepository;

        public CategoriaController(IRepositoryFactory repositoryFactory)
        {
            _categoriaRepository = repositoryFactory.CategoriaRepository();
        }

        [HttpGet("getAll")]
        public ActionResult<List<Categoria>> GetAll()
        {
            var categorias = _categoriaRepository.GetAll();
            return Ok(categorias);
        }

        [HttpGet("{id}")]
        public ActionResult<Categoria> GetById(int id)
        {
            var categoria = _categoriaRepository.GetById(id);
            if (categoria == null)
                return NotFound();

            return Ok(categoria);
        }

        [HttpPost("create")]
        public IActionResult Create([FromBody] string nome)
        {
            if (string.IsNullOrWhiteSpace(nome))
                return BadRequest("Nome da categoria é obrigatório.");

            var novaCategoria = _categoriaRepository.Salvar(nome);

            return CreatedAtAction(nameof(GetById), new { id = novaCategoria.Id }, novaCategoria);
        }

        [HttpPut("editar")]
        public IActionResult Edit([FromBody] Categoria categoria)
        {
            try
            {
                _categoriaRepository.Editar(categoria);
                return Ok();
            }
            catch (Exception ex) {
                return BadRequest(ex);  
            }

        }
    }
}
