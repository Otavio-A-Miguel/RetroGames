namespace RetroGames.Model.Consultas;

public class JogoFiltro
{
    /// <summary>Parte do nome do jogo. Vazio ou nulo: sem filtro.</summary>
    public string? Busca { get; set; }

    /// <summary>Plataforma do jogo. Nulo: sem filtro.</summary>
    public int? IdPlataforma { get; set; }

    /// <summary>
    /// Ids de jogos permitidos. Nulo: sem filtro. Lista vazia: nenhum jogo atende.
    /// </summary>
    public List<int>? Ids { get; set; }

    /// <summary>Se verdadeiro, inclui os jogos inativos.</summary>
    public bool IncluirInativos { get; set; }

    /// <summary>Número da página, começando em 1.</summary>
    public int Pagina { get; set; } = 1;

    public int TamanhoPagina { get; set; } = 10;
}
