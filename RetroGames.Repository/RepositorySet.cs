namespace RetroGames.Repository;

// Recebe os cinco repositórios pela injeção de dependência.
public class RepositorySet(
    IPlataformaRepository plataforma,
    IJogoRepository jogo,
    IJogoGeneroRepository jogoGenero,
    IGeneroRepository genero,
    ICondicaoJogoRepository condicaoJogo) : IRepositorySet
{
    public IPlataformaRepository Plataforma { get; } = plataforma;
    public IJogoRepository Jogo { get; } = jogo;
    public IJogoGeneroRepository JogoGenero { get; } = jogoGenero;
    public IGeneroRepository Genero { get; } = genero;
    public ICondicaoJogoRepository CondicaoJogo { get; } = condicaoJogo;
}
