using Microsoft.AspNetCore.Diagnostics;
using RetroGames.Services;
using RetroGames.WebApi.ApiModels;

namespace RetroGames.WebApi.Middlewares;

/// <summary>
/// Único ponto que transforma exceções não tratadas em resposta HTTP com o envelope.
/// </summary>
public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        RetroGamesResponseApiModel resposta;

        if (exception is BusinessRuleException erroNegocio)
        {
            // Regra de negócio violada: status e código vêm do catálogo BusinessErrors.
            httpContext.Response.StatusCode = (int)erroNegocio.Erro.Status;
            resposta = new RetroGamesResponseApiModel
            {
                ResponseCode = erroNegocio.Erro.Code,
                ErrorMessage = erroNegocio.Message,
            };
        }
        else
        {
            // Erro inesperado: o detalhe vai só para o log, nunca para o cliente.
            logger.LogError(exception, "Erro inesperado em {Metodo} {Caminho}",
                httpContext.Request.Method, httpContext.Request.Path);

            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            resposta = new RetroGamesResponseApiModel
            {
                ResponseCode = ResponseCodes.ERRO,
                ErrorMessage = "Ocorreu um erro inesperado. Tente novamente mais tarde.",
            };
        }

        await httpContext.Response.WriteAsJsonAsync(resposta, cancellationToken);
        return true;   // a exceção foi tratada
    }
}
