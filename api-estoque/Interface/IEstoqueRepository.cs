using api_estoque.DTO;
using api_estoque.Models;

namespace api_estoque.Interface
{
    public interface IEstoqueRepository
    {

        EstoqueDTO GetEstoque();
        Estoque GetById(int userId);
        Estoque Create(int userId);
    }
}
