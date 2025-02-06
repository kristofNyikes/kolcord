import InputField from '../Components/InputField/InputField';
import SubmitButton from '../Components/Buttons/SubmitButton';
import { useNavigate } from 'react-router';
import { useContext, useState } from 'react';
import { Context } from '../Components/Contexts/Context';
import Spinner from '../Components/Spinner/Spinner';
import { Data } from '../types/types';


const LoginPage = () => {
  const [email, setEmail] = useState<string>('');
  const [password, setPassword] = useState<string>('');
  const [isLoading, setIsLoading] = useState<boolean>(false);
  const context = useContext(Context);

  if (!context) {
    throw new Error('Context must be used within a Context.Provider');
  }

  const [, setSignedIn] = context;
  const navigate = useNavigate();

  const body = {
    email: email,
    password: password,
  };

  const options = {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify(body),
  };

  const handleAuthSubmit = async (e: React.FormEvent<HTMLFormElement>): Promise<void> => {
    e.preventDefault();
    setIsLoading(true);

    try {
      const baseUrl = import.meta.env.VITE_BASE_URL;
      const response = await fetch(`${baseUrl}/api/account/login`, options);

      if (response.ok) {
        const data: Data = await response.json();
        localStorage.setItem('userName', data.userName);
        localStorage.setItem('email', data.email);
        localStorage.setItem('accessToken', data.token);
        localStorage.setItem('refreshToken', data.refreshToken);
        
        setSignedIn(true);
        navigate("/main")
      }
    } catch (error) {
      console.error(error);
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div className="flex flex-col justify-center items-center">
      {!isLoading ? (
        <div>
          <form onSubmit={handleAuthSubmit} className="bg-black bg-opacity-30 rounded-lg shadow-lg p-5 flex flex-col items-center  mt-24 md:mt-16">
            <p className="text-xl mb-5 md:text-2xl">Welcome back we missed you!</p>
            <InputField inputValue={email} inputState={setEmail} type="email">
              Email:{' '}
            </InputField>
            <InputField inputValue={password} inputState={setPassword} type="password">
              Pasword:{' '}
            </InputField>
            <SubmitButton>Submit</SubmitButton>
          </form>
        </div>
      ) : (
        <div className='m-40'>
          <Spinner size={64}/>
        </div>
      )}
    </div>
  );
};

export default LoginPage;
