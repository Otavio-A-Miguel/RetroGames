namespace RetroGames.WebApi.ApiModels;

/// <summary>Uma página de resultados, com os totais.</summary>
public class PaginaResponseApiModel<T>
{
    public List<T> Itens { get; set; } = [];
    public int Pagina { get; set; }
    public int TamanhoPagina { get; set; }
    public int TotalItens { get; set; }
    public int TotalPaginas { get; set; }
}
