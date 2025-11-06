import InputField from "../InputField/InputField";
import SubmitButton from "../Buttons/SubmitButton";
import { useNavigate } from "react-router";
import { useContext, useState } from "react";
import { Context } from "../Contexts/Context";
import Spinner from "../Spinner/Spinner";
import { Data } from "../../types/types";
import { LoginRegisterProps } from "../../types/types";

const LoginModal = ({ setModal }: LoginRegisterProps) => {
  const [email, setEmail] = useState<string>("");
  const [password, setPassword] = useState<string>("");
  const [isLoading, setIsLoading] = useState<boolean>(false);
  const context = useContext(Context);

  if (!context) {
    throw new Error("Context must be used within a Context.Provider");
  }

  const [, setSignedIn] = context;
  const navigate = useNavigate();

  const body = {
    email: email,
    password: password,
  };

  const options = {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(body),
  };

  const handleAuthSubmit = async (
    e: React.FormEvent<HTMLFormElement>
  ): Promise<void> => {
    e.preventDefault();
    setIsLoading(true);

    try {
      const baseUrl = import.meta.env.VITE_BASE_URL;
      const response = await fetch(`${baseUrl}/api/account/login`, options);

      if (response.ok) {
        const data: Data = await response.json();
        localStorage.setItem("userName", data.userName);
        localStorage.setItem("email", data.email);
        localStorage.setItem("accessToken", data.token);
        localStorage.setItem("refreshToken", data.refreshToken);
        localStorage.setItem("userId", data.userId);

        setSignedIn(true);
        navigate("/main");
      }
    } catch (error) {
      console.error(error);
    } finally {
      setIsLoading(false);
    }
  };

  const handleChangeModal = () => setModal(true);

  return (
    <div className="flex flex-col justify-center items-center">
      {!isLoading ? (
        <div>
          <form
            onSubmit={handleAuthSubmit}
            className="bg-black bg-opacity-30 rounded-lg shadow-lg p-5 flex flex-col items-center  mt-24 md:mt-16"
          >
            <p className="text-xl mb-5 md:text-2xl">
              Welcome back we missed you!
            </p>
            <InputField inputValue={email} inputState={setEmail} type="email">
              Email:{" "}
            </InputField>
            <InputField
              inputValue={password}
              inputState={setPassword}
              type="password"
            >
              Pasword:{" "}
            </InputField>
            <SubmitButton>Submit</SubmitButton>
            <button onClick={handleChangeModal} className="hover:text-red-600">
              Don't have an account yet? Sign up
            </button>
          </form>
        </div>
      ) : (
        <div className="m-40">
          <Spinner size={64} />
        </div>
      )}
    </div>
  );
};

export default LoginModal;
