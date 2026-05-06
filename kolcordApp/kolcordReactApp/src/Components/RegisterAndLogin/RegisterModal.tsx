import { useContext, useState } from "react";
import InputField from "../InputField/InputField";
import SubmitButton from "../Buttons/SubmitButton";
import { useNavigate } from "react-router-dom";
import { Context } from "../Contexts/Context";
import Spinner from "../Spinner/Spinner";
import { AuthData } from "../../types/types";
import { LoginRegisterProps } from "../../types/types";
import ErrorModal from "../Error/ErrorModal";
import { registrationChecker } from "../../Helpers/authChecker";

const RegisterModal = ({ setModal }: LoginRegisterProps) => {
  const [username, setUsername] = useState<string>("");
  const [email, setEmail] = useState<string>("");
  const [password, setPassword] = useState<string>("");
  const [isLoading, setIsLoading] = useState<boolean>(false);
  const [error, setError] = useState<Response | null>(null);
  const [errorList, setErrorList] = useState<string[]>([]);
  const context = useContext(Context);
  const navigate = useNavigate();

  const errorModalTimer = () => {
    setTimeout(() => {
      setError(null);
      setErrorList([]);
    }, 5000);
  };

  if (!context) {
    throw new Error("Context must be used within a Context.Provider");
  }

  const [, setSignedIn] = context;

  const body = {
    email: email,
    password: password,
    username: username,
  };

  const options = {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(body),
  };

  const handleAuthSubmit = async (
    e: React.FormEvent<HTMLFormElement>,
  ): Promise<void> => {
    e.preventDefault();
    setIsLoading(true);

    const errorList = registrationChecker(
      body.email,
      body.password,
      body.username,
    );
    if (errorList.length > 0) {
      setErrorList(errorList);
      setIsLoading(false);
      errorModalTimer();
      return;
    }

    try {
      const baseUrl = import.meta.env.VITE_BASE_URL;
      const response = await fetch(`${baseUrl}/api/account/register`, options);

      if (response.ok) {
        const data: AuthData = await response.json();
        localStorage.setItem("userName", data.userName);
        localStorage.setItem("email", data.email);
        localStorage.setItem("accessToken", data.token);
        localStorage.setItem("refreshToken", data.refreshToken);
        localStorage.setItem("userId", data.userId);

        setSignedIn(true);
        navigate("/main");
      } else {
        const data = await response.json();
        const errorBody: string[] = [];
        errorBody[0] = data.error;
        setError(response);
        setErrorList(errorBody);
        errorModalTimer();
      }
    } catch (error) {
      console.error(error);
    } finally {
      setIsLoading(false);
    }
  };

  const handleChangeModal = () => setModal(false);

  return (
    <div className="flex flex-col justify-center items-center">
      {errorList.length > 0 && (
        <ErrorModal error={error} errorList={errorList} />
      )}
      {!isLoading ? (
        <form
          onSubmit={handleAuthSubmit}
          className="bg-black bg-opacity-30 rounded-lg shadow-lg p-5 flex flex-col items-center  mt-24 md:mt-16"
        >
          <p className="text-xl mb-5 md:text-2xl">Create an account</p>
          <InputField
            inputValue={username}
            inputState={setUsername}
            type="text"
          >
            Username:{" "}
          </InputField>
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
          <button onClick={handleChangeModal} className="hover:text-red-700">
            Already have an account? Log in
          </button>
        </form>
      ) : (
        <div className="m-40">
          <Spinner size={64} />
        </div>
      )}
    </div>
  );
};
export default RegisterModal;
