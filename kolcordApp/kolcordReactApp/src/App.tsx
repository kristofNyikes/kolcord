import { Context } from './Components/Contexts/Context';
import { Outlet } from 'react-router';
import Logo from './Components/Logo/Logo';
import { useEffect, useState } from 'react';

function App() {
  const [signedIn, setSignedIn] = useState<boolean>(() => {
    return !!localStorage.getItem('accessToken');
  });

  useEffect(() => {
    if(signedIn) {
      const token = localStorage.getItem('accessToken');
      if(!token) setSignedIn(false);
    }
  }, [])

  const route = signedIn ? '/main' : '/';

  return (
    <Context.Provider value={[signedIn, setSignedIn]}>
      <div className="bg-gradient-to-t from-red-950 via-stone-950 to-red-950 min-h-screen text-white font-oswald pt-3">
        <Logo route={route} />
        <div>{signedIn ? <p>signed in</p> : <p>not signed in</p>}</div>
        <Outlet />
      </div>
    </Context.Provider>
  );
}

export default App;