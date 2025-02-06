
type Props = {
  errorMessage: string | null;
};

const LoginErrorFields = ({ errorMessage }: Props) => {
  return <p>{errorMessage}</p>
};

export default LoginErrorFields;
