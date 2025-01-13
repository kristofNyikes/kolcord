import React from 'react';
import { useValidateToken } from '../hooks/useValidateToken';


type Props = {
  children: React.ReactNode;
}

const ProtectedRoute: React.FC<Props> = ({children}: Props) => {
  useValidateToken();

  return children
}

export default ProtectedRoute