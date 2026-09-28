import api from "./api";

export const instituicaoService = {
    listar: async () => {
        const resposta = await api.get('/Instituicao');
        return resposta.data;
    }
};
