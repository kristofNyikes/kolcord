import RegisterModal from "../Components/RegisterAndLogin/RegisterModal";
import LoginModal from "../Components/RegisterAndLogin/LoginModal";
import { useState } from "react";

const HomePage = () => {
  const [isOnRegister, setIsOnRegister] = useState<boolean>(true);
  return (
    <div className="flex items-center flex-col font-oswald">
      <div className="flex flex-col items-center">
        <h2 className="text-2xl font-oswald mt-12 md:text-5xl">
          Welcome to Kolcord!
        </h2>
        <h3 className="text-2xl m-5 md:text-2xl">
          Wanna join this fantastick app? Just sign up so you can be a part of a
          fantastic experience
        </h3>
      </div>

      {isOnRegister ? (
        <RegisterModal setModal={setIsOnRegister} />
      ) : (
        <LoginModal setModal={setIsOnRegister} />
      )}
    </div>
  );
};

export default HomePage;
