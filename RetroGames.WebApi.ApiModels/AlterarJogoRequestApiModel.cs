namespace RetroGames.WebApi.ApiModels;

/// <summary>
/// O jogo completo, como o PUT exige. O id vem da rota.
/// Ativo permite reativar um jogo desativado.
/// </summary>
public class AlterarJogoRequestApiModel
{
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public int AnoLancamento { get; set; }
    public decimal Preco { get; set; }
    public int Estoque { get; set; }
    public bool Ativo { get; set; }
    public int IdPlataforma { get; set; }
    public int IdCondicaoJogo { get; set; }
    public List<int> IdsGeneros { get; set; } = [];
}
