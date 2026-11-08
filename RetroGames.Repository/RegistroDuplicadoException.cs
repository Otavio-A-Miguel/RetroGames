namespace RetroGames.Repository;

/// <summary>
/// Lançada quando o MySQL recusa um valor repetido numa coluna UNIQUE (erro 1062).
/// </summary>
public class RegistroDuplicadoException(Exception innerException)
    : Exception("Já existe um registro com este valor.", innerException);
