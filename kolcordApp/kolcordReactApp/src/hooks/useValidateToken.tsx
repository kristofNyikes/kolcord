import { useEffect } from 'react';
import { useNavigate } from 'react-router';
import { fetchWithTokenCheck } from '../Helpers/fetchWithTokenRefresh';
import { constants } from '../Helpers/globalConstants';

export const useValidateToken = () => {
  const navigate = useNavigate();

  useEffect(() => {
    const validateToken = async () => {
      try {
        console.log('refresh-token before', localStorage.getItem('refreshToken'));
        const response = await fetchWithTokenCheck('/api/account/refresh-token-expiration', {});
        if (response.ok) {
          console.log('refresh-token AFTER', localStorage.getItem('refreshToken'));
          const data = await response.json();
          const expiration = new Date(data.expiration).getTime() / constants.secInMiliSec;
          const now = Math.floor(Date.now() / constants.secInMiliSec);

          if (expiration < now) {
            navigate('/login');
          }
        } else if (response.status === 401) {
          navigate('/login');
        }
      } catch (error) {
        console.log('refresh-token ERROR', localStorage.getItem('refreshToken'));
        console.error('Token validation failed:', error);
        navigate('/login');
      }
    };

    validateToken();

    const intervalId = setInterval(() => {
      validateToken();
    }, constants.tokenCheckTimeInMin);

    return () => clearInterval(intervalId);
  }, [navigate]);
};
