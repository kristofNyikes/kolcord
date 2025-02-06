import { Link, useNavigate } from 'react-router-dom';
import NavIcon from './NavIcon';
import { useFriendRequests } from '../../Routes/FriendRequestsContext';
import { fetchWithTokenCheck } from '../../Helpers/fetchWithTokenRefresh';
import { useContext } from 'react';
import { Context } from '../Contexts/Context';

const TopBar: React.FC = () => {
  const { friendRequests } = useFriendRequests();
  const [signedIn, setSignedIn] = useContext(Context);
  const navigate = useNavigate();

  const options = {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
  }

  const onLogout = async () => {
    const response = await fetchWithTokenCheck('/api/account/logout', options);
    if(response.ok) {
      setSignedIn(false);
      localStorage.clear();
      navigate('/');
    }
  }
  return (
    <div className="flex items-center p-2 bg-stone-950/60 justify-between">
      <div className='flex gap-4'>
        <NavIcon />
        <Link to={'friend-requests'}>
          <div className="relative">
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
      <div>
        <button onClick={onLogout}>
          <img src="/logout-icon.png" alt="logout button" className='w-9'/>
        </button>
      </div>
    </div>
  );
};

export default TopBar;
