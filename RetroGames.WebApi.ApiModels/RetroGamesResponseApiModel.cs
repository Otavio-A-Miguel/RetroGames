namespace RetroGames.WebApi.ApiModels;

/// <summary>Envelope sem dados: usado em erros e em respostas sem conteúdo.</summary>
public class RetroGamesResponseApiModel
{
    public int ResponseCode { get; set; } = ResponseCodes.SUCESSO;
    public string? ErrorMessage { get; set; }
}

/// <summary>Envelope com dados.</summary>
public class RetroGamesResponseApiModel<T>(T data) : RetroGamesResponseApiModel
{
    public T Data { get; set; } = data;
}
