// Transforma uma falha de requisição em mensagem para o usuário.
export function obterMensagemErro(erro) {
  // Resposta da nossa API: o envelope traz a mensagem pronta ("Jogo não encontrado.").
  const mensagem = erro?.response?.data?.errorMessage
  if (mensagem) {
    return mensagem
  }

  // Sem resposta, ou resposta sem envelope: não chegamos à API.
  // (Com a API parada, o proxy do Vite responde 500 sem corpo.)
  return 'Não foi possível conectar ao servidor.'
}
