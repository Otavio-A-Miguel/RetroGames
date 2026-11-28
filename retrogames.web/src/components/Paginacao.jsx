// Anterior, números e Próxima. Não aparece quando há uma página só.
export default function Paginacao({ pagina, totalPaginas, aoMudar }) {
  if (totalPaginas <= 1) {
    return null
  }

  const numeros = Array.from({ length: totalPaginas }, (_, indice) => indice + 1)

  return (
    <nav aria-label="Paginação do catálogo">
      <ul className="pagination justify-content-center mt-4">
        <li className={`page-item ${pagina === 1 ? 'disabled' : ''}`}>
          <button type="button" className="page-link" disabled={pagina === 1}
                  onClick={() => aoMudar(pagina - 1)}>
            Anterior
          </button>
        </li>

        {numeros.map((numero) => (
          <li key={numero} className={`page-item ${numero === pagina ? 'active' : ''}`}>
            <button type="button" className="page-link"
                    aria-current={numero === pagina ? 'page' : undefined}
                    onClick={() => aoMudar(numero)}>
              {numero}
            </button>
          </li>
        ))}

        <li className={`page-item ${pagina === totalPaginas ? 'disabled' : ''}`}>
          <button type="button" className="page-link" disabled={pagina === totalPaginas}
                  onClick={() => aoMudar(pagina + 1)}>
            Próxima
          </button>
        </li>
      </ul>
    </nav>
  )
}
