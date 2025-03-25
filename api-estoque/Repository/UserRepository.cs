using api_estoque.DTO;
using api_estoque.EntityConfig;
using api_estoque.Interface;
using api_estoque.Models;
using api_estoque.Padroes.Singleton;

namespace api_estoque.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
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
            User usarioLogado = GetByEmail(user.Email);

            if (usarioLogado == default)
            {
              
               User userNew = Salvar(new User { Email = user.Email, Name = user.Nome });
                UserSingleton.Instance.DefinirUsuario(userNew);
                return userNew;
            }

            UserSingleton.Instance.DefinirUsuario(usarioLogado);
            return usarioLogado;
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
