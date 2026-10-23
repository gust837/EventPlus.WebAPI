import { BrowserRouter } from 'react-router-dom';
import AppLayout from './routes/AppLayout';

function App() {
  return (
    <BrowserRouter>
      <AppLayout />
    </BrowserRouter>
  );
}

export default App;