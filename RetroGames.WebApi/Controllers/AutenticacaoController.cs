using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RetroGames.Services;
using RetroGames.WebApi.ApiModels;
using RetroGames.WebApi.Extensions;
using RetroGames.WebApi.Mappers;

namespace RetroGames.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AutenticacaoController(IAutenticacaoService autenticacaoService) : ControllerBase
{
    // POST /api/Autenticacao/login
    [HttpPost("login")]
    public async Task<ActionResult<RetroGamesResponseApiModel<UsuarioLogadoResponseApiModel>>> Login(
        LoginRequestApiModel request)
    {
        var usuario = await autenticacaoService.AutenticarAsync(request.Email, request.Senha);
        if (usuario is null)
        {
            return Unauthorized(new RetroGamesResponseApiModel
            {
                ResponseCode = ResponseCodes.CREDENCIAIS_INVALIDAS.Code,
                ErrorMessage = ResponseCodes.CREDENCIAIS_INVALIDAS.DefaultMessage,
            });
        }

        // Cria a sessão: o cookie retrogames.sessao vai na resposta.
        var principal = usuario.ParaClaimsPrincipal();
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

        return Ok(new RetroGamesResponseApiModel<UsuarioLogadoResponseApiModel>(principal.ParaUsuarioLogadoResponse()));
    }

    // POST /api/Autenticacao/logout
    [Authorize]
    [HttpPost("logout")]
    public async Task<ActionResult<RetroGamesResponseApiModel>> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        return Ok(new RetroGamesResponseApiModel());
    }

    // GET /api/Autenticacao/eu
    [Authorize]
    [HttpGet("eu")]
    public ActionResult<RetroGamesResponseApiModel<UsuarioLogadoResponseApiModel>> Eu()
    {
        return Ok(new RetroGamesResponseApiModel<UsuarioLogadoResponseApiModel>(User.ParaUsuarioLogadoResponse()));
    }
}
