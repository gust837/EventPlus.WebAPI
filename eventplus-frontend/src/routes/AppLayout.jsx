import { useLocation } from "react-router-dom";
import { Header } from "../components/Header";
import { authService } from "../services/authService";
import AppRoutes from './AppRoutes';

function AppLayout(){
    const location = useLocation();
    const estaLogado = authService.estaAutenticado();

    const rotasSemHeader = ['/', '/login'];
    const exibirHeader = estaLogado && !rotasSemHeader.includes(location.pathname);

    return (
        <>
            {exibirHeader && <Header />}
            <main>
                <AppRoutes />
            </main>
        </>
   );
}

export default AppLayout;