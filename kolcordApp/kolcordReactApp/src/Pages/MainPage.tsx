import ServerList from '../Components/ServerList/ServerList';
import TopBar from '../Components/TopBar/TopBar';
import { Outlet } from 'react-router';
import { fetchWithTokenCheck } from '../Helpers/fetchWithTokenRefresh';
import { useEffect, useState } from 'react';
import { Requests } from '../types/types';
import { FriendRequestsProvider } from '../Routes/FriendRequestsContext';

const MainPage = () => {
  const [friendRequests, setFriendRequests] = useState<Requests[] | null>(null);

  useEffect(() => {
    const fetchFriends = async () => {
      const response = await fetchWithTokenCheck('/api/friendship/friend-requests', {});
      if (response.ok) {
        const data = await response.json();
        setFriendRequests(data);
      }
    };
    fetchFriends();
  }, []);

  return (
    <FriendRequestsProvider>
      <div>
        <TopBar />
        <div className="flex flex-row overflow-hidden">
          <div className="w-16 flex-none overflow-y-auto">
            <ServerList />
          </div>
          <div className="flex-grow bg-black/35 h-screen w-screen">
            <Outlet context={friendRequests} />
          </div>
        </div>
      </div>
    </FriendRequestsProvider>
  );
};

export default MainPage;
