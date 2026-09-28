import axios from 'axios';

// 1. Criamos a instância do Axios com a URL base da sua WebAPI (.NET)
// A sua WebAPI está configurada para rodar em http://localhost:5280
const api = axios.create({
  baseURL: 'https://localhost:7266/api',
  headers: {
    'Content-Type': 'application/json',
  },
});

// 2. Interceptor de Requisição:
// Toda vez que o React fizer uma chamada para a API, essa função roda antes.
// Ela verifica se já temos um Token JWT salvo no navegador (localStorage).
// Se existir, ela anexa o cabeçalho "Authorization: Bearer <token>" automaticamente!
api.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem('token');
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

// 3. Interceptor de Resposta (Opcional, mas muito útil):
// Se a API retornar erro 401 (não autorizado/token expirado), podemos limpar o login
api.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response && error.response.status === 401) {
      console.warn('Sessão expirada ou usuário não autenticado.');
      // localStorage.removeItem('token');
    }
    return Promise.reject(error);
  }
);

export default api;
