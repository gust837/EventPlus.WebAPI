import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { authService } from '../services/authService';
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
      navigate('/home');
    } catch (err) {
      console.error(err);
      setErro('E-mail ou senha incorretos. Tente novamente.');
    } finally {
      setCarregando(false);
    }
  }

  return (
    <div className="login-tela">
      <div className="login-container">
        
        {/* Logo estilizado conforme o Figma */}
        <div className="login-logo">
          <span className="logo-linhas">≡</span>VENT+
        </div>
        {/* Mensagem de erro caso o login falhe */}
        {erro && <p className="login-alerta">{erro}</p>}
        <form onSubmit={handleSubmit} className="login-formulario">
          
          {/* Input de Login / Email com linha inferior */}
          <div className="login-campo">
            <input
              type="email"
              placeholder="Login"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              required
              className="login-input-linha"
            />
          </div>
          {/* Input de Senha com linha inferior */}
          <div className="login-campo">
            <input
              type="password"
              placeholder="Senha"
              value={senha}
              onChange={(e) => setSenha(e.target.value)}
              required
              className="login-input-linha"
            />
          </div>
          {/* Texto "Esqueceu a senha?" */}
          <a href="#esqueceu" className="login-esqueceu-senha">
            Esqueceu a senha?
          </a>
          {/* Botão de Entrar */}
          <button 
            type="submit" 
            className="login-btn-entrar" 
            disabled={carregando}
          >
            {carregando ? 'Entrando...' : 'Entrar'}
          </button>
        </form>
      </div>
    </div>
  );
}