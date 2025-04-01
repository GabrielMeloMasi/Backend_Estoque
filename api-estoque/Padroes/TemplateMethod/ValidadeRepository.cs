using api_estoque.EntityConfig;
using api_estoque.Interface;
using api_estoque.Models;
using Microsoft.EntityFrameworkCore;

namespace api_estoque.Padroes.TemplateMethod
{
    public class ValidadeRepository : IValidadeRepository
    {
        private readonly AppDbContext _context;
        public ValidadeRepository(AppDbContext context)
        {
            _context = context;
        }
            public void Editar(Validade validade)
        {
            try
            {
                var validadeBanco = GetById(validade.Id);
                validadeBanco = validade;
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
            }
        }

        public List<Validade> GetValidadeList(int idProduto)
        {
            return _context.EstoqueProdutos
                .Include(x => x.Validades)
                .Where(x => x.ProdutoId == idProduto) 
                .SelectMany(x => x.Validades)
                .ToList();
        }


        public void Save(Validade validade)
        {
            try
            {
                _context.Add(validade);
                _context.SaveChanges();
            }
            catch (Exception ex) 
            {
            }

        }

        private Validade GetById(int id) 
        {
            return _context.Validade.FirstOrDefault(x => x.Id == id);
        }

    }
}
