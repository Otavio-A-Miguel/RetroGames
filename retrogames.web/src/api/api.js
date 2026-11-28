import axios from 'axios'

// Única instância do Axios do projeto. Nenhum componente importa o Axios diretamente.
const api = axios.create({
  baseURL: '/api',        // o proxy do Vite repassa para a API na porta 5080
  withCredentials: true,  // o cookie de sessão vai junto em toda requisição
})

export default api
