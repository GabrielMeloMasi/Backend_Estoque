using api_estoque.DTO;
using api_estoque.EntityConfig;
using api_estoque.Interface;
using api_estoque.Models;
using api_estoque.Padroes.Singleton;
using System.Diagnostics.Eventing.Reader;

namespace api_estoque.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        private readonly IEstoqueRepository _estoqueRepository;
        public UserRepository(AppDbContext context, IEstoqueRepository estoqueRepository)
        {
            _context = context;
            _estoqueRepository = estoqueRepository;
        }
        public List<User> GetAll()
        {
            return _context.Users.ToList();
        }

        public User GetById(int id)
        {
            return _context.Users.FirstOrDefault(u => u.Id == id);
        }

        public User LoginUser(UserLoginDTO user)
        {
            try
            {
                User usuarioLogado = GetByEmail(user.Email);

                if (usuarioLogado == default)
                {

                    User userNew = Salvar(new User { Email = user.Email, Name = user.Nome });
                    UserSingleton.Instance.DefinirUsuario(userNew);

                    Estoque estoque = _estoqueRepository.Create(userNew.Id);
                    EstoqueSingleton.Instance.DefinirEstoque(estoque);
                    return userNew;
                }
                else
                {
                    Estoque estoque = _estoqueRepository.GetById(usuarioLogado.Id);
                    EstoqueSingleton.Instance.DefinirEstoque(estoque);
                    UserSingleton.Instance.DefinirUsuario(usuarioLogado);
                }


                return usuarioLogado;
            }
            catch (Exception ex)
            {
                throw;
            }
            
        }

        private User GetByEmail(string email) {

            return _context.Users.FirstOrDefault(u => u.Email.ToLower().Contains(email.ToLower()));
        }

        public User Salvar(User user)
        {
            User userNew = new User
            {
                Email = user.Email,
                Name = user.Name,
                Telefone = user.Telefone
            };

            _context.Users.Add(userNew);
            _context.SaveChanges();

            return userNew;
        }

        public void Editar(User user)
        {
            User usuarioBanco = GetById(user.Id);

            if(usuarioBanco == default)
            {
                usuarioBanco = user;
                _context.SaveChanges();
            }
        }
    }
}
