import { createContext } from 'react';

export const Context = createContext<[boolean, React.Dispatch<React.SetStateAction<boolean>>]>([false, () => {}]);