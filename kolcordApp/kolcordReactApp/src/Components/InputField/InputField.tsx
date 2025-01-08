type InputField = {
  inputValue : string;
  inputState: (value: string) => void;
  type: string
  children: React.ReactNode;
}

const InputField = ({ inputValue, inputState, type, children } : InputField) => {
  return (
    <div className='font-oswald flex flex-col mb-5'>
        <label className='text-xl md:text-2xl'>{children}</label>
        <input type={type} value={inputValue} onChange={(e) => inputState(e.target.value)} className='bg-black mx-4 text-xl w-60 rounded border-none outline-none py-1 px-2 md:w-96 md:h-12 md:text-2xl'/>
      </div>
  )
}

export default InputField