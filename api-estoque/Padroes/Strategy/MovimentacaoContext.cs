using api_estoque.Models;

namespace api_estoque.Padroes.Strategy
{
    public class MovimentacaoContext
    {
        private readonly IMovimentacaoStrategy _strategy;

        public MovimentacaoContext(IMovimentacaoStrategy strategy)
        {
            _strategy = strategy;
        }

        public double Calcular(List<Movimentacao> movimentacoes)
        {
            return _strategy.CalcularValorTotal(movimentacoes);
        }
    }
}
