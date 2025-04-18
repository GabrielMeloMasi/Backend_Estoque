using api_estoque.EntityConfig;
using api_estoque.Models;
using api_estoque.Padroes.Singleton;

namespace api_estoque.Padroes.Strategy
{
    public class MovimentacaoSaidaStrategy : IMovimentacaoStrategy
    {
        private readonly AppDbContext _context;

        public MovimentacaoSaidaStrategy(AppDbContext context)
        {
            _context = context;
        }

        public void Salvar(int estoqueProdutoId, int quantidade)
        {
            try
            {
                Movimentacao movimentacao = new Movimentacao
                {
                    UserId = UserSingleton.Instance.Usuario.Id,
                    Tipo = "S",
                    EstoqueProdutoId = estoqueProdutoId,
                    Quantidade = quantidade
                };

                _context.Movimentacao.Add(movimentacao);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao salvar movimentação de saída.", ex);
            }
        }
    }


}

