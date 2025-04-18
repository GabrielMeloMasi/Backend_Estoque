using api_estoque.DTO;
using api_estoque.EntityConfig;
using api_estoque.Interface;
using api_estoque.Models;
using api_estoque.Repository;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace api_estoque.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController: ControllerBase
    {

        private readonly IUserRepository _userRepository;

        public UserController(IRepositoryFactory repositoryFactory)
        {
            _userRepository = repositoryFactory.UserRepository();
        }

        [HttpPost("login")]
        public IActionResult Login(UserLoginDTO user)
        {
            try
            {
                return Ok(_userRepository.LoginUser(user));
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("getAll")]
        public IActionResult GetAll()
        {
            return Ok(_userRepository.GetAll());
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var user = _userRepository.GetById(id);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpPost("create")]
        public IActionResult Create([FromBody] User user)
        {
            try
            {
                var newUser = _userRepository.Salvar(user);

                return CreatedAtAction(nameof(GetById), new { id = newUser.Id }, newUser);
            }
            catch (Exception ex) { 
                return BadRequest(ex);
            }

        }

        [HttpPut("editar")]
        public IActionResult Edit([FromBody] User user)
        {
            try
            {
                _userRepository.Editar(user);
                return Ok();
            }
            catch (Exception ex) { 
                return BadRequest(ex);
            }
        }
    }
}
