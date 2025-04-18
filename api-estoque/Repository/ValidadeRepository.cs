using api_estoque.EntityConfig;
using api_estoque.Interface;
using api_estoque.Models;
using Microsoft.EntityFrameworkCore;

namespace api_estoque.Repository
{
    public class ValidadeRepository : IValidadeRepository
    {
        private readonly AppDbContext _context;
        public ValidadeRepository(AppDbContext context)
        {
            _context = context;
        }
        public List<Validade> Editar(int estoqueprodId, List<Validade> validade)
        {
            try
            {
                List<Validade> validadesBanco = GetValidadeList(estoqueprodId);
                // 1. Atualizar validades existentes
                foreach (var validadeBanco in validadesBanco)
                {
                    var validadeAtualizada = validade
                        .FirstOrDefault(v => v.Id == validadeBanco.Id);

                    if (validadeAtualizada != null)
                    {
                        validadeBanco.DataValidade = validadeAtualizada.DataValidade;
                        validadeBanco.Quantidade = validadeAtualizada.Quantidade;
                        _context.Validade.Update(validadeBanco);
                    }
                }

            
                var novasValidades = validade
                    .Where(v => v.Id == 0 || v.Id == null) 
                    .ToList();

                foreach (var nova in novasValidades)
                {
                    nova.EstoqueProdutoId = estoqueprodId;
                    _context.Validade.Add(nova);
                }

                
                var idsAtualizados = validade.Select(v => v.Id).ToList();

                var validadesParaRemover = validadesBanco
                    .Where(v => !idsAtualizados.Contains(v.Id))
                    .ToList();

                if (validadesParaRemover.Any())
                    _context.Validade.RemoveRange(validadesParaRemover);

                // 4. Salvar mudanças
                _context.SaveChanges();

                
                return _context.Validade.Where(x => x.EstoqueProdutoId == estoqueprodId).ToList();
            }
            catch (Exception ex)
            {
            }
        }

        public List<Validade> GetValidadeList(int estoqueProdId)
        {
            return _context.Validade.Where(v => v.EstoqueProdutoId == estoqueProdId).ToList();
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
