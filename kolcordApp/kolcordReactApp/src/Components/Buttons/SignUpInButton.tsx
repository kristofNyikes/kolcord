import { Link } from 'react-router-dom'

type SignUpInButton  ={
  children: React.ReactNode;
  route: string;
}

const SignUpInButton = ({ children, route } : SignUpInButton) => {
  return (
    <Link to={`/${route}`} className="bg-red-800 rounded text-xl px-4 py-2 mx-4 border-4 border-transparent md:text-2xl hover:border-black hover:text-black">
          {children}
        </Link>
  )
}

export default SignUpInButton