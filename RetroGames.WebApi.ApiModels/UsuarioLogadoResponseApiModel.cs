namespace RetroGames.WebApi.ApiModels;

/// <summary>Dados do usuário logado. Nunca leva o hash da senha.</summary>
public class UsuarioLogadoResponseApiModel
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Perfil { get; set; } = string.Empty;
}
