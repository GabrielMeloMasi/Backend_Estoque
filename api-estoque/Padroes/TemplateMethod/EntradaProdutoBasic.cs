using api_estoque.EntityConfig;
using api_estoque.Models;
using Microsoft.EntityFrameworkCore;

namespace api_estoque.Padroes.TemplateMethod
{
    public class EntradaProdutoBasic: EntradaProdutoTemplate
    {
        public EntradaProdutoBasic(AppDbContext context, Produto produto)
       : base(context, produto) { }

        protected override void Validar()
        {
            //if (produto.QuantTotal <= 0)
                throw new Exception("Quantidade inválida.");
        }

        protected override void AdicionaAtualizaProduto() { 
            
        }

        protected override void AtualizarEstoque()
        {
            //produto.QuantTotal += quantidade;
        }

        protected override void RegistrarMovimentacao()
        {
        

          //  _context.Movimentacao.Add(mov);
        }
    }
}
