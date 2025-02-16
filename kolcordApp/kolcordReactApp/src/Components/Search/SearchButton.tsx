import React from 'react'
import { SearchButtonProp } from '../../types/types'

const SearchButton: React.FC<SearchButtonProp> = ({onSearch}) => {
  return (
    <button onClick={(onSearch)} className='bg-red-900 rounded text-xl px-4 py-2 mx-4 border-4 border-transparent md:text-2xl hover:border-black hover:text-black'>
      Search
    </button>
  )
}

export default SearchButton