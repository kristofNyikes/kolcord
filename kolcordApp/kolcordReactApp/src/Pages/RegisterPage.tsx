import { useState } from 'react';
import InputField from '../Components/InputField/InputField';
import SubmitButton from '../Components/Buttons/SubmitButton';
import { Link, useNavigate } from 'react-router-dom';

type Data = {
  userName: string;
  email: string;
  token: string;
  refreshToken: string;
}

const RegisterPage = () => {
  const [username, setUsername] = useState<string>('');
  const [email, setEmail] = useState<string>('');
  const [password, setPassword] = useState<string>('');
  const navigate = useNavigate();

  const body = {
    email: email,
    password: password,
    username: username
  };

  const options = {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify(body)
  }

  const handleAuthSubmit = async (e: React.FormEvent<HTMLFormElement>): Promise<void> => {
    e.preventDefault();
    try {
      const baseUrl = import.meta.env.VITE_BASE_URL;
      console.log( baseUrl)
      const response = await fetch(`${baseUrl}/api/account/register`, options)
      if(response.ok) {
        const data: Data = await response.json();
        localStorage.setItem('userName', data.userName);
        localStorage.setItem('email', data.email);
        localStorage.setItem('accessToken', data.token);
        localStorage.setItem('refreshToken', data.refreshToken);
        navigate("/main")
      }
    } catch (error) {
      console.error(error);
    }
  };

  return (
    <div className="flex justify-center items-center">
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
    </div>
  );
};
export default RegisterPage;
