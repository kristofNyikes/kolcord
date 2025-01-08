import { Link } from 'react-router-dom'

type SignUpInButton  ={
  children: React.ReactNode;
}

const SignUpInButton = ({ children } : SignUpInButton) => {
  return (
    <Link to="/register" className="bg-red-800 rounded text-xl px-4 py-2 mx-4 border-4 border-transparent hover:border-black hover:text-black">
          {children}
        </Link>
  )
}

export default SignUpInButton