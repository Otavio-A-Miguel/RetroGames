namespace RetroGames.Model.Consultas;

public class PaginaResultado<T>
{
    /// <summary>Itens da página pedida.</summary>
    public List<T> Itens { get; set; } = [];

    /// <summary>Total de itens que atendem ao filtro, em todas as páginas.</summary>
    public int Total { get; set; }
}
