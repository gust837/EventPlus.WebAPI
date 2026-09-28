import { useState, useEffect } from "react";
import { eventoService } from "../services/eventoService";
import {tipoEventoService} from '../services/tipoEventoService';
import {instituicaoService} from '../services/instituicaoService';
import { Link } from "react-router-dom";
import './CadastrarEvento.css';

export function CadastrarEvento(){
    const [listaEventos, setListaEventos] = useState([]);
    const [tiposEvento, setTiposEvento] = useState([]);
    const [instituicoes, setInstituicoes] = useState([]);
    
    const [nome, setNome] = useState('');
    const [descricao, setDescricao] = useState('');
    const [data, setData] = useState('');
    const [idTipoEvento, setIdTipoEvento] = useState('');
    const [idInstituicao, setIdInstituicao] = useState('');
    const [arquivo, setArquivo] = useState(null);

    const [carregando, setCarregando] = useState(true);

    async function carregarDadosIniciais() {
        try {
            setCarregando(true);
            
            const listaEventos = await eventoService.listar();
            const listarTipos = await tipoEventoService.listar();
            const listaInstituicoes = await instituicaoService.listar();

            setListaEventos(listaEventos);
            setTiposEvento(listarTipos);
            setInstituicoes(listaInstituicoes);
        } 
        catch (err) {
            console.error(err);
            console.error("Erro ao carregar dados", err);
        }
        finally{
            setCarregando(false)
        }
    }

    useEffect(() => {
        carregarDadosIniciais();
    }, []);


    async function handleCadastrar(e) {
        e.preventDefault();

        try {
            const formData = new FormData();
            formData.append('NomeEvento', nome)
            formData.append('Descricao', descricao);
            formData.append('DataEvento', data);
            formData.append('IdTipoEvento', idTipoEvento);
            formData.append('IdInstituicao', idInstituicao);
            if (arquivo) {
                formData.append('ArquivoImagem', arquivo);
            }

            await eventoService.cadastrar(formData);
            alert('Evento cadastrado com sucesso!');

            setNome('');
            setDescricao('');
            setData('');
            setIdTipoEvento('');
            setIdInstituicao('');
            setArquivo(null);

            carregarDadosIniciais();
        } 
        catch (err) {
            console.error(err);
            alert('Erro ao cadastrar o evento.')
        }
    }

    if (carregando) return <p>Carregando dados...</p>;

    return (
    <div className="cadastro-container">
      <form onSubmit={handleCadastrar}>
        <div className="campo-grupo">
          <label htmlFor="nome">Nome do Evento</label>
          <input
            id="nome"
            type="text"
            placeholder="Ex: Workshop de React"
            value={nome}
            onChange={(e) => setNome(e.target.value)}
            required
          />
        </div>

        <div className="campo-grupo">
          <label htmlFor="descricao">Descrição</label>
          <textarea
            id="descricao"
            placeholder="Descreva sobre o que será o evento..."
            value={descricao}
            onChange={(e) => setDescricao(e.target.value)}
            rows={3}
            required
          />
        </div>

         <div className="campo-grupo">
          <label htmlFor="data">Data do Evento</label>
          <input
            id="data"
            type="date"
            value={data}
            onChange={(e) => setData(e.target.value)}
            required
          />
        </div>

        {/* SELECT PARA TIPO DE EVENTO */}
        <div className="campo-grupo">
          <label>Tipo de Evento</label>
          <select 
            value={idTipoEvento} 
            onChange={(e) => setIdTipoEvento(e.target.value)}
            required
          >
            <option value="">Selecione um tipo...</option>
            {tiposEvento.map((tipo) => (
              <option key={tipo.idTipoEvento} value={tipo.idTipoEvento}>
                {tipo.titulo}
              </option>
            ))}
          </select>
        </div>
        {/* SELECT PARA INSTITUIÇÃO */}
        <div className="campo-grupo">
          <label>Instituição</label>
          <select 
            value={idInstituicao} 
            onChange={(e) => setIdInstituicao(e.target.value)}
            required
          >
            <option value="">Selecione a instituição...</option>
            {instituicoes.map((inst) => (
              <option key={inst.idInstituicao} value={inst.idInstituicao}>
                {inst.nomeFantasia}
              </option>
            ))}
          </select>
        </div>
        {/* INPUT DE IMAGEM */}
        <div className="campo-grupo">
          <label>Banner do Evento</label>
          <input 
            type="file" 
            accept="image/*"
            onChange={(e) => setArquivo(e.target.files[0])}
          />
        </div>
        <button type="submit">Cadastrar Evento</button>
      </form>
      <Link to="/eventos">← Voltar para lista</Link>
    </div>
  );
}