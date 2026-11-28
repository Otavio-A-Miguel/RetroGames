using Microsoft.AspNetCore.Authorization;
using RetroGames.Model.Constantes;

namespace RetroGames.WebApi.Configuration;

public static class AutorizacaoConfig
{
    public static IServiceCollection AddAutorizacao(this IServiceCollection services)
    {
        services.AddAuthorizationBuilder()
            // Segura por padrão: toda rota exige login, salvo [AllowAnonymous].
            .SetFallbackPolicy(new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build())
            // Gravação e gestão: login com o perfil Administrador.
            .AddPolicy(Politicas.Administrador, politica => politica
                .RequireAuthenticatedUser()
                .RequireRole(Perfis.Administrador));

        return services;
    }
}
