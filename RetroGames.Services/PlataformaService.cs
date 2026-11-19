using RetroGames.Model.Model;
using RetroGames.Repository;

namespace RetroGames.Services;

public class PlataformaService(IRepositorySet repositorio) : IPlataformaService
{
    public Task<List<Plataforma>> ListarAsync()
        => repositorio.Plataforma.ListarAsync();

    public async Task<Plataforma> BuscarPorIdAsync(int id)
    {
        return await repositorio.Plataforma.BuscarPorIdAsync(id)
            ?? throw new BusinessRuleException(BusinessErrors.PLATAFORMA_NAO_ENCONTRADA);
    }

    public async Task<Plataforma> InserirAsync(Plataforma plataforma)
    {
        plataforma.Nome = plataforma.Nome.Trim();

        // RN01: o nome não pode se repetir.
        if (await repositorio.Plataforma.ExisteComNomeAsync(plataforma.Nome))
        {
            throw new BusinessRuleException(BusinessErrors.PLATAFORMA_NOME_DUPLICADO);
        }

        try
        {
            plataforma.Id = await repositorio.Plataforma.InserirAsync(plataforma);
        }
        catch (RegistroDuplicadoException)
        {
            // Outra requisição gravou o mesmo nome entre a conferência e o INSERT.
            throw new BusinessRuleException(BusinessErrors.PLATAFORMA_NOME_DUPLICADO);
        }

        return plataforma;
    }

    public async Task<Plataforma> AlterarAsync(Plataforma plataforma)
    {
        await BuscarPorIdAsync(plataforma.Id);   // lança PLATAFORMA_NAO_ENCONTRADA se não existir

        plataforma.Nome = plataforma.Nome.Trim();

        // RN01, ignorando a própria plataforma: manter o próprio nome não é duplicidade.
        if (await repositorio.Plataforma.ExisteComNomeAsync(plataforma.Nome, plataforma.Id))
        {
            throw new BusinessRuleException(BusinessErrors.PLATAFORMA_NOME_DUPLICADO);
        }

        try
        {
            await repositorio.Plataforma.AlterarAsync(plataforma);
        }
        catch (RegistroDuplicadoException)
        {
            throw new BusinessRuleException(BusinessErrors.PLATAFORMA_NOME_DUPLICADO);
        }

        return plataforma;
    }

    public async Task ExcluirAsync(int id)
    {
        await BuscarPorIdAsync(id);   // lança PLATAFORMA_NAO_ENCONTRADA se não existir

        // RN02: plataforma com jogos não pode ser excluída.
        if (await repositorio.Plataforma.PossuiJogosAsync(id))
        {
            throw new BusinessRuleException(BusinessErrors.PLATAFORMA_POSSUI_JOGOS);
        }

        await repositorio.Plataforma.ExcluirAsync(id);
    }
}
