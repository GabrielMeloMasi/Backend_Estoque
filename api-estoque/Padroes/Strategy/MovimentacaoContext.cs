using api_estoque.Models;

namespace api_estoque.Padroes.Strategy
{
    public class MovimentacaoContext
    {

        private IMovimentacaoStrategy _strategy;

        public void SetStrategy(IMovimentacaoStrategy strategy)
        {
            _strategy = strategy;
        }

        public void SalvarMovimentacao(int estoqueProdutoId, int quantidade)
        {
            _strategy.Salvar(estoqueProdutoId, quantidade);
        }
    }
}
