using api_estoque.Models;

namespace api_estoque.Interface
{
    public interface IValidadeRepository
    {
        Validade Save(int estoqueProId, DateTime data, int quantidade);
        List<Validade> Editar(int estoqueprodId, List<Validade> validade);
        List<Validade> GetValidadeList(int estoqueProdId);
        bool Saida(int estoqueProdId, int quantidade);
    }
}
