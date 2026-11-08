using System.Data.Common;
using MySqlConnector;

namespace RetroGames.Repository;

public abstract class RepositoryBase(RepositoryConfig config)
{
    // Abre uma conexão nova. Quem chama fecha com "await using".
    protected async Task<DbConnection> CriarConexaoAsync()
    {
        var conexao = new MySqlConnection(config.ConnectionString);
        try
        {
            await conexao.OpenAsync();
        }
        catch
        {
            await conexao.DisposeAsync();
            throw;
        }
        return conexao;
    }

    // Cria um comando com o texto SQL, ligado à conexão.
    protected static DbCommand CriarComando(DbConnection conexao, string sql)
    {
        var comando = conexao.CreateCommand();
        comando.CommandText = sql;
        return comando;
    }

    // Todo valor entra como parâmetro (@nome), nunca concatenado no SQL.
    protected static void AdicionarParametro(DbCommand comando, string nome, object? valor)
    {
        var parametro = comando.CreateParameter();
        parametro.ParameterName = nome;
        parametro.Value = valor ?? DBNull.Value;
        comando.Parameters.Add(parametro);
    }

    // Executa INSERT ou UPDATE. Devolve o id gerado (só faz sentido no INSERT).
    // É o único lugar que converte o erro 1062 do MySQL.
    protected static async Task<long> ExecutarGravacaoAsync(DbCommand comando)
    {
        try
        {
            await comando.ExecuteNonQueryAsync();
            return ((MySqlCommand)comando).LastInsertedId;
        }
        catch (MySqlException ex) when (ex.ErrorCode == MySqlErrorCode.DuplicateKeyEntry)
        {
            throw new RegistroDuplicadoException(ex);
        }
    }
}
