using System.Data.Common;
using RetroGames.Model.Model;

namespace RetroGames.Repository;

public class PlataformaRepository(RepositoryConfig config)
    : RepositoryBase(config), IPlataformaRepository
{
    private const string SelectPlataforma =
        "SELECT co_seq_plataforma, no_plataforma, no_fabricante, nu_ano_lancamento " +
        "FROM tb_plataforma";

    public async Task<List<Plataforma>> ListarAsync()
    {
        await using var conexao = await CriarConexaoAsync();
        await using var comando = CriarComando(conexao, SelectPlataforma + " ORDER BY no_plataforma");
        await using var leitor = await comando.ExecuteReaderAsync();

        var plataformas = new List<Plataforma>();
        while (await leitor.ReadAsync())
        {
            plataformas.Add(Mapear(leitor));
        }
        return plataformas;
    }

    public async Task<Plataforma?> BuscarPorIdAsync(int id)
    {
        await using var conexao = await CriarConexaoAsync();
        await using var comando = CriarComando(conexao, SelectPlataforma + " WHERE co_seq_plataforma = @id");
        AdicionarParametro(comando, "@id", id);
        await using var leitor = await comando.ExecuteReaderAsync();

        return await leitor.ReadAsync() ? Mapear(leitor) : null;
    }

    public async Task<bool> ExisteComNomeAsync(string nome, int? idIgnorar = null)
    {
        await using var conexao = await CriarConexaoAsync();
        await using var comando = CriarComando(conexao,
            "SELECT COUNT(*) FROM tb_plataforma " +
            "WHERE no_plataforma = @nome AND (@idIgnorar IS NULL OR co_seq_plataforma <> @idIgnorar)");
        AdicionarParametro(comando, "@nome", nome);
        AdicionarParametro(comando, "@idIgnorar", idIgnorar);

        return Convert.ToInt64(await comando.ExecuteScalarAsync()) > 0;
    }

    public async Task<bool> PossuiJogosAsync(int id)
    {
        await using var conexao = await CriarConexaoAsync();
        await using var comando = CriarComando(conexao,
            "SELECT COUNT(*) FROM tb_jogo WHERE co_plataforma = @id");
        AdicionarParametro(comando, "@id", id);

        return Convert.ToInt64(await comando.ExecuteScalarAsync()) > 0;
    }

    public async Task<int> InserirAsync(Plataforma plataforma)
    {
        await using var conexao = await CriarConexaoAsync();
        await using var comando = CriarComando(conexao,
            "INSERT INTO tb_plataforma (no_plataforma, no_fabricante, nu_ano_lancamento) " +
            "VALUES (@nome, @fabricante, @anoLancamento)");
        AdicionarParametro(comando, "@nome", plataforma.Nome);
        AdicionarParametro(comando, "@fabricante", plataforma.Fabricante);
        AdicionarParametro(comando, "@anoLancamento", plataforma.AnoLancamento);

        return (int)await ExecutarGravacaoAsync(comando);
    }

    public async Task AlterarAsync(Plataforma plataforma)
    {
        await using var conexao = await CriarConexaoAsync();
        await using var comando = CriarComando(conexao,
            "UPDATE tb_plataforma " +
            "SET no_plataforma = @nome, no_fabricante = @fabricante, nu_ano_lancamento = @anoLancamento " +
            "WHERE co_seq_plataforma = @id");
        AdicionarParametro(comando, "@nome", plataforma.Nome);
        AdicionarParametro(comando, "@fabricante", plataforma.Fabricante);
        AdicionarParametro(comando, "@anoLancamento", plataforma.AnoLancamento);
        AdicionarParametro(comando, "@id", plataforma.Id);

        await ExecutarGravacaoAsync(comando);
    }

    public async Task ExcluirAsync(int id)
    {
        await using var conexao = await CriarConexaoAsync();
        await using var comando = CriarComando(conexao,
            "DELETE FROM tb_plataforma WHERE co_seq_plataforma = @id");
        AdicionarParametro(comando, "@id", id);

        await comando.ExecuteNonQueryAsync();
    }

    // Converte a linha atual do leitor em uma Plataforma, coluna por coluna.
    private static Plataforma Mapear(DbDataReader leitor) => new()
    {
        Id = leitor.GetInt32(leitor.GetOrdinal("co_seq_plataforma")),
        Nome = leitor.GetString(leitor.GetOrdinal("no_plataforma")),
        Fabricante = leitor.GetString(leitor.GetOrdinal("no_fabricante")),
        AnoLancamento = leitor.GetInt32(leitor.GetOrdinal("nu_ano_lancamento")),
    };
}
