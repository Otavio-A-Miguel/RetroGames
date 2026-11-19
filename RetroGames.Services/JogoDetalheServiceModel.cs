using RetroGames.Model.Model;

namespace RetroGames.Services;

/// <summary>Jogo com os dados que vêm de outras tabelas, combinados com LINQ.</summary>
public class JogoDetalheServiceModel
{
    public Jogo Jogo { get; set; } = new();

    /// <summary>Vem de tb_plataforma.</summary>
    public string NomePlataforma { get; set; } = string.Empty;

    /// <summary>Vem de tb_condicao_jogo.</summary>
    public string DescricaoCondicaoJogo { get; set; } = string.Empty;

    /// <summary>Vêm de tb_jogo_genero e tb_genero.</summary>
    public List<Genero> Generos { get; set; } = [];
}
