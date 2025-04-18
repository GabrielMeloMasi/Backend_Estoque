using api_estoque.Models;

namespace api_estoque.Interface
{
    public interface IValidadeRepository
    {
        void Save(Validade validade);
        List<Validade> Editar(int estoqueprodId, List<Validade> validade);
        List<Validade> GetValidadeList(int estoqueProdId);
    }
}
