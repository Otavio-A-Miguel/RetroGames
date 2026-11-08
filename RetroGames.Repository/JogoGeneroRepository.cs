using System.Data.Common;
using RetroGames.Model.Model;

namespace RetroGames.Repository;

public class JogoGeneroRepository(RepositoryConfig config)
    : RepositoryBase(config), IJogoGeneroRepository
{
    private const string SelectJogoGenero = "SELECT co_jogo, co_genero FROM tb_jogo_genero";

    public async Task<List<JogoGenero>> ListarTodosAsync()
    {
        await using var conexao = await CriarConexaoAsync();
        await using var comando = CriarComando(conexao,
            SelectJogoGenero + " ORDER BY co_jogo, co_genero");

        return await LerAsync(comando);
    }

    public async Task<List<JogoGenero>> ListarPorJogosAsync(IReadOnlyCollection<int> idsJogos)
    {
        // Sem jogos, o IN () seria erro de SQL: não há o que consultar.
        if (idsJogos.Count == 0)
        {
            return [];
        }

        var ids = idsJogos.ToList();
        var nomes = ids.Select((_, i) => $"@jogo{i}").ToList();

        await using var conexao = await CriarConexaoAsync();
        await using var comando = CriarComando(conexao,
            SelectJogoGenero + $" WHERE co_jogo IN ({string.Join(", ", nomes)}) ORDER BY co_jogo, co_genero");
        for (var i = 0; i < ids.Count; i++)
        {
            AdicionarParametro(comando, nomes[i], ids[i]);
        }

        return await LerAsync(comando);
    }

    public async Task InserirAsync(int idJogo, IReadOnlyCollection<int> idsGeneros)
    {
        if (idsGeneros.Count == 0)
        {
            return;
        }

        var generos = idsGeneros.ToList();
        var valores = generos.Select((_, i) => $"(@idJogo, @genero{i})");

        await using var conexao = await CriarConexaoAsync();
        await using var comando = CriarComando(conexao,
            "INSERT INTO tb_jogo_genero (co_jogo, co_genero) VALUES " + string.Join(", ", valores));
        AdicionarParametro(comando, "@idJogo", idJogo);
        for (var i = 0; i < generos.Count; i++)
        {
            AdicionarParametro(comando, $"@genero{i}", generos[i]);
        }

        await ExecutarGravacaoAsync(comando);
    }

    public async Task RemoverPorJogoAsync(int idJogo)
    {
        await using var conexao = await CriarConexaoAsync();
        await using var comando = CriarComando(conexao,
            "DELETE FROM tb_jogo_genero WHERE co_jogo = @idJogo");
        AdicionarParametro(comando, "@idJogo", idJogo);

        await comando.ExecuteNonQueryAsync();
    }

    // Lê todas as linhas do resultado e converte cada uma em um JogoGenero.
    private static async Task<List<JogoGenero>> LerAsync(DbCommand comando)
    {
        await using var leitor = await comando.ExecuteReaderAsync();

        var vinculos = new List<JogoGenero>();
        while (await leitor.ReadAsync())
        {
            vinculos.Add(new JogoGenero
            {
                IdJogo = leitor.GetInt32(leitor.GetOrdinal("co_jogo")),
                IdGenero = leitor.GetInt32(leitor.GetOrdinal("co_genero")),
            });
        }
        return vinculos;
    }
}
