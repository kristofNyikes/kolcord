import ServerList from '../Components/ServerList/ServerList';
import TopBar from '../Components/TopBar/TopBar';
import { Outlet } from 'react-router';
import { fetchWithTokenCheck } from '../Helpers/fetchWithTokenRefresh';
import { useEffect, useState } from 'react';
import { Requests } from '../types/types';
import { FriendRequestsProvider } from '../Components/Contexts/FriendRequestsProvider';

const MainPage: React.FC = () => {
  const [friendRequests, setFriendRequests] = useState<Requests[] | null>(null);

  useEffect(() => {
    const fetchFriends = async () => {
      const response = await fetchWithTokenCheck('/api/friendship/friend-requests', {});
      if (response.ok) {
        const data: Requests[] = await response.json();
        setFriendRequests(data);
      }
    };
    fetchFriends();
  }, []);

  return (
    <FriendRequestsProvider>
      <div className="flex flex-col h-screen">
        <div className="fixed top-0 left-0 right-0 z-50">
          <TopBar />
        </div>

        <div className="flex flex-row pt-14 h-full">
          {' '}
          <div className="w-16 flex-none">
            <ServerList />
          </div>
          <div className="flex-grow bg-black/35 overflow-auto">
            <Outlet context={friendRequests} />
          </div>
        </div>
      </div>
    </FriendRequestsProvider>
  );
};

export default MainPage;
