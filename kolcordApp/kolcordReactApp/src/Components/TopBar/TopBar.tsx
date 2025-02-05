import { Link } from 'react-router-dom';
import NavIcon from './NavIcon';

const TopBar: React.FC = () => {
  return (
    <div className="flex items-center p-2 gap-4 bg-stone-950/60">
      <NavIcon />
      <Link to={'friend-requests'}>
        <img src="friend-icon.png" alt="navigation to friend requests" className="w-8" />
      </Link>
      <Link to={'search'}>
        <img src="search-icon.png" alt="search for people on the platform" className="w-7" />
      </Link>
    </div>
  );
};

export default TopBar;
