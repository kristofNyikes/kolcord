import { Link } from 'react-router-dom';

const Logo  = () => {

  return (
    <Link to={"/"} className='flex justify-center md:justify-start md:p-5'>
          <img src="main-icon.png" alt="main icon" className="w-20" />
          <h1 className="text-7xl font-oswald content-center px-4 text-black">Kolcord</h1>
        </Link>
  )
}

export default Logo