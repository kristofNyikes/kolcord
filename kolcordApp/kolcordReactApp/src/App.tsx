import { Outlet } from 'react-router';
import Logo from './Components/Logo/Logo';

function App() {
  return (
    <div className='bg-gradient-to-b from-red-950 to-stone-950 min-h-screen text-white'>
      <Logo/>
      <Outlet />
    </div>
  );
}

export default App;
