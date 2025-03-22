namespace api_estoque.Interface
{
    public interface IObserver
    {
        //quero adicionar um observer para caso o produto vença  ele retire do  estoque e salve nas movimentações
        void AtualizaEstoqueValidade(string mensagem);
    }
}
