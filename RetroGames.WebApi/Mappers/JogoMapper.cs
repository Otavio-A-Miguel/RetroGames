using RetroGames.Model.Consultas;
using RetroGames.Model.Model;
using RetroGames.Services;
using RetroGames.WebApi.ApiModels;

namespace RetroGames.WebApi.Mappers;

public static class JogoMapper
{
    // Request da listagem → filtro do Service. O gênero vai à parte (o Service resolve com LINQ).
    public static JogoFiltro ParaFiltro(this ListarJogoRequestApiModel request) => new()
    {
        Busca = request.Busca,
        IdPlataforma = request.IdPlataforma,
        Pagina = request.Pagina,
        TamanhoPagina = request.TamanhoPagina,
        IncluirInativos = request.IncluirInativos,
    };

    // Entrada → modelo. Ativo, Cadastro e Id são definidos pelo Service e pelo banco.
    public static Jogo ParaModel(this InserirJogoRequestApiModel request) => new()
    {
        Nome = request.Nome,
        Descricao = request.Descricao,
        AnoLancamento = request.AnoLancamento,
        Preco = request.Preco,
        Estoque = request.Estoque,
        IdPlataforma = request.IdPlataforma,
        IdCondicaoJogo = request.IdCondicaoJogo,
    };

    // Na alteração, o id vem da rota, nunca do corpo.
    public static Jogo ParaModel(this AlterarJogoRequestApiModel request, int id) => new()
    {
        Id = id,
        Nome = request.Nome,
        Descricao = request.Descricao,
        AnoLancamento = request.AnoLancamento,
        Preco = request.Preco,
        Estoque = request.Estoque,
        Ativo = request.Ativo,
        IdPlataforma = request.IdPlataforma,
        IdCondicaoJogo = request.IdCondicaoJogo,
    };

    public static ListarJogoResponseApiModel ParaListarResponse(this JogoDetalheServiceModel detalhe) => new()
    {
        Id = detalhe.Jogo.Id,
        Nome = detalhe.Jogo.Nome,
        AnoLancamento = detalhe.Jogo.AnoLancamento,
        Preco = detalhe.Jogo.Preco,
        Estoque = detalhe.Jogo.Estoque,
        Ativo = detalhe.Jogo.Ativo,
        NomePlataforma = detalhe.NomePlataforma,
        DescricaoCondicaoJogo = detalhe.DescricaoCondicaoJogo,
        Generos = detalhe.Generos.Select(g => g.Nome).ToList(),
    };

    public static ListarPorIdJogoResponseApiModel ParaListarPorIdResponse(this JogoDetalheServiceModel detalhe) => new()
    {
        Id = detalhe.Jogo.Id,
        Nome = detalhe.Jogo.Nome,
        Descricao = detalhe.Jogo.Descricao,
        AnoLancamento = detalhe.Jogo.AnoLancamento,
        Preco = detalhe.Jogo.Preco,
        Estoque = detalhe.Jogo.Estoque,
        Ativo = detalhe.Jogo.Ativo,
        IdPlataforma = detalhe.Jogo.IdPlataforma,
        NomePlataforma = detalhe.NomePlataforma,
        IdCondicaoJogo = detalhe.Jogo.IdCondicaoJogo,
        DescricaoCondicaoJogo = detalhe.DescricaoCondicaoJogo,
        Generos = detalhe.Generos.Select(g => g.ParaResponse()).ToList(),
    };

    // Resultado do Service → página da API, com os totais.
    public static PaginaResponseApiModel<ListarJogoResponseApiModel> ParaPaginaResponse(
        this PaginaResultado<JogoDetalheServiceModel> resultado, int pagina, int tamanhoPagina) => new()
    {
        Itens = resultado.Itens.Select(d => d.ParaListarResponse()).ToList(),
        Pagina = pagina,
        TamanhoPagina = tamanhoPagina,
        TotalItens = resultado.Total,
        TotalPaginas = (int)Math.Ceiling(resultado.Total / (double)tamanhoPagina),
    };
}
