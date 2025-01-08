import SignUpInButton from '../Components/Buttons/SignUpInButton';

const HomePage = () => {
  return (
    <div className="flex items-center flex-col font-oswald">
      <div className="flex flex-col items-center">
        
        <h2 className='text-4xl font-oswald'>Welcome to Kolcord!</h2>
        <h3>Sign up or Log in</h3>
      </div>
      <div className="m-4">
        <SignUpInButton>
          Sign Up!
        </SignUpInButton>
        <SignUpInButton>
          Log In!
        </SignUpInButton>
      </div>
    </div>
  );
};

export default HomePage;
