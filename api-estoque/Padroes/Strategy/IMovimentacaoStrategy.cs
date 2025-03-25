using api_estoque.Models;
using System.Runtime.InteropServices;

namespace api_estoque.Padroes.Strategy
{
    public interface IMovimentacaoStrategy
    {

        double CalcularValorTotal(List<Movimentacao> movimentacoes);
    }
}
