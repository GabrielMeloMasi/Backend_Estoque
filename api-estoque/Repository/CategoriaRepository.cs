using api_estoque.EntityConfig;
using api_estoque.Interface;
using api_estoque.Models;
using Microsoft.EntityFrameworkCore;

namespace api_estoque.Repository
{
    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly AppDbContext _context;

        public CategoriaRepository(AppDbContext context)
        {
            _context = context;
        }
        public List<Categoria> GetAll()
        {
            return _context.Categoria.ToList();
        }

        public Categoria GetById(int id)
        {
            return _context.Categoria.FirstOrDefault(c => c.Id == id);
        }

        public Categoria Salvar(string nome)
        {
            var categoria = new Categoria
            {
                Nome = nome
            };

            _context.Categoria.Add(categoria);
            _context.SaveChanges();

            return categoria;
        }

        public void Editar(Categoria categoria)
        {
            Categoria categoriaBanco = GetById(categoria.Id);

            if (categoriaBanco != null)
            {
                categoriaBanco = categoria;

                _context.Entry(categoriaBanco).State = EntityState.Modified;
                _context.SaveChanges();
            }
        }
    }
}
