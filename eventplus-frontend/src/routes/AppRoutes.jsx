import {Routes, Route} from "react-router-dom";
import {Eventos} from '../pages/Eventos';
import { Login } from "../pages/Login";
import { Home } from "../pages/Home";
import { CadastrarEvento } from "../pages/CadastrarEvento";

function AppRoutes(){
    return(
            <Routes>
                <Route path="/" element={<Login />}/>
                <Route path="/login" element={<Login />}/>
                <Route path="/home" element={<Home />}/>
                <Route path="/eventos" element={<Eventos />}/>
                <Route path="/cadastrar-evento" element={<CadastrarEvento />}/>
            </Routes>        
    );
}

export default AppRoutes;