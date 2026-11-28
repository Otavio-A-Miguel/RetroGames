import { Link } from 'react-router-dom'

export default function NaoEncontrada() {
  return (
    <div className="text-center my-5">
      <h1 className="h3">Página não encontrada</h1>
      <p className="text-muted">O endereço que você abriu não existe.</p>
      <Link to="/" className="btn btn-primary">Voltar ao catálogo</Link>
    </div>
  )
}
