import { useContext, useState } from 'react';
import InputField from '../Components/InputField/InputField';
import SubmitButton from '../Components/Buttons/SubmitButton';
import { Link, useNavigate } from 'react-router-dom';
import { Context } from '../Components/Contexts/Context';
import Spinner from '../Components/Spinner/Spinner';
import { AuthData } from '../types/types';


const RegisterPage = () => {
  const [username, setUsername] = useState<string>('');
  const [email, setEmail] = useState<string>('');
  const [password, setPassword] = useState<string>('');
  const [isLoading, setIsLoading] = useState<boolean>(false);
  const context = useContext(Context);
  const navigate = useNavigate();

  if (!context) {
    throw new Error('Context must be used within a Context.Provider');
  }

  const [, setSignedIn] = context;

  const body = {
    email: email,
    password: password,
    username: username,
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
    setIsLoading(false);

    try {
      const baseUrl = import.meta.env.VITE_BASE_URL;
      const response = await fetch(`${baseUrl}/api/account/register`, options);

      if (response.ok) {
        const data: AuthData = await response.json();
        localStorage.setItem('userName', data.userName);
        localStorage.setItem('email', data.email);
        localStorage.setItem('accessToken', data.accessToken);
        localStorage.setItem('refreshToken', data.refreshToken);

        setSignedIn(true);
        navigate('/main');
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
        <form onSubmit={handleAuthSubmit} className="bg-black bg-opacity-30 rounded-lg shadow-lg p-5 flex flex-col items-center  mt-24 md:mt-16">
          <p className="text-xl mb-5 md:text-2xl">Create an account</p>
          <InputField inputValue={username} inputState={setUsername} type="text">
            Username:{' '}
          </InputField>
          <InputField inputValue={email} inputState={setEmail} type="email">
            Email:{' '}
          </InputField>
          <InputField inputValue={password} inputState={setPassword} type="password">
            Pasword:{' '}
          </InputField>
          <SubmitButton>Submit</SubmitButton>
          <Link to={'/login'} className="hover:text-red-700">
            Already have an account?
          </Link>
        </form>
      ) : (
        <div className="m-40">
          <Spinner size={64} />
        </div>
      )}
    </div>
  );
};
export default RegisterPage;
