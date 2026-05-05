type SubmitButton = {
  children: React.ReactNode;
}

const SubmitButton = ({ children } : SubmitButton) => {
  return (
    <button type='submit' className='font-oswald text-2xl border-4 border-transparent rounded-lg bg-red-900 px-10 py-2 hover:bg-red-950'>{children}</button>
  )
}

export default SubmitButton