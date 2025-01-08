import { useState } from 'react';
import InputField from '../Components/InputField/InputField';
import SubmitButton from '../Components/Buttons/SubmitButton';

const LoginPage = () => {
  const [email, setEmail] = useState<string>('');
  const [password, setPassword] = useState<string>('');
  return (
    <div className='flex flex-col justify-center items-center'>
      <form className="bg-black bg-opacity-30 rounded-lg shadow-lg p-5 flex flex-col items-center  mt-24 md:mt-16">
      <p className='text-xl mb-5 md:text-2xl'>Welcome back we missed you!</p>
        <InputField inputValue={email} inputState={setEmail} type="email">
          Email:{' '}
        </InputField>
        <InputField inputValue={password} inputState={setPassword} type="password">
          Pasword:{' '}
        </InputField>
        <SubmitButton>Submit</SubmitButton>
      </form>
    </div>
  );
};

export default LoginPage;
