import { useState, useEffect } from "react";
import { eventoService } from '../services/eventoService'
import { Link } from "react-router-dom";
import './Eventos.css';

export function Eventos() {
    const [eventos, setEventos] = useState([]);
    const [carregando, setCarregado] = useState(true);
    const [erro, setErro] = useState(null);

    async function carregarLista() {
        try {
            setCarregado(true);
            setErro(null);

            const dados = await eventoService.listar();

            setEventos(dados);
        }
        catch (err) {
            console.error(err);
            setErro('Não foi possivel carregar a lista de eventos. Verifique se a API está ligada.')
        } 
        finally {
            setCarregado(false);
        }
    }


    useEffect(() => {
        carregarLista();
    }, []);

      return (
    <div className="eventos-container">
      {/* O Cabeçalho fica sempre visível, com o botão! */}
      <header className="eventos-header">
        <h1>Eventos Cadastrados</h1>
        <Link to="/cadastrar-evento" className="btn-novo-evento">
          + Novo evento
        </Link>
      </header>

      {/* 1. Se estiver carregando, mostra o aviso aqui embaixo: */}
      {carregando && (
        <div className="eventos-feedback-container">
          <p className="mensagem-carregando">Carregando eventos...</p>
        </div>
      )}

      {/* 2. Se deu erro, mostra o erro e o botão de tentar novamente: */}
      {erro && (
        <div className="eventos-feedback-container">
          <p className="mensagem-erro">{erro}</p>
          <button className="btn-tentar-novamente" onClick={carregarLista}>
            Tentar novamente
          </button>
        </div>
      )}

      {/* 3. Se terminou de carregar e não deu erro: */}
      {!carregando && !erro && (
        eventos.length === 0 ? (
          <p className="mensagem-vazio">Nenhum evento encontrado.</p>
        ) : (
          <div className="tabela-container">
            <table className="tabela-eventos">
              <thead>
                <tr>
                  <th>Nome</th>
                  <th>Descrição</th>
                  <th>Data</th>
                </tr>
              </thead>
              <tbody>
                {/* Repare: 'evento' no singular tanto no parâmetro quanto nos campos: */}
                {eventos.map((evento) => (
                  <tr key={evento.idEvento}>
                    <td>{evento.nomeEvento}</td>
                    <td>{evento.descricao}</td>
                    <td>{new Date(evento.dataEvento).toLocaleDateString('pt-BR')}</td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        )
      )}
    </div>
  );
}