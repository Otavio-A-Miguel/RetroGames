using Microsoft.AspNetCore.Mvc;
using RetroGames.Services;
using RetroGames.WebApi.ApiModels;
using RetroGames.WebApi.Mappers;

namespace RetroGames.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlataformaController(IPlataformaService plataformaService) : ControllerBase
{
    // GET /api/Plataforma
    [HttpGet]
    public async Task<ActionResult<RetroGamesResponseApiModel<List<ListarPlataformaResponseApiModel>>>> Listar()
    {
        var plataformas = await plataformaService.ListarAsync();

        return Ok(new RetroGamesResponseApiModel<List<ListarPlataformaResponseApiModel>>(
            plataformas.Select(p => p.ParaResponse()).ToList()));
    }

    // GET /api/Plataforma/{id}
    [HttpGet("{id:int}")]
    public async Task<ActionResult<RetroGamesResponseApiModel<ListarPlataformaResponseApiModel>>> ListarPorId(int id)
    {
        var plataforma = await plataformaService.BuscarPorIdAsync(id);

        return Ok(new RetroGamesResponseApiModel<ListarPlataformaResponseApiModel>(plataforma.ParaResponse()));
    }

    // POST /api/Plataforma  (dados no corpo)
    [HttpPost]
    public async Task<ActionResult<RetroGamesResponseApiModel<ListarPlataformaResponseApiModel>>> Inserir(
        InserirPlataformaRequestApiModel request)
    {
        var criada = await plataformaService.InserirAsync(request.ParaModel());

        // 201 Created, com o cabeçalho Location apontando para GET /api/Plataforma/{id}.
        return CreatedAtAction(nameof(ListarPorId), new { id = criada.Id },
            new RetroGamesResponseApiModel<ListarPlataformaResponseApiModel>(criada.ParaResponse()));
    }

    // PUT /api/Plataforma/{id}  (registro completo no corpo)
    [HttpPut("{id:int}")]
    public async Task<ActionResult<RetroGamesResponseApiModel<ListarPlataformaResponseApiModel>>> Alterar(
        int id, AlterarPlataformaRequestApiModel request)
    {
        var alterada = await plataformaService.AlterarAsync(request.ParaModel(id));

        return Ok(new RetroGamesResponseApiModel<ListarPlataformaResponseApiModel>(alterada.ParaResponse()));
    }

    // DELETE /api/Plataforma/{id}  (exclusão física; a API não usa 204)
    [HttpDelete("{id:int}")]
    public async Task<ActionResult<RetroGamesResponseApiModel>> Excluir(int id)
    {
        await plataformaService.ExcluirAsync(id);

        return Ok(new RetroGamesResponseApiModel());
    }
}
