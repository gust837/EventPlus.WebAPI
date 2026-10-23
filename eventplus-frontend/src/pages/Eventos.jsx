import { useState, useEffect } from "react";
import { eventoService } from '../services/eventoService';
import { tipoEventoService } from "../services/tipoEventoService";
import { Link } from "react-router-dom";
import './Eventos.css';

export function Eventos() {
  const [eventos, setEventos] = useState([]);
  const [carregando, setCarregado] = useState(true);
  const [tipos, setTipos] = useState([]);
  const [tipoSelecionado, setTipoSelecionado] = useState('todos');
  const [erro, setErro] = useState(null);

  async function carregarLista() {
    try {
      setCarregado(true);
      setErro(null);

      const dados = await eventoService.listar();

      setEventos(dados);

    try {
      const dadosTipos = await tipoEventoService.listar();
      setTipos(dadosTipos);
    } 
    catch (errTipos) {
      console.warn('Não foi possivel carregar os tipos.', errTipos) 
    }

  }catch (err) {
      console.error(err);
      setErro('Não foi possivel carregar a lista de eventos. Verifique se a API está ligada.')
    }
    finally {
      setCarregado(false);
    }
  }


  async function handleDeletar(id) {
    const confirmado = window.confirm('Tem certeza que deseja excluir este evento?');

    if (!confirmado) return;

    try {
      await eventoService.deletar(id);

      setEventos((listaAtual) => listaAtual.filter((e) => e.idEvento !== id));

    } catch (err) {
      console.error(err);
      alert('Não foi possivel excluir o evento.')
    }
  }

  useEffect(() => {
    carregarLista();
  }, []);

  const eventosFiltrados = tipoSelecionado === 'todos' ? eventos : eventos.filter(e => e.idTipoEvento === tipoSelecionado)

 return (
    <div className="eventos-pagina">
      <div className="eventos-container">
        
        {/* Cabeçalho com o botão de cadastrar novo */}
        <header className="eventos-header">
          <h1 className="eventos-titulo-secao">CATÁLOGO DE EVENTOS</h1>
          <Link to="/cadastrar-evento" className="btn-novo-evento">
            + Novo Evento
          </Link>
        </header>
        {/* Barra de Filtros em Pílula (Figma) */}
        <div className="eventos-filtros">
          <button
            className={`filtro-pill ${tipoSelecionado === 'todos' ? 'ativo' : ''}`}
            onClick={() => setTipoSelecionado('todos')}
          >
            Todos
          </button>
          {tipos.map((tipo) => (
            <button
              key={tipo.idTipoEvento}
              className={`filtro-pill ${tipoSelecionado === tipo.idTipoEvento ? 'ativo' : ''}`}
              onClick={() => setTipoSelecionado(tipo.idTipoEvento)}
            >
              {tipo.titulo}
            </button>
          ))}
        </div>
        {/* Feedback visual de Carregando / Erro */}
        {carregando && (
          <div className="eventos-feedback-container">
            <p className="mensagem-carregando">Carregando eventos...</p>
          </div>
        )}
        {erro && (
          <div className="eventos-feedback-container">
            <p className="mensagem-erro">{erro}</p>
            <button className="btn-tentar-novamente" onClick={carregarLista}>
              Tentar novamente
            </button>
          </div>
        )}
        {/* Grid de Cards 3x3 do Figma */}
        {!carregando && !erro && (
          eventosFiltrados.length === 0 ? (
            <p className="mensagem-vazio">Nenhum evento encontrado nesta categoria.</p>
          ) : (
            <div className="eventos-grid">
              {eventosFiltrados.map((evento) => (
                <div key={evento.idEvento} className="evento-card">
                  
                  {/* Imagem do Evento */}
                  <div className="card-imagem-wrapper">
                    {evento.imagemUrl ? (
                      <img src={evento.imagemUrl} alt={evento.nomeEvento} className="card-imagem" />
                    ) : (
                      <div className="card-sem-imagem">Sem imagem</div>
                    )}
                    {/* Badge / Tag de Categoria azulzinha */}
                    <span className="card-categoria-tag">
                      {evento.idTipoEventoNavigation?.titulo || 'Categoria'}
                    </span>
                  </div>
                  {/* Informações */}
                  <div className="card-corpo">
                    <h3 className="card-titulo">{evento.nomeEvento}</h3>
                    <p className="card-descricao">{evento.descricao}</p>
                    
                    <div className="card-rodape-acoes">
                      <Link to={`/eventos/${evento.idEvento}`} className="link-ver-evento">
                        Ver Evento
                      </Link>
                      <button 
                        onClick={() => handleDeletar(evento.idEvento)} 
                        className="btn-deletar-card"
                        title="Excluir evento"
                      >
                        🗑️
                      </button>
                    </div>
                  </div>
                </div>
              ))}
            </div>
          )
        )}
      </div>
      {/* Rodapé Senai padrão do Figma */}
      <footer className="eventos-rodape-institucional">
        Escola Senai de Informática - 2026
      </footer>
    </div>
  );
}