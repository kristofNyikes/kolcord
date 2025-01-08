import { Outlet } from 'react-router';
import Logo from './Components/Logo/Logo';

function App() {
  return (
    <div className='bg-gradient-to-t from-red-950 via-stone-950 to-red-950 min-h-screen text-white font-oswald pt-3'>
      <Logo/>
      <Outlet />
    </div>
  );
}

export default App;
