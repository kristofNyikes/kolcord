import { Link } from 'react-router-dom'


const NavIcon: React.FC = () => {
  return (
    <div>
      <Link to={"/main"}>
        <img src="/main-icon.png" alt="icon with navigation to main page" className='w-8'/>
      </Link>
    </div>
  )
}

export default NavIcon