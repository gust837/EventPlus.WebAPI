import { useState, useEffect } from "react";
import { Link } from "react-router-dom";
import { eventoService } from "../services/eventoService";
import './Home.css';

export function Home() {
    const [proximosEventos, setProximosEventos] = useState([]);
    const [carregando, setCarregando] = useState(true);

    useEffect(() => {
        async function carregarProximos(param) {
            try {
                setCarregando(true);
                const dados = await eventoService.listar();

                setProximosEventos(dados.slice(0, 3));
            } catch (err) {
                console.error("Erro ao carregar eventos da home", err);
            } finally {
                setCarregando(false);
            }
        }

        carregarProximos();
    }, []);

    return (
        <div className="home-pagina">
            <section className="home-hero">
                <div className="home-hero-conteudo">
                    <span className="hero-tag">Destaque 2026</span>
                    <h1 className="hero-titulo">NextGen Data Forum 2026</h1>
                    <p className="hero-descricao">Dados, cibersegurança e inteligencia artificial para novos negocios.</p>
                    <div className="hero-info-data">03 a 05 de Dezembro Belo Horizonte, MG</div>
                    <Link to="/eventos" className="hero-btn-conhecer">Ver todos os Eventos</Link>
                </div>

                <div className="hero-pontos">
                    <span className="ponto ativo"></span>
                    <span className="ponto"></span>
                    <span className="ponto"></span>
                </div>
            </section>

            <section className="home-visao">
                <div className="visao-container">
                    <div className="visao-ilustracao">
                        <div className="visao-circulo-grafico">
                            👥
                        </div>
                    </div>
                    <div className="visao-texto">
                        <h2>Visao</h2>
                        <p>A EventPlus organiza eventos e informações em uma interface direta,
                            com foco em facilitar a consulta da agenda, o cadastro e a navegação
                            entre as telas principais.
                        </p>
                    </div>
                </div>
            </section>

            <section className="home-proximos">
                <div className="proximos-container">
                    <h2 className="proximos-titulo-secao">PROXIMOS EVENTOS</h2>

                    {carregando ? (
                        <p className="home-carregando">Carregando eventos...</p>
                    ) : proximosEventos.length === 0 ? (
                        <p className="home-vazio">Nenhum evento agendado no momento.</p>
                    ) : (
                        <div className="proximos-grid">{proximosEventos.map((evento) => (
                            <div key={evento.idEvento} className="proximo-card">
                                <div className="proximo-card-img">{evento.imagemUrl ? (
                                    <img src={evento.imagemUrl} alt={evento.nomeEvento} />
                                    ) : ( <div className="card-sem-foto">Sem imagem</div>
                                    )}
                                    <span className="proximo-card-categoria">
                                        {evento.idTipoEventoNavigation?.titulo || 'Tecnologia'}
                                    </span>
                                </div>

                                <div className="proximo-card-corpo">
                                    <h3>{evento.nomeEvento}</h3>
                                    <p>{evento.descricao}</p>
                                    <Link to="/eventos" className="proximo-card-link">Ver evento</Link>
                                </div>
                            </div>
                        ))}
                        </div>
                    )}
                </div>
            </section>

            <section className="home-contato">
                <div className="contato-container">
                    <div className="contato-info">
                        <h2>CONTATO</h2>
                        <p>Rua Niteroi, 180 - Centro</p>
                        <p>São Caetano do Sul - SP</p>
                        <a href="tel:1142252000" className="contato-telefone">(11) 4225-2000</a>
                    </div>

                    <div className="contato-mapa-box">
                        <div className="mapa-pin">📍</div>
                        <span>Senai São Caetano do Sul</span>
                    </div>
                </div>

                <footer className="home-rodape">
                    Escola Senai de Informatica - 2026
                </footer>
            </section>
        </div>
    );
}