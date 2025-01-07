import { Link } from 'react-router-dom';

const HomePage = () => {
  return (
    <div className="flex items-center flex-col font-oswald">
      <div className="flex flex-col items-center">
        <div className='flex'>
          <img src="main-icon.png" alt="main icon" className="w-20" />
          <h1 className="text-7xl font-oswald content-center px-4">Kolcord</h1>
        </div>
        <h2 className='text-4xl font-oswald'>Welcome to Kolcord!</h2>
        <h3>Sign up or Log in</h3>
      </div>
      <div className="m-4">
        <Link to="/register" className="bg-red-800 text-black border-4 border-black rounded px-4 py-2 mx-4">
          Sign Up!
        </Link>
        <Link to="/login" className="bg-red-800 text-gray-100 rounded px-4 py-2 mx-4">
          Log in!
        </Link>
      </div>
    </div>
  );
};

export default HomePage;
