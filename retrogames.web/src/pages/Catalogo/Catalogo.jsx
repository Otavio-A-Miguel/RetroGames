import { useEffect, useState } from 'react'
import { useSearchParams } from 'react-router-dom'
import { generoApi, jogoApi, plataformaApi } from '../../api/servicos'
import { obterMensagemErro } from '../../api/erros'
import CardJogo from '../../components/CardJogo'
import Paginacao from '../../components/Paginacao'
import { Carregando, EstadoVazio, MensagemErro } from '../../components/MensagensEstado'

const TAMANHO_PAGINA = 12

export default function Catalogo() {
  // Os filtros moram na URL: ?busca=...&idPlataforma=...&idGenero=...&pagina=...
  const [searchParams, setSearchParams] = useSearchParams()
  const busca = searchParams.get('busca') ?? ''
  const idPlataforma = searchParams.get('idPlataforma') ?? ''
  const idGenero = searchParams.get('idGenero') ?? ''
  const pagina = Number(searchParams.get('pagina')) || 1

  const [termo, setTermo] = useState(busca)   // o que está digitado na caixa de busca
  const [plataformas, setPlataformas] = useState([])
  const [generos, setGeneros] = useState([])
  const [resultado, setResultado] = useState(null)
  const [carregando, setCarregando] = useState(true)
  const [erro, setErro] = useState(null)
  const [tentativa, setTentativa] = useState(0)   // muda ao clicar em "Tentar novamente"

  // Listas dos filtros: carregam uma vez. Se falharem, os filtros ficam vazios
  // e o erro aparece na lista de jogos, que usa a mesma API.
  useEffect(() => {
    plataformaApi.listar().then(setPlataformas).catch(() => {})
    generoApi.listar().then(setGeneros).catch(() => {})
  }, [])

  // Botão Voltar: quando a busca da URL muda, a caixa de texto acompanha.
  useEffect(() => {
    setTermo(busca)
  }, [busca])

  // Jogos: recarregam sempre que um filtro da URL muda.
  useEffect(() => {
    let atual = true   // ignora respostas de uma busca que já foi substituída
    setCarregando(true)
    setErro(null)

    jogoApi.listar({
      busca: busca || undefined,
      idPlataforma: idPlataforma || undefined,
      idGenero: idGenero || undefined,
      pagina,
      tamanhoPagina: TAMANHO_PAGINA,
    })
      .then((dados) => { if (atual) setResultado(dados) })
      .catch((e) => { if (atual) setErro(obterMensagemErro(e)) })
      .finally(() => { if (atual) setCarregando(false) })

    return () => { atual = false }
  }, [busca, idPlataforma, idGenero, pagina, tentativa])

  // Grava filtros na URL. Trocar um filtro volta à página 1.
  function atualizarFiltros(alteracoes) {
    const novos = new URLSearchParams(searchParams)
    for (const [chave, valor] of Object.entries(alteracoes)) {
      if (valor) {
        novos.set(chave, valor)
      } else {
        novos.delete(chave)
      }
    }
    if (!('pagina' in alteracoes)) {
      novos.delete('pagina')
    }
    setSearchParams(novos)
  }

  function aoBuscar(evento) {
    evento.preventDefault()   // não recarrega a página
    atualizarFiltros({ busca: termo.trim() })
  }

  function mudarPagina(numero) {
    atualizarFiltros({ pagina: numero > 1 ? String(numero) : '' })
    window.scrollTo(0, 0)
  }

  let conteudo
  if (carregando) {
    conteudo = <Carregando texto="Carregando jogos..." />
  } else if (erro) {
    conteudo = <MensagemErro mensagem={erro} aoTentarNovamente={() => setTentativa((t) => t + 1)} />
  } else if (!resultado || resultado.itens.length === 0) {
    conteudo = (
      <EstadoVazio titulo="Nenhum jogo encontrado">
        <p>Tente outra busca ou limpe os filtros.</p>
      </EstadoVazio>
    )
  } else {
    conteudo = (
      <>
        <p className="text-muted small">{resultado.totalItens} jogo(s) encontrado(s)</p>
        <div className="row row-cols-1 row-cols-sm-2 row-cols-lg-3 row-cols-xl-4 g-3">
          {resultado.itens.map((jogo) => (
            <div key={jogo.id} className="col">
              <CardJogo jogo={jogo} />
            </div>
          ))}
        </div>
        <Paginacao pagina={resultado.pagina} totalPaginas={resultado.totalPaginas} aoMudar={mudarPagina} />
      </>
    )
  }

  return (
    <>
      <h1 className="h3 mb-3">Catálogo</h1>

      <form className="row g-2 mb-4" onSubmit={aoBuscar}>
        <div className="col-md-5">
          <input type="search" className="form-control" placeholder="Buscar pelo nome"
                 aria-label="Buscar pelo nome" value={termo}
                 onChange={(e) => setTermo(e.target.value)} />
        </div>
        <div className="col-md-3">
          <select className="form-select" aria-label="Plataforma" value={idPlataforma}
                  onChange={(e) => atualizarFiltros({ idPlataforma: e.target.value })}>
            <option value="">Todas as plataformas</option>
            {plataformas.map((p) => <option key={p.id} value={p.id}>{p.nome}</option>)}
          </select>
        </div>
        <div className="col-md-3">
          <select className="form-select" aria-label="Gênero" value={idGenero}
                  onChange={(e) => atualizarFiltros({ idGenero: e.target.value })}>
            <option value="">Todos os gêneros</option>
            {generos.map((g) => <option key={g.id} value={g.id}>{g.nome}</option>)}
          </select>
        </div>
        <div className="col-md-1 d-grid">
          <button type="submit" className="btn btn-primary">Buscar</button>
        </div>
      </form>

      {conteudo}
    </>
  )
}
