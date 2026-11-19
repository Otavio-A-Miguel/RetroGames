using RetroGames.Model.Model;
using RetroGames.Repository;

namespace RetroGames.Services;

public class GeneroService(IRepositorySet repositorio) : IGeneroService
{
    public Task<List<Genero>> ListarAsync()
        => repositorio.Genero.ListarAsync();
}
