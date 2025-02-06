import LoginErrorFields from './LoginErrorFields';

type Props = {
  message: string;
  errorArr: string[] | null;
};

const LoginError = ({ message, errorArr }: Props) => {
  return (
    <>
      {errorArr ? (
        errorArr.map((err, i) => {
          return <LoginErrorFields errorMessage={err} key={i}/>;
        })
      ) : (
        <LoginErrorFields errorMessage={message}/>
      )}
    </>
  );
};

export default LoginError;
