import api from './api';

export const authService = {
  // Faz o login enviando email e senha para a API
  login: async (email, senha) => {
    try {
      // POST http://localhost:5280/api/Login
      const response = await api.post('/Login', { email, senha });

      // Se deu certo, a API retorna { token, expiracao, usuario }
      const { token, usuario } = response.data;

      // Salvamos o token no localStorage para que o api.js envie nas próximas requisições
      if (token) {
        localStorage.setItem('token', token);
        localStorage.setItem('usuario', JSON.stringify(usuario));
      }

      return response.data;
    } catch (error) {
      console.error('Erro no login:', error.response?.data || error.message);
      throw error;
    }
  },

  // Faz logout limpando os dados do navegador
  logout: () => {
    localStorage.removeItem('token');
    localStorage.removeItem('usuario');
  },

  // Retorna os dados do usuário autenticado salvo localmente
  getUsuarioLogado: () => {
    const usuario = localStorage.getItem('usuario');
    return usuario ? JSON.parse(usuario) : null;
  },

  // Verifica se existe um token salvo
  estaAutenticado: () => {
    return !!localStorage.getItem('token');
  },
};
