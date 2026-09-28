import api from './api';

export const eventoService = {
  // GET: api/Evento - Lista todos os eventos
  listar: async () => {
    try {
      const response = await api.get('/Evento');
      return response.data;
    } catch (error) {
      console.error('Erro ao listar eventos:', error.response?.data || error.message);
      throw error;
    }
  },

  // GET: api/Evento/{id} - Busca um evento específico
  buscarPorId: async (id) => {
    try {
      const response = await api.get(`/Evento/${id}`);
      return response.data;
    } catch (error) {
      console.error(`Erro ao buscar evento ${id}:`, error.response?.data || error.message);
      throw error;
    }
  },

  // POST: api/Evento - Cadastra um evento (suporta upload com FormData)
  cadastrar: async (formDataOuObjeto) => {
    try {
      // Como a API aceita multipart/form-data para imagem,
      // se passarmos um FormData, o Axios ajusta os headers automaticamente
      const response = await api.post('/Evento', formDataOuObjeto, {
        headers: {
          'Content-Type': 'multipart/form-data',
        },
      });
      return response.data;
    } catch (error) {
      console.error('Erro ao cadastrar evento:', error.response?.data || error.message);
      throw error;
    }
  },

  // PUT: api/Evento/{id} - Atualiza um evento
  atualizar: async (id, dadosEvento) => {
    try {
      const response = await api.put(`/Evento/${id}`, dadosEvento);
      return response.data;
    } catch (error) {
      console.error(`Erro ao atualizar evento ${id}:`, error.response?.data || error.message);
      throw error;
    }
  },

  // DELETE: api/Evento/{id} - Deleta um evento
  deletar: async (id) => {
    try {
      await api.delete(`/Evento/${id}`);
    } catch (error) {
      console.error(`Erro ao deletar evento ${id}:`, error.response?.data || error.message);
      throw error;
    }
  },
};
