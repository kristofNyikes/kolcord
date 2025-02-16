import React from 'react';
import { useValidateToken } from '../hooks/useValidateToken';
import { ProtectedRouteProps } from '../types/types';



const ProtectedRoute: React.FC<ProtectedRouteProps> = ({children}) => {
  useValidateToken();

  return children
}

export default ProtectedRoute