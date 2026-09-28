import { useState } from "react";
import { useNavigate } from "react-router-dom";
import {authService} from '../services/authService';
import './Login.css';

export function Login() {
    const [email, setEmail] = useState('');
    const [senha, setSenha] = useState('');

    const [carregando, setCarregando] = useState(false);
    const [erro, setErro] = useState(null);

    const navigate = useNavigate();

    async function handleSubmit(e) {
        e.preventDefault();

        try {
            setCarregando(true);
            setErro(null);
            
            await authService.login(email, senha);

            navigate('/eventos');
        } 
        catch (err) {
            console.error(err);
            setErro('Email ou senha incorretos. Tente novamente.');
        }
        finally{
            setCarregando(false);
        }
    }

    return(
        <div className="login-container">
            <div className="login-card">
                <h1 className="login-titulo">Entrar no EventPlus</h1>
                {erro && <div className="login-alerta-erro">{erro}</div>}

                <form onSubmit={handleSubmit} className="login-form">
                    <div className="campo-grupo">
                        <label htmlFor="email">E-mail</label>
                        <input 
                        id="email"
                        type="email"
                        placeholder="seuemail@dominio.com"
                        value={email}
                        onChange={(e) => setEmail(e.target.value)}
                        required
                        />
                    </div>

                    <div className="campo-grupo">
                        <label htmlFor="senha">Senha</label>
                        <input 
                        id="senha"
                        type="password"
                        placeholder="Digite sua senha"
                        value={senha}
                        onChange={(e) => setSenha(e.target.value)}
                        required
                        />
                    </div>
                    <button type="submit" className="btn-entrar" disabled={carregando}>{carregando ? 'Entrando' : 'Entrar'}</button>
                </form>
            </div>
        </div>
    );
}