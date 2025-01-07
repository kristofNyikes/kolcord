import { Outlet } from 'react-router';

function App() {
  return (
    <div className='bg-gradient-to-b from-red-950 to-stone-950 min-h-screen'>
      <Outlet />
    </div>
  );
}

export default App;
