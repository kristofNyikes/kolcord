import { Link } from 'react-router-dom';
import { LogoProp } from '../../types/types';



const Logo: React.FC<LogoProp>  = ({route}) => {

  return (
    <Link to={route} className='flex justify-center pt-3 md:justify-start md:p-5'>
          <img src="main-icon.png" alt="main icon" className="w-20" />
          <h1 className="text-7xl font-oswald content-center px-4 text-black">Kolcord</h1>
        </Link>
  )
}

export default Logo