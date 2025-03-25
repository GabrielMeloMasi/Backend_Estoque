using api_estoque.Models;

namespace api_estoque.Padroes.Strategy
{
    public class MovimentacaoEntradaStrategy : IMovimentacaoStrategy
    {
        public double CalcularValorTotal(List<Movimentacao> movimentacoes)
        {
            return movimentacoes.Where(m => m.Tipo == "entrada").Sum(m => m.Produto.Preco * m.Quantidade);
        }
    }
}
