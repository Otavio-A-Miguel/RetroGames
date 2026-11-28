import { Link, NavLink, Outlet } from 'react-router-dom'

export default function Layout() {
  return (
    <div className="d-flex flex-column min-vh-100">
      <nav className="navbar navbar-expand-md navbar-dark bg-dark">
        <div className="container">
          <Link className="navbar-brand" to="/">RetroGames</Link>

          {/* Em tela estreita, o menu vira este botão (JavaScript do Bootstrap). */}
          <button
            className="navbar-toggler"
            type="button"
            data-bs-toggle="collapse"
            data-bs-target="#menuPrincipal"
            aria-controls="menuPrincipal"
            aria-expanded="false"
            aria-label="Abrir menu"
          >
            <span className="navbar-toggler-icon"></span>
          </button>

          <div className="collapse navbar-collapse" id="menuPrincipal">
            <ul className="navbar-nav me-auto">
              <li className="nav-item">
                {/* NavLink troca a página sem recarregar e marca o link ativo. */}
                <NavLink className="nav-link" to="/" end>Catálogo</NavLink>
              </li>
            </ul>
          </div>
        </div>
      </nav>

      <main className="container py-4 flex-grow-1">
        <Outlet />
      </main>

      <footer className="border-top bg-light py-3">
        <div className="container small text-muted">RetroGames · loja de jogos retrô usados</div>
      </footer>
    </div>
  )
}
