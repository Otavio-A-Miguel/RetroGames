namespace RetroGames.WebApi.ApiModels;

/// <summary>
/// Códigos da própria API (0 a 99). Erros de negócio não se repetem aqui:
/// o envelope devolve o código do BusinessErrors (100 em diante).
/// </summary>
public static class ResponseCodes
{
    public const int SUCESSO = 0;
    public const int ERRO = 1;

    public static readonly (int Code, string DefaultMessage) MODEL_NULL =
        (2, "O modelo veio nulo.");

    public static readonly (int Code, string DefaultMessage) MODEL_STATE_INVALIDO =
        (3, "Há um erro com o modelo enviado.");

    public static readonly (int Code, string DefaultMessage) NAO_AUTENTICADO =
        (4, "Faça login para continuar.");

    public static readonly (int Code, string DefaultMessage) ACESSO_NEGADO =
        (5, "Você não tem permissão para esta operação.");

    public static readonly (int Code, string DefaultMessage) CREDENCIAIS_INVALIDAS =
        (6, "E-mail ou senha inválidos.");
}
