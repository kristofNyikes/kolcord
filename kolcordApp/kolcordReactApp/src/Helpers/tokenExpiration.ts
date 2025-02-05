import { jwtDecode } from 'jwt-decode';
import { constants } from './globalConstants';

export const isExpired = ():boolean => {
  const accessToken = localStorage.getItem('accessToken')
  if(!accessToken) return true;

  const {exp} = jwtDecode(accessToken!);
  const now = Math.floor(Date.now() / constants.secInMiliSec);
  return exp! < now;
}