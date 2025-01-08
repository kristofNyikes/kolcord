import SignUpInButton from '../Components/Buttons/SignUpInButton';

const HomePage = () => {
  return (
    <div className="flex items-center flex-col font-oswald">
      <div className="flex flex-col items-center">
        <h2 className='text-4xl font-oswald mt-12 md:text-7xl'>Welcome to Kolcord!</h2>
        <h3 className='text-2xl m-10 md:text-4xl'>Wanna join this fantastick app? Just sign up so you can be a part of a fantastic experience</h3>
        <p className='text-lg md:text-xl'>Already signed up? Just hit the Log In button!</p>
      </div>
      <div className="m-4">
        <SignUpInButton route='register'>
          Sign Up!
        </SignUpInButton>
        <SignUpInButton route='login'>
          Log In!
        </SignUpInButton>
      </div>
    </div>
  );
};

export default HomePage;
