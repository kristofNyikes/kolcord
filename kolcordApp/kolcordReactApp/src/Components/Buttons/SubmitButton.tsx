type SubmitButton = {
  children: React.ReactNode;
}

const SubmitButton = ({ children } : SubmitButton) => {
  return (
    <button type='submit' className='font-oswald text-2xl border-4 border-transparent rounded bg-red-800 px-5 py-2 hover:border-black hover:text-black'>{children}</button>
  )
}

export default SubmitButton