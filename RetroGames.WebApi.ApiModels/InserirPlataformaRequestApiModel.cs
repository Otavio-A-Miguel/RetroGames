namespace RetroGames.WebApi.ApiModels;

/// <summary>O que o cliente pode enviar para incluir uma plataforma (sem Id).</summary>
public class InserirPlataformaRequestApiModel
{
    public string Nome { get; set; } = string.Empty;
    public string Fabricante { get; set; } = string.Empty;
    public int AnoLancamento { get; set; }
}
