import { useState } from 'react';
import InputField from '../Components/InputField/InputField';
import SubmitButton from '../Components/Buttons/SubmitButton';
import { Link } from 'react-router-dom';

const RegisterPage = () => {
  const [username, setUsername] = useState<string>('');
  const [email, setEmail] = useState<string>('');
  const [password, setPassword] = useState<string>('');
  return (
    <div className="flex justify-center items-center">
      <form className="bg-black bg-opacity-30 rounded-lg shadow-lg p-5 flex flex-col items-center  mt-24 md:mt-16">
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
