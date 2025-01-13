import { createBrowserRouter } from 'react-router';
import App from '../App';
import HomePage from '../Pages/HomePage';
import LoginPage from '../Pages/LoginPage';
import RegisterPage from '../Pages/RegisterPage';
import MainPage from '../Pages/MainPage';
import ProtectedRoute from './ProtectedRoute';

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
        children: [],
      },
    ],
  },
]);
