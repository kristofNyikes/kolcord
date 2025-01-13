import { jwtDecode } from 'jwt-decode';

type Data = {
  accessToken: string;
  refreshToken: string;
  userName: string;
  email: string;
};

const isTokenExpiringSoon = (token: string | null): boolean => {
  const { exp } = jwtDecode(token!);
  const now = Math.floor(Date.now() / 1000);
  return exp! - now < 300;
};

export const fetchWithTokenCheck = async (url: string, options: any): Promise<Response> => {
  const baseUrl: string = import.meta.env.BASE_URL;
  let accessToken: string | null = localStorage.getItem('accessToken');

  if (isTokenExpiringSoon(accessToken)) {
    const refreshToken: string | null = localStorage.getItem('refreshToken');
    const response: Response = await fetch(`${baseUrl}/api/account/refresh-token`, {
      method: 'POST',
      headers: { 'Content-Type': 'applicatin/json' },
      body: JSON.stringify({ refreshToken }),
    });

    if (response.ok) {
      const data: Data = await response.json();
      localStorage.setItem('accessToken', data.accessToken);
      localStorage.setItem('refreshToken', data.refreshToken);
      accessToken = data.accessToken;
    } else {
      throw new Error('Session expired. Please log in again.');
    }
  }

  options.headers = {
    ...options,
    Authorization: `Bearer: ${accessToken}`,
  };

  return fetch(`${baseUrl}${url}`, options);
};
