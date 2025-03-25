using api_estoque.Models;

namespace api_estoque.Interface
{
    public interface ICategoriaRepository
    {
        List<Categoria> GetAll();
        Categoria GetById(int id);
        Categoria Salvar(string nome);
        void Editar(Categoria categoria);
    }
}
