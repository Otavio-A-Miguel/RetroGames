using System.Security.Claims;
using RetroGames.WebApi.ApiModels;
using RetroGames.WebApi.Extensions;

namespace RetroGames.WebApi.Mappers;

public static class UsuarioMapper
{
    // Lê os dados do usuário das claims da sessão (sem consultar o banco).
    public static UsuarioLogadoResponseApiModel ParaUsuarioLogadoResponse(this ClaimsPrincipal principal) => new()
    {
        Id = principal.ObterIdUsuario() ?? 0,
        Nome = principal.FindFirstValue(ClaimTypes.Name) ?? string.Empty,
        Email = principal.FindFirstValue(ClaimTypes.Email) ?? string.Empty,
        Perfil = principal.FindFirstValue(ClaimTypes.Role) ?? string.Empty,
    };
}
