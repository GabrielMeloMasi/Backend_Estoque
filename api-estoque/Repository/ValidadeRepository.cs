using api_estoque.EntityConfig;
using api_estoque.Interface;
using api_estoque.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

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
                throw ex;
            }
        }

        public List<Validade> GetValidadeList(int estoqueProdId)
        {
            return _context.Validade.Where(v => v.EstoqueProdutoId == estoqueProdId).ToList();
        }


        public Validade Save(int estoqueProId, DateTime data, int quantidade)
        {
            try
            {
                Validade val = new Validade { 
                    EstoqueProdutoId = estoqueProId,
                    Quantidade = quantidade,
                    DataValidade = data
                };

                _context.Validade.Add(val);
                _context.SaveChanges();

                return val;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private Validade GetById(int id)
        {
            return _context.Validade.FirstOrDefault(x => x.Id == id);
        }

        public bool Saida(int estoqueProdId, int quantidade)
        {

            try
            {
                var validades = GetValidadeList(estoqueProdId)
                           .OrderBy(v => v.DataValidade)
                           .ToList();

                if (!validades.Any())
                    return false;

                int quantidadeRestante = quantidade;

                foreach (var validade in validades)
                {
                    if (quantidadeRestante <= 0)
                        break;

                    if (validade.Quantidade <= quantidadeRestante)
                    {
                        quantidadeRestante -= validade.Quantidade;
                        _context.Validade.Remove(validade);
                    }
                    else
                    {
                        validade.Quantidade -= quantidadeRestante;
                        quantidadeRestante = 0;
                        _context.Validade.Update(validade);
                    }
                }

                if (quantidadeRestante > 0)
                    return false;

                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
           
        
    }
    
}
