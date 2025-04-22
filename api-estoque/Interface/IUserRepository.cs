using api_estoque.DTO;
using Microsoft.AspNetCore.Identity.Data;
using api_estoque.Models;

namespace api_estoque.Interface
{
    public interface IUserRepository
    {
        User LoginUser(User user);
        User GetById (int id);
        List<User> GetAll();
        User Salvar (User user);
        void Editar(User user);

    }
}
