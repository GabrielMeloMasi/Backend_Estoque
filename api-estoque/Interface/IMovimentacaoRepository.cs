using api_estoque.DTO;
using api_estoque.Models;
using System.Text.Json.Nodes;

namespace api_estoque.Interface
{
    public interface IMovimentacaoRepository
    {

        List<Movimentacao> GetAll();
        List<Movimentacao> GetProduto(int idProduto);
        List<Movimentacao> GetTipo(string tipoMovimentacao);
        List<FilterDTO> Filter(JsonObject filterDTO);

    }
}
