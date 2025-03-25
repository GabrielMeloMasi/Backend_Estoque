using api_estoque.EntityConfig;
using api_estoque.Models;

namespace api_estoque.Padroes.TemplateMethod
{
    public abstract class EntradaProdutoTemplate
    {
        protected readonly AppDbContext _context;
        protected readonly Produto produto;

        protected EntradaProdutoTemplate(AppDbContext context, Produto produto)
        {
            _context = context;
            this.produto = produto;
        }

        public void Executar()
        {
            Validar();
            AdicionaAtualizaProduto();
            AtualizarEstoque();
            SalvarValidades(); // opcional
            RegistrarMovimentacao();
            Salvar();
        }

        protected abstract void Validar();
        protected abstract void AdicionaAtualizaProduto();
        protected abstract void AtualizarEstoque();
        protected virtual void SalvarValidades() { } // opcional
        protected abstract void RegistrarMovimentacao();
        protected virtual void Salvar()
        {
            _context.SaveChanges();
        }
    }
}
