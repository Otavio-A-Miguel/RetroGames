using RetroGames.Model.Consultas;
using RetroGames.Model.Model;

namespace RetroGames.Services;

public interface IJogoService
{
    Task<PaginaResultado<JogoDetalheServiceModel>> ListarAsync(JogoFiltro filtro, int? idGenero);
    Task<JogoDetalheServiceModel> BuscarPorIdAsync(int id);
    Task<JogoDetalheServiceModel> InserirAsync(Jogo jogo, List<int> idsGeneros);
    Task<JogoDetalheServiceModel> AlterarAsync(Jogo jogo, List<int> idsGeneros);
    Task DesativarAsync(int id);
}
