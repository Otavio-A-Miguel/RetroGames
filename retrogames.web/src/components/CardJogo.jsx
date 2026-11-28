import { Link } from 'react-router-dom'
import { formatarPreco } from '../lib/formato'

// Cartão de um jogo no catálogo. Só recebe dados por props.
export default function CardJogo({ jogo }) {
  const esgotado = jogo.estoque === 0

  return (
    <div className="card h-100">
      <div className="card-body d-flex flex-column">
        <small className="text-muted">{jogo.nomePlataforma}</small>
        <h2 className="h6 card-title mt-1">{jogo.nome}</h2>
        <p className="small text-muted mb-2">
          {jogo.anoLancamento} · {jogo.descricaoCondicaoJogo}
        </p>
        <div className="mb-3">
          {jogo.generos.map((genero) => (
            <span key={genero} className="badge text-bg-secondary me-1">{genero}</span>
          ))}
        </div>
        <div className="mt-auto d-flex justify-content-between align-items-center">
          <span className="fw-bold">{formatarPreco(jogo.preco)}</span>
          {esgotado && <span className="badge text-bg-danger">Esgotado</span>}
        </div>
      </div>
      <div className="card-footer bg-transparent">
        <Link to={`/jogos/${jogo.id}`} className="btn btn-outline-primary btn-sm w-100">
          Ver detalhes
        </Link>
      </div>
    </div>
  )
}
