using System.Transactions;
using RetroGames.Model.Consultas;
using RetroGames.Model.Model;
using RetroGames.Repository;

namespace RetroGames.Services;

public class JogoService(IRepositorySet repositorio, IUsuarioAtualService usuarioAtual) : IJogoService
{
    public async Task<PaginaResultado<JogoDetalheServiceModel>> ListarAsync(JogoFiltro filtro, int? idGenero)
    {
        // Filtro por gênero: resolvido aqui, com LINQ, e entregue ao Repository
        // como lista de ids. A contagem e a paginação continuam no SQL.
        filtro.Ids = null;
        if (idGenero is not null)
        {
            var vinculos = await repositorio.JogoGenero.ListarTodosAsync();
            filtro.Ids = vinculos
                .Where(v => v.IdGenero == idGenero)
                .Select(v => v.IdJogo)
                .ToList();
        }

        // RN04: inativos só para o administrador, e só quando ele pede.
        filtro.IncluirInativos = filtro.IncluirInativos && usuarioAtual.EhAdministrador;

        var pagina = await repositorio.Jogo.ListarAsync(filtro);
        return new PaginaResultado<JogoDetalheServiceModel>
        {
            Itens = await MontarDetalhesAsync(pagina.Itens),
            Total = pagina.Total,
        };
    }

    public async Task<JogoDetalheServiceModel> BuscarPorIdAsync(int id)
    {
        var jogo = await repositorio.Jogo.BuscarPorIdAsync(id)
            ?? throw new BusinessRuleException(BusinessErrors.JOGO_NAO_ENCONTRADO);

        // RN04: para quem não é administrador, jogo inativo não existe.
        if (!jogo.Ativo && !usuarioAtual.EhAdministrador)
        {
            throw new BusinessRuleException(BusinessErrors.JOGO_NAO_ENCONTRADO);
        }

        return (await MontarDetalhesAsync([jogo]))[0];
    }

    public async Task<JogoDetalheServiceModel> InserirAsync(Jogo jogo, List<int> idsGeneros)
    {
        jogo.Nome = jogo.Nome.Trim();
        jogo.Ativo = true;
        jogo.Cadastro = DateTime.UtcNow;
        var generos = idsGeneros.Distinct().ToList();

        await ValidarReferenciasAsync(jogo, generos);   // RN05

        var transactionOptions = new TransactionOptions
        {
            IsolationLevel = IsolationLevel.ReadCommitted,
            Timeout = TransactionManager.DefaultTimeout,
        };
        using (var transactionScope = new TransactionScope(TransactionScopeOption.Required,
                   transactionOptions, TransactionScopeAsyncFlowOption.Enabled))
        {
            jogo.Id = await repositorio.Jogo.InserirAsync(jogo);
            await repositorio.JogoGenero.InserirAsync(jogo.Id, generos);
            transactionScope.Complete();   // só depois da última gravação
        }

        return await BuscarPorIdAsync(jogo.Id);
    }

    public async Task<JogoDetalheServiceModel> AlterarAsync(Jogo jogo, List<int> idsGeneros)
    {
        _ = await repositorio.Jogo.BuscarPorIdAsync(jogo.Id)
            ?? throw new BusinessRuleException(BusinessErrors.JOGO_NAO_ENCONTRADO);

        jogo.Nome = jogo.Nome.Trim();
        var generos = idsGeneros.Distinct().ToList();

        await ValidarReferenciasAsync(jogo, generos);   // RN05

        var transactionOptions = new TransactionOptions
        {
            IsolationLevel = IsolationLevel.ReadCommitted,
            Timeout = TransactionManager.DefaultTimeout,
        };
        using (var transactionScope = new TransactionScope(TransactionScopeOption.Required,
                   transactionOptions, TransactionScopeAsyncFlowOption.Enabled))
        {
            await repositorio.Jogo.AlterarAsync(jogo);
            await repositorio.JogoGenero.RemoverPorJogoAsync(jogo.Id);
            await repositorio.JogoGenero.InserirAsync(jogo.Id, generos);
            transactionScope.Complete();   // só depois da última gravação
        }

        return await BuscarPorIdAsync(jogo.Id);
    }

    // RN03: o jogo nunca é apagado, só marcado como inativo.
    public async Task DesativarAsync(int id)
    {
        _ = await repositorio.Jogo.BuscarPorIdAsync(id)
            ?? throw new BusinessRuleException(BusinessErrors.JOGO_NAO_ENCONTRADO);

        await repositorio.Jogo.DesativarAsync(id);
    }

    // RN05: plataforma, condição e gêneros informados precisam existir.
    private async Task ValidarReferenciasAsync(Jogo jogo, List<int> idsGeneros)
    {
        if (await repositorio.Plataforma.BuscarPorIdAsync(jogo.IdPlataforma) is null)
        {
            throw new BusinessRuleException(BusinessErrors.PLATAFORMA_NAO_ENCONTRADA);
        }

        var condicoes = await repositorio.CondicaoJogo.ListarAsync();
        if (!condicoes.Any(c => c.Id == jogo.IdCondicaoJogo))
        {
            throw new BusinessRuleException(BusinessErrors.CONDICAO_NAO_ENCONTRADA);
        }

        var generos = await repositorio.Genero.ListarAsync();
        if (idsGeneros.Any(id => !generos.Any(g => g.Id == id)))
        {
            throw new BusinessRuleException(BusinessErrors.GENERO_NAO_ENCONTRADO);
        }
    }

    // Monta o detalhe de uma lista de jogos sempre com 4 consultas,
    // qualquer que seja o tamanho da lista (nunca uma consulta por jogo).
    private async Task<List<JogoDetalheServiceModel>> MontarDetalhesAsync(List<Jogo> jogos)
    {
        if (jogos.Count == 0)
        {
            return [];
        }

        var plataformas = await repositorio.Plataforma.ListarAsync();
        var condicoes = await repositorio.CondicaoJogo.ListarAsync();
        var generos = await repositorio.Genero.ListarAsync();
        var vinculos = await repositorio.JogoGenero.ListarPorJogosAsync(jogos.Select(j => j.Id).ToList());

        return (from jogo in jogos
                join plataforma in plataformas on jogo.IdPlataforma equals plataforma.Id
                join condicao in condicoes on jogo.IdCondicaoJogo equals condicao.Id
                select new JogoDetalheServiceModel
                {
                    Jogo = jogo,
                    NomePlataforma = plataforma.Nome,
                    DescricaoCondicaoJogo = condicao.Descricao,
                    Generos = (from vinculo in vinculos
                               where vinculo.IdJogo == jogo.Id
                               join genero in generos on vinculo.IdGenero equals genero.Id
                               orderby genero.Nome
                               select genero).ToList(),
                }).ToList();
    }
}
