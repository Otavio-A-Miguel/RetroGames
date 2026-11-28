using Microsoft.AspNetCore.Authorization;
using RetroGames.WebApi.Configuration;
using Microsoft.AspNetCore.Mvc;
using RetroGames.Services;
using RetroGames.WebApi.ApiModels;
using RetroGames.WebApi.Mappers;

namespace RetroGames.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class JogoController(IJogoService jogoService) : ControllerBase
{
    // GET /api/Jogo?busca=...&idPlataforma=...&idGenero=...&pagina=...&tamanhoPagina=...
    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<RetroGamesResponseApiModel<PaginaResponseApiModel<ListarJogoResponseApiModel>>>> Listar(
        [FromQuery] ListarJogoRequestApiModel request)
    {
        var resultado = await jogoService.ListarAsync(request.ParaFiltro(), request.IdGenero);

        return Ok(new RetroGamesResponseApiModel<PaginaResponseApiModel<ListarJogoResponseApiModel>>(
            resultado.ParaPaginaResponse(request.Pagina, request.TamanhoPagina)));
    }

    // GET /api/Jogo/{id}
    [AllowAnonymous]
    [HttpGet("{id:int}")]
    public async Task<ActionResult<RetroGamesResponseApiModel<ListarPorIdJogoResponseApiModel>>> ListarPorId(int id)
    {
        var detalhe = await jogoService.BuscarPorIdAsync(id);

        return Ok(new RetroGamesResponseApiModel<ListarPorIdJogoResponseApiModel>(detalhe.ParaListarPorIdResponse()));
    }

    // POST /api/Jogo  (jogo e gêneros gravados numa transação, no Service)
    [Authorize(Policy = Politicas.Administrador)]
    [HttpPost]
    public async Task<ActionResult<RetroGamesResponseApiModel<ListarPorIdJogoResponseApiModel>>> Inserir(
        InserirJogoRequestApiModel request)
    {
        var criado = await jogoService.InserirAsync(request.ParaModel(), request.IdsGeneros);

        return CreatedAtAction(nameof(ListarPorId), new { id = criado.Jogo.Id },
            new RetroGamesResponseApiModel<ListarPorIdJogoResponseApiModel>(criado.ParaListarPorIdResponse()));
    }

    // PUT /api/Jogo/{id}  (jogo completo no corpo; "ativo": true reativa)
    [Authorize(Policy = Politicas.Administrador)]
    [HttpPut("{id:int}")]
    public async Task<ActionResult<RetroGamesResponseApiModel<ListarPorIdJogoResponseApiModel>>> Alterar(
        int id, AlterarJogoRequestApiModel request)
    {
        var alterado = await jogoService.AlterarAsync(request.ParaModel(id), request.IdsGeneros);

        return Ok(new RetroGamesResponseApiModel<ListarPorIdJogoResponseApiModel>(alterado.ParaListarPorIdResponse()));
    }

    // DELETE /api/Jogo/{id}  (exclusão lógica, RN03: o jogo só é desativado)
    [Authorize(Policy = Politicas.Administrador)]
    [HttpDelete("{id:int}")]
    public async Task<ActionResult<RetroGamesResponseApiModel>> Excluir(int id)
    {
        await jogoService.DesativarAsync(id);

        return Ok(new RetroGamesResponseApiModel());
    }
}
