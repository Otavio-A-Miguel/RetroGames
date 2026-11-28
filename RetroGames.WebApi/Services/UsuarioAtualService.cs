using System.Security.Claims;
using RetroGames.Model.Constantes;
using RetroGames.Services;
using RetroGames.WebApi.Extensions;

namespace RetroGames.WebApi.Services;

public class UsuarioAtualService(IHttpContextAccessor httpContextAccessor) : IUsuarioAtualService
{
    private ClaimsPrincipal? Usuario => httpContextAccessor.HttpContext?.User;

    public bool EstaAutenticado => Usuario?.Identity?.IsAuthenticated == true;

    public int? Id => EstaAutenticado ? Usuario!.ObterIdUsuario() : null;

    public bool EhAdministrador => EstaAutenticado && Usuario!.IsInRole(Perfis.Administrador);
}
