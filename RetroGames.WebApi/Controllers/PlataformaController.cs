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
}
