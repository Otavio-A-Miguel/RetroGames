using System.Text.Json;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using RetroGames.WebApi.ApiModels;

namespace RetroGames.WebApi.Filters;

/// <summary>
/// Roda depois do model binding e antes da action. Valida cada argumento
/// que tenha um validator registrado; com erro, responde 400 e a action não roda.
/// </summary>
public class FluentValidationFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext contexto, ActionExecutionDelegate proximo)
    {
        var erros = new Dictionary<string, List<string>>();

        foreach (var argumento in contexto.ActionArguments.Values)
        {
            if (argumento is null)
            {
                continue;
            }

            // Procura um IValidator<TipoDoArgumento>. Sem validator, o argumento passa.
            var tipoValidator = typeof(IValidator<>).MakeGenericType(argumento.GetType());
            if (contexto.HttpContext.RequestServices.GetService(tipoValidator) is not IValidator validator)
            {
                continue;
            }

            var resultado = await validator.ValidateAsync(new ValidationContext<object>(argumento));
            foreach (var falha in resultado.Errors)
            {
                // "AnoLancamento" vira "anoLancamento", como no JSON.
                var campo = JsonNamingPolicy.CamelCase.ConvertName(falha.PropertyName);
                if (!erros.TryGetValue(campo, out var mensagens))
                {
                    erros[campo] = mensagens = [];
                }
                mensagens.Add(falha.ErrorMessage);
            }
        }

        if (erros.Count > 0)
        {
            contexto.Result = RespostaInvalida(erros.ToDictionary(e => e.Key, e => e.Value.ToArray()));
            return;
        }

        await proximo();
    }

    /// <summary>400 com responseCode 3 e os erros por campo em data.</summary>
    public static BadRequestObjectResult RespostaInvalida(Dictionary<string, string[]> erros) =>
        new(new RetroGamesResponseApiModel<Dictionary<string, string[]>>(erros)
        {
            ResponseCode = ResponseCodes.MODEL_STATE_INVALIDO.Code,
            ErrorMessage = ResponseCodes.MODEL_STATE_INVALIDO.DefaultMessage,
        });
}
