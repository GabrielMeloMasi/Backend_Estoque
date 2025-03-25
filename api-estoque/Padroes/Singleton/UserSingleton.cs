using api_estoque.Models;

namespace api_estoque.Padroes.Singleton
{
    public class UserSingleton
    {
        private static UserSingleton _instance;
        private static readonly object _lock = new();

        public User Usuario { get; private set; }

        private UserSingleton() { }

        public static UserSingleton Instance
        {
            get
            {
                lock (_lock)
                {
                    return _instance ??= new UserSingleton();
                }
            }
        }

        public void DefinirUsuario(User usuario)
        {
            Usuario = usuario;
        }
    }
}
