import { useEffect, useState } from 'react'
import { Link, useParams } from 'react-router-dom'
import { jogoApi } from '../../api/servicos'
import { obterMensagemErro } from '../../api/erros'
import { formatarPreco } from '../../lib/formato'
import { Carregando, MensagemErro } from '../../components/MensagensEstado'

export default function DetalheJogo() {
  const { id } = useParams()   // /jogos/7 → "7"
  const [jogo, setJogo] = useState(null)
  const [carregando, setCarregando] = useState(true)
  const [erro, setErro] = useState(null)          // { mensagem, podeTentar }
  const [tentativa, setTentativa] = useState(0)

  useEffect(() => {
    // /jogos/abc nem chega à API: só números são ids válidos.
    if (!/^\d+$/.test(id)) {
      setErro({ mensagem: 'Jogo não encontrado.', podeTentar: false })
      setCarregando(false)
      return
    }

    let atual = true
    setCarregando(true)
    setErro(null)

    jogoApi.buscarPorId(id)
      .then((dados) => { if (atual) setJogo(dados) })
      .catch((e) => {
        if (atual) {
          // 404 (inexistente ou inativo): tentar de novo não adianta.
          setErro({ mensagem: obterMensagemErro(e), podeTentar: e?.response?.status !== 404 })
        }
      })
      .finally(() => { if (atual) setCarregando(false) })

    return () => { atual = false }
  }, [id, tentativa])

  const voltar = <Link to="/" className="btn btn-link px-0 mb-3">← Voltar ao catálogo</Link>

  if (carregando) {
    return <>{voltar}<Carregando texto="Carregando jogo..." /></>
  }

  if (erro) {
    return (
      <>
        {voltar}
        <MensagemErro
          mensagem={erro.mensagem}
          aoTentarNovamente={erro.podeTentar ? () => setTentativa((t) => t + 1) : undefined}
        />
      </>
    )
  }

  return (
    <>
      {voltar}
      <div className="row g-4">
        <div className="col-lg-8">
          <small className="text-muted">{jogo.nomePlataforma}</small>
          <h1 className="h3">{jogo.nome}</h1>
          <p className="text-muted">{jogo.anoLancamento} · {jogo.descricaoCondicaoJogo}</p>

          <div className="mb-3">
            {jogo.generos.map((genero) => (
              <span key={genero.id} className="badge text-bg-secondary me-1">{genero.nome}</span>
            ))}
          </div>

          {/* Descrição é opcional (ds_jogo aceita NULL): nunca mostra "null". */}
          {jogo.descricao
            ? <p>{jogo.descricao}</p>
            : <p className="text-muted fst-italic">Sem descrição.</p>}
        </div>

        <div className="col-lg-4">
          <div className="card">
            <div className="card-body">
              <p className="h4 mb-2">{formatarPreco(jogo.preco)}</p>
              {jogo.estoque > 0
                ? <p className="text-success mb-0">{jogo.estoque} em estoque</p>
                : <span className="badge text-bg-danger">Esgotado</span>}
            </div>
          </div>
        </div>
      </div>
    </>
  )
}
