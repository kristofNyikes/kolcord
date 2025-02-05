import { Link } from 'react-router-dom';
import NavIcon from './NavIcon';
import { useFriendRequests } from '../../Routes/FriendRequestsContext';

const TopBar: React.FC = () => {
  const { friendRequests } = useFriendRequests();
  return (
    <div className="flex items-center p-2 gap-4 bg-stone-950/60">
      <NavIcon />
      <Link to={'friend-requests'}>
        <div className='relative'>
          <img src="friend-icon.png" alt="navigation to friend requests" className="w-9" />
          {friendRequests && friendRequests.length > 0 && (
            <span className="absolute top-0 right-0 bg-red-900 text-white text-xs rounded-full px-2 py-1">{friendRequests.length}</span>
          )}
        </div>
      </Link>
      <Link to={'search'}>
        <img src="search-icon.png" alt="search for people on the platform" className="w-7" />
      </Link>
    </div>
  );
};

export default TopBar;
