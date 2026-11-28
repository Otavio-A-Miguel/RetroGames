import api from './api'

// Uma função por endpoint. Cada uma devolve só o "data" do envelope
// { responseCode, data, errorMessage }. Gravação e login entram na aula 8.

export const jogoApi = {
  async listar(filtros) {
    const resposta = await api.get('/Jogo', { params: filtros })
    return resposta.data.data
  },
  async buscarPorId(id) {
    const resposta = await api.get(`/Jogo/${id}`)
    return resposta.data.data
  },
}

export const plataformaApi = {
  async listar() {
    const resposta = await api.get('/Plataforma')
    return resposta.data.data
  },
}

export const generoApi = {
  async listar() {
    const resposta = await api.get('/Genero')
    return resposta.data.data
  },
}

export const condicaoJogoApi = {
  async listar() {
    const resposta = await api.get('/CondicaoJogo')
    return resposta.data.data
  },
}
