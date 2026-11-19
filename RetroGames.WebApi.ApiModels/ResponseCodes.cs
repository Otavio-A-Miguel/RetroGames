namespace RetroGames.WebApi.ApiModels;

/// <summary>
/// Códigos da própria API (0 a 99). Erros de negócio não se repetem aqui:
/// o envelope devolve o código do BusinessErrors (100 em diante).
/// </summary>
public static class ResponseCodes
{
    public const int SUCESSO = 0;
    public const int ERRO = 1;
}
