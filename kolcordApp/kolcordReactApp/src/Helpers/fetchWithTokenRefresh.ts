import { jwtDecode } from 'jwt-decode';
import { constants } from './globalConstants';

type Data = {
  accessToken: string;
  refreshToken: string;
};

const isTokenExpiringSoon = (token: string | null): boolean => {
  if (!token) return true;

  try {
    const { exp } = jwtDecode(token!);
    const now = Math.floor(Date.now() / constants.secInMiliSec);
    return exp! - now < constants.expiration;
  } catch {
    return true;
  }
};

export const fetchWithTokenCheck = async (url: string, options: RequestInit): Promise<Response> => {
  const baseUrl = import.meta.env.VITE_BASE_URL;
  let accessToken = localStorage.getItem('accessToken');

  if (isTokenExpiringSoon(accessToken)) {
    const refreshToken = localStorage.getItem('refreshToken');
    const response = await fetch(`${baseUrl}/api/account/refresh-token`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({refreshToken: refreshToken})
    });

    if (response.ok) {
      const data: Data = await response.json();
      localStorage.setItem('accessToken', data.accessToken);
      localStorage.setItem('refreshToken', data.refreshToken);
      accessToken = data.accessToken;

    } else {
      console.log("some error...", response)
      throw new Error('Session expired');
    }
  }

  options.headers = {
    ...options.headers,
    Authorization: `Bearer ${accessToken}`,
  };

  return fetch(`${baseUrl}${url}`, options);
};
