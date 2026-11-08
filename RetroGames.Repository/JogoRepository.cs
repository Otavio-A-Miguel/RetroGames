using System.Data.Common;
using RetroGames.Model.Consultas;
using RetroGames.Model.Model;

namespace RetroGames.Repository;

public class JogoRepository(RepositoryConfig config) : RepositoryBase(config), IJogoRepository
{
    private const string SelectJogo =
        "SELECT co_seq_jogo, no_jogo, ds_jogo, nu_ano_lancamento, vl_preco, qt_estoque, " +
        "st_ativo, dt_cadastro, co_plataforma, co_condicao_jogo FROM tb_jogo";

    public async Task<PaginaResultado<Jogo>> ListarAsync(JogoFiltro filtro)
    {
        // Lista de ids vazia: nenhum jogo pode atender ao filtro, não vale consultar o banco.
        if (filtro.Ids is { Count: 0 })
        {
            return new PaginaResultado<Jogo>();
        }

        var (where, adicionarParametros) = MontarFiltro(filtro);
        await using var conexao = await CriarConexaoAsync();

        // Total de jogos que atendem ao filtro (todas as páginas).
        await using var comandoTotal = CriarComando(conexao, "SELECT COUNT(*) FROM tb_jogo" + where);
        adicionarParametros(comandoTotal);
        var total = Convert.ToInt32(await comandoTotal.ExecuteScalarAsync());

        // Jogos da página pedida, com o mesmo filtro. A ordem inclui o id para a paginação ser estável.
        await using var comando = CriarComando(conexao,
            SelectJogo + where + " ORDER BY no_jogo, co_seq_jogo LIMIT @limite OFFSET @deslocamento");
        adicionarParametros(comando);
        AdicionarParametro(comando, "@limite", filtro.TamanhoPagina);
        AdicionarParametro(comando, "@deslocamento", (filtro.Pagina - 1) * filtro.TamanhoPagina);
        await using var leitor = await comando.ExecuteReaderAsync();

        var itens = new List<Jogo>();
        while (await leitor.ReadAsync())
        {
            itens.Add(Mapear(leitor));
        }
        return new PaginaResultado<Jogo> { Itens = itens, Total = total };
    }

    public async Task<Jogo?> BuscarPorIdAsync(int id)
    {
        await using var conexao = await CriarConexaoAsync();
        await using var comando = CriarComando(conexao, SelectJogo + " WHERE co_seq_jogo = @id");
        AdicionarParametro(comando, "@id", id);
        await using var leitor = await comando.ExecuteReaderAsync();

        return await leitor.ReadAsync() ? Mapear(leitor) : null;
    }

    public async Task<int> InserirAsync(Jogo jogo)
    {
        await using var conexao = await CriarConexaoAsync();
        await using var comando = CriarComando(conexao,
            "INSERT INTO tb_jogo (no_jogo, ds_jogo, nu_ano_lancamento, vl_preco, qt_estoque, " +
            "st_ativo, dt_cadastro, co_plataforma, co_condicao_jogo) " +
            "VALUES (@nome, @descricao, @anoLancamento, @preco, @estoque, " +
            "@ativo, @cadastro, @idPlataforma, @idCondicaoJogo)");
        AdicionarParametro(comando, "@nome", jogo.Nome);
        AdicionarParametro(comando, "@descricao", jogo.Descricao);
        AdicionarParametro(comando, "@anoLancamento", jogo.AnoLancamento);
        AdicionarParametro(comando, "@preco", jogo.Preco);
        AdicionarParametro(comando, "@estoque", jogo.Estoque);
        AdicionarParametro(comando, "@ativo", jogo.Ativo);
        AdicionarParametro(comando, "@cadastro", jogo.Cadastro);
        AdicionarParametro(comando, "@idPlataforma", jogo.IdPlataforma);
        AdicionarParametro(comando, "@idCondicaoJogo", jogo.IdCondicaoJogo);

        return (int)await ExecutarGravacaoAsync(comando);
    }

    public async Task AlterarAsync(Jogo jogo)
    {
        await using var conexao = await CriarConexaoAsync();
        await using var comando = CriarComando(conexao,
            "UPDATE tb_jogo SET no_jogo = @nome, ds_jogo = @descricao, nu_ano_lancamento = @anoLancamento, " +
            "vl_preco = @preco, qt_estoque = @estoque, co_plataforma = @idPlataforma, " +
            "co_condicao_jogo = @idCondicaoJogo WHERE co_seq_jogo = @id");
        AdicionarParametro(comando, "@nome", jogo.Nome);
        AdicionarParametro(comando, "@descricao", jogo.Descricao);
        AdicionarParametro(comando, "@anoLancamento", jogo.AnoLancamento);
        AdicionarParametro(comando, "@preco", jogo.Preco);
        AdicionarParametro(comando, "@estoque", jogo.Estoque);
        AdicionarParametro(comando, "@idPlataforma", jogo.IdPlataforma);
        AdicionarParametro(comando, "@idCondicaoJogo", jogo.IdCondicaoJogo);
        AdicionarParametro(comando, "@id", jogo.Id);

        await ExecutarGravacaoAsync(comando);
    }

    // Exclusão lógica (RN03): o jogo nunca é apagado, só marcado como inativo.
    public async Task DesativarAsync(int id)
    {
        await using var conexao = await CriarConexaoAsync();
        await using var comando = CriarComando(conexao,
            "UPDATE tb_jogo SET st_ativo = FALSE WHERE co_seq_jogo = @id");
        AdicionarParametro(comando, "@id", id);

        await ExecutarGravacaoAsync(comando);
    }

    // Monta o WHERE só com os filtros informados. O texto do SQL leva apenas
    // nomes de parâmetros (@busca, @id0...); os valores vão separados, como parâmetros.
    private static (string Where, Action<DbCommand> AdicionarParametros) MontarFiltro(JogoFiltro filtro)
    {
        var condicoes = new List<string>();
        var parametros = new List<(string Nome, object Valor)>();

        if (!filtro.IncluirInativos)
        {
            condicoes.Add("st_ativo = TRUE");
        }
        if (!string.IsNullOrWhiteSpace(filtro.Busca))
        {
            condicoes.Add("no_jogo LIKE @busca");
            parametros.Add(("@busca", $"%{filtro.Busca}%"));
        }
        if (filtro.IdPlataforma is not null)
        {
            condicoes.Add("co_plataforma = @idPlataforma");
            parametros.Add(("@idPlataforma", filtro.IdPlataforma.Value));
        }
        if (filtro.Ids is not null)
        {
            var nomes = filtro.Ids.Select((_, i) => $"@id{i}").ToList();
            condicoes.Add($"co_seq_jogo IN ({string.Join(", ", nomes)})");
            for (var i = 0; i < filtro.Ids.Count; i++)
            {
                parametros.Add((nomes[i], filtro.Ids[i]));
            }
        }

        var where = condicoes.Count == 0 ? "" : " WHERE " + string.Join(" AND ", condicoes);
        return (where, comando =>
        {
            foreach (var (nome, valor) in parametros)
            {
                AdicionarParametro(comando, nome, valor);
            }
        });
    }

    private static Jogo Mapear(DbDataReader leitor)
    {
        var ordemDescricao = leitor.GetOrdinal("ds_jogo");
        return new Jogo
        {
            Id = leitor.GetInt32(leitor.GetOrdinal("co_seq_jogo")),
            Nome = leitor.GetString(leitor.GetOrdinal("no_jogo")),
            Descricao = leitor.IsDBNull(ordemDescricao) ? null : leitor.GetString(ordemDescricao),
            AnoLancamento = leitor.GetInt32(leitor.GetOrdinal("nu_ano_lancamento")),
            Preco = leitor.GetDecimal(leitor.GetOrdinal("vl_preco")),
            Estoque = leitor.GetInt32(leitor.GetOrdinal("qt_estoque")),
            Ativo = leitor.GetBoolean(leitor.GetOrdinal("st_ativo")),
            Cadastro = leitor.GetDateTime(leitor.GetOrdinal("dt_cadastro")),
            IdPlataforma = leitor.GetInt32(leitor.GetOrdinal("co_plataforma")),
            IdCondicaoJogo = leitor.GetInt32(leitor.GetOrdinal("co_condicao_jogo")),
        };
    }
}
