using System.Net;

namespace RetroGames.Services;

/// <summary>
/// Lançada quando uma regra de negócio é violada. Guarda o erro do catálogo BusinessErrors.
/// </summary>
public class BusinessRuleException((int Code, string DefaultMessage, HttpStatusCode Status) erro)
    : Exception(erro.DefaultMessage)
{
    /// <summary>Erro do catálogo: código, mensagem e status HTTP.</summary>
    public (int Code, string DefaultMessage, HttpStatusCode Status) Erro { get; } = erro;
}
