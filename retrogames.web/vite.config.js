import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

export default defineConfig({
  plugins: [react()],
  server: {
    port: 5173,
    strictPort: true, // se a 5173 estiver ocupada, para com erro em vez de mudar de porta
    proxy: {
      // Tudo que começa com /api vai para a API. Para o navegador, é a mesma origem.
      '/api': 'http://localhost:5080',
    },
  },
})
