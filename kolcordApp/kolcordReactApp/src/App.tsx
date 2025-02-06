import { Context } from './Components/Contexts/Context';
import { Outlet, useNavigate } from 'react-router';
import Logo from './Components/Logo/Logo';
import { useEffect, useState } from 'react';

function App() {
  const [signedIn, setSignedIn] = useState<boolean>(() => {
    return !!localStorage.getItem('accessToken');
  });
  const navigate = useNavigate();
  useEffect(() => {
    if (signedIn) {
      const token = localStorage.getItem('accessToken');
      if (!token) {
        setSignedIn(false);
        localStorage.clear();
        navigate('/');
      } else {
        navigate('/main');
      }
    } else {
      navigate('/');
    }
  }, [signedIn, navigate]);

  const route = '/';

  return (
    <Context.Provider value={[signedIn, setSignedIn]}>
      <div className="bg-gradient-to-t from-red-950 via-stone-950 to-red-950 min-h-screen text-white font-oswald">
        {!signedIn && <Logo route={route} />}
        <Outlet />
      </div>
    </Context.Provider>
  );
}

export default App;
