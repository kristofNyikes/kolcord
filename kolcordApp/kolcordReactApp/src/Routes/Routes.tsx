import { createBrowserRouter } from 'react-router';
import App from '../App';
import HomePage from '../Pages/HomePage';
import LoginPage from '../Pages/LoginPage';
import RegisterPage from '../Pages/RegisterPage';
import MainPage from '../Pages/MainPage';
import ProtectedRoute from './ProtectedRoute';
import FriendList from '../Components/Friends/FriendList';
import FriendRequests from '../Components/Friends/FriendRequests';
import Search from '../Components/Search/Search';

export const router = createBrowserRouter([
  {
    path: '/',
    element: <App />,
    children: [
      { path: '', element: <HomePage /> },
      { path: 'login', element: <LoginPage /> },
      { path: 'register', element: <RegisterPage /> },
      {
        path: 'main',
        element: (
          <ProtectedRoute>
            <MainPage />
          </ProtectedRoute>
        ),
        children: [
          {path: '', element: <FriendList/>},
          {path: 'friend-requests', element: <FriendRequests/>},
          {path: 'search', element: <Search/>}
        ],
      },
    ],
  },
]);
