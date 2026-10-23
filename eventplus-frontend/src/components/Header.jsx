import { NavLink, useNavigate } from 'react-router-dom';
import { authService } from '../services/authService';
import './Header.css';

export function Header() {
  const navigate = useNavigate();
  const usuario = authService.getUsuarioLogado();

  function handleLogout() {
    authService.logout();
    navigate('/login');
  }

  return (
    <header className="header">
      <div className="header-inner">
        <NavLink to="/home" className="header-logo">
          <span className="logo-e">≡</span>VENT+
        </NavLink>
        <nav className="header-nav">
          <NavLink to="/home" className={({ isActive }) => isActive ? 'nav-link active' : 'nav-link'}>Home</NavLink>
          <NavLink to="/eventos" className={({ isActive }) => isActive ? 'nav-link active' : 'nav-link'}>Eventos</NavLink>
          <NavLink to="/usuarios" className={({ isActive }) => isActive ? 'nav-link active' : 'nav-link'}>Usuários</NavLink>
          <NavLink to="/contatos" className={({ isActive }) => isActive ? 'nav-link active' : 'nav-link'}>Contatos</NavLink>
        </nav>
        <button onClick={handleLogout} className="header-user-btn">
          <svg xmlns="http://www.w3.org/2000/svg" width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round"><path d="M20 21v-2a4 4 0 0 0-4-4H8a4 4 0 0 0-4 4v2"/><circle cx="12" cy="7" r="4"/></svg>
          <span>{usuario?.nome || 'Usuário'}</span>
        </button>
      </div>
    </header>
  );
}