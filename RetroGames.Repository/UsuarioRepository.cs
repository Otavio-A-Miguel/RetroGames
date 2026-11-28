using System.Data.Common;
using RetroGames.Model.Constantes;
using RetroGames.Model.Model;

namespace RetroGames.Repository;

public class UsuarioRepository(RepositoryConfig config) : RepositoryBase(config), IUsuarioRepository
{
    private const string SelectUsuario =
        "SELECT co_seq_usuario, no_usuario, ds_email, ds_senha_hash, st_ativo, dt_cadastro, co_perfil " +
        "FROM tb_usuario";

    public async Task<Usuario?> BuscarPorEmailAsync(string email)
    {
        await using var conexao = await CriarConexaoAsync();
        await using var comando = CriarComando(conexao, SelectUsuario + " WHERE ds_email = @email");
        AdicionarParametro(comando, "@email", email);
        await using var leitor = await comando.ExecuteReaderAsync();

        return await leitor.ReadAsync() ? Mapear(leitor) : null;
    }

    public async Task<Usuario?> BuscarPorIdAsync(int id)
    {
        await using var conexao = await CriarConexaoAsync();
        await using var comando = CriarComando(conexao, SelectUsuario + " WHERE co_seq_usuario = @id");
        AdicionarParametro(comando, "@id", id);
        await using var leitor = await comando.ExecuteReaderAsync();

        return await leitor.ReadAsync() ? Mapear(leitor) : null;
    }

    public async Task<bool> ExisteAdministradorAsync()
    {
        await using var conexao = await CriarConexaoAsync();
        await using var comando = CriarComando(conexao,
            "SELECT COUNT(*) FROM tb_usuario WHERE co_perfil = @idPerfil");
        AdicionarParametro(comando, "@idPerfil", PerfilIds.Administrador);

        return Convert.ToInt64(await comando.ExecuteScalarAsync()) > 0;
    }

    public async Task<int> InserirAsync(Usuario usuario)
    {
        await using var conexao = await CriarConexaoAsync();
        await using var comando = CriarComando(conexao,
            "INSERT INTO tb_usuario (no_usuario, ds_email, ds_senha_hash, st_ativo, dt_cadastro, co_perfil) " +
            "VALUES (@nome, @email, @senhaHash, @ativo, @cadastro, @idPerfil)");
        AdicionarParametro(comando, "@nome", usuario.Nome);
        AdicionarParametro(comando, "@email", usuario.Email);
        AdicionarParametro(comando, "@senhaHash", usuario.SenhaHash);
        AdicionarParametro(comando, "@ativo", usuario.Ativo);
        AdicionarParametro(comando, "@cadastro", usuario.Cadastro);
        AdicionarParametro(comando, "@idPerfil", usuario.IdPerfil);

        // ds_email é UNIQUE: e-mail repetido vira RegistroDuplicadoException.
        return (int)await ExecutarGravacaoAsync(comando);
    }

    private static Usuario Mapear(DbDataReader leitor) => new()
    {
        Id = leitor.GetInt32(leitor.GetOrdinal("co_seq_usuario")),
        Nome = leitor.GetString(leitor.GetOrdinal("no_usuario")),
        Email = leitor.GetString(leitor.GetOrdinal("ds_email")),
        SenhaHash = leitor.GetString(leitor.GetOrdinal("ds_senha_hash")),
        Ativo = leitor.GetBoolean(leitor.GetOrdinal("st_ativo")),
        Cadastro = leitor.GetDateTime(leitor.GetOrdinal("dt_cadastro")),
        IdPerfil = leitor.GetInt32(leitor.GetOrdinal("co_perfil")),
    };
}
