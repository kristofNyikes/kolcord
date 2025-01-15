import { useEffect } from 'react';
import { useNavigate } from 'react-router';
import { fetchWithTokenCheck } from '../Helpers/fetchWithTokenRefresh';
import { constants } from '../Helpers/globalConstants';

export const useValidateToken = () => {
  const navigate = useNavigate();

  useEffect(() => {
    const validateToken = async () => {
      try {
        const response = await fetchWithTokenCheck('/api/account/refresh-token-expiration', {});
        if (response.ok) {
          const data = await response.json();
          const expiration = new Date(data.expiration).getTime() / constants.secInMiliSec;
          const now = Math.floor(Date.now() / constants.secInMiliSec);

          if (expiration < now) {
            navigate('/login');
            localStorage.clear();
          }
        } else if (response.status === 401) {
          navigate('/login');
          localStorage.clear();
        }
      } catch (error) {
        console.error('Token validation failed:', error);
        navigate('/login');
        localStorage.clear();
      }
    };

    validateToken();

    const intervalId = setInterval(() => {
      validateToken();
    }, constants.tokenCheckTimeInMin);

    return () => clearInterval(intervalId);
  }, [navigate]);
};
