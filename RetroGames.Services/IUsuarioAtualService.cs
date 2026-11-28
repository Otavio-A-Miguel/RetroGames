namespace RetroGames.Services;

/// <summary>
/// Quem está chamando. O Service não conhece HTTP: a WebApi implementa
/// esta interface lendo o usuário da requisição.
/// </summary>
public interface IUsuarioAtualService
{
    bool EstaAutenticado { get; }
    int? Id { get; }
    bool EhAdministrador { get; }
}
