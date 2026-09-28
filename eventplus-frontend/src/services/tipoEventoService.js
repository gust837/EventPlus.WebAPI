import api from "./api";

export const tipoEventoService = {
    listar: async () => {
        const resposta = await api.get('/TipoEvento');
        return resposta.data;
    }
};