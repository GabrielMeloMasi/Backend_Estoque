using api_estoque.Models;

namespace api_estoque.Interface
{
    public interface IValidadeRepository
    {
        void Save(Validade validade);
        void Editar(Validade validade);
        List<Validade> GetValidadeList(int idProduto);
    }
}
