type InputField = {
  inputValue : string;
  inputState: (value: string) => void;
  type: string
  children: React.ReactNode;
}

const InputField = ({ inputValue, inputState, type, children } : InputField) => {
  return (
    <div>
        <label>{children}</label>
        <input type={type} value={inputValue} onChange={(e) => inputState(e.target.value)} className='bg-black mx-4'/>
      </div>
  )
}

export default InputField