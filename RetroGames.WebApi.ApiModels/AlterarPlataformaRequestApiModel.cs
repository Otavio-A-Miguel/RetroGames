namespace RetroGames.WebApi.ApiModels;

/// <summary>O que o cliente pode enviar para alterar uma plataforma. O id vem da rota.</summary>
public class AlterarPlataformaRequestApiModel
{
    public string Nome { get; set; } = string.Empty;
    public string Fabricante { get; set; } = string.Empty;
    public int AnoLancamento { get; set; }
}
