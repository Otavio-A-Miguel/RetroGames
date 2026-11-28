namespace RetroGames.Repository;

// Recebe os repositórios pela injeção de dependência.
public class RepositorySet(
    IPlataformaRepository plataforma,
    IJogoRepository jogo,
    IJogoGeneroRepository jogoGenero,
    IGeneroRepository genero,
    ICondicaoJogoRepository condicaoJogo,
    IUsuarioRepository usuario) : IRepositorySet
{
    public IPlataformaRepository Plataforma { get; } = plataforma;
    public IJogoRepository Jogo { get; } = jogo;
    public IJogoGeneroRepository JogoGenero { get; } = jogoGenero;
    public IGeneroRepository Genero { get; } = genero;
    public ICondicaoJogoRepository CondicaoJogo { get; } = condicaoJogo;
    public IUsuarioRepository Usuario { get; } = usuario;
}
