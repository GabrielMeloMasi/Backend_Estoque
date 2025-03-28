using api_estoque.Models;

namespace api_estoque.Padroes.Singleton
{
    public class EstoqueSingleton
    {
        private static EstoqueSingleton _instance;
        private static readonly object _lock = new();

        public Estoque Estoque { get; private set; }

        private EstoqueSingleton() { }

        public static EstoqueSingleton Instance
        {
            get
            {
                lock (_lock)
                {
                    return _instance ??= new EstoqueSingleton();
                }
            }
        }

        public void DefinirEstoque(Estoque estoque)
        {
            Estoque = estoque;
        }
    }
}
