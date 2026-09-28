import { BrowserRouter, Routes, Route } from "react-router-dom";
import {Eventos} from '../pages/Eventos';
import { Login } from "../pages/Login";
import { CadastrarEvento } from "../pages/CadastrarEvento";

function AppRoutes(){
    return(
        <BrowserRouter>
            <Routes>
                <Route path="/" element={<Login />}/>
                <Route path="/login" element={<Login />}/>
                <Route path="/eventos" element={<Eventos />}/>
                <Route path="/cadastrar-evento" element={<CadastrarEvento />}/>
            </Routes>        
        </BrowserRouter>
    );
}

export default AppRoutes;