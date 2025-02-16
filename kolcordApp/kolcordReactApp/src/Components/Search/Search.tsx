import { useState } from 'react';
import InputField from '../InputField/InputField';
import Spinner from '../Spinner/Spinner';
import SearchResultItem from './SearchResultItem';
import { fetchWithTokenCheck } from '../../Helpers/fetchWithTokenRefresh';
import { UserWithFrStatus } from '../../types/types';
import SearchButton from './SearchButton';

const Search = () => {
  const [search, setSearch] = useState<string>('');
  const [isLoading, setIsLoading] = useState<boolean>(false);
  const [searchResult, setSearchResult] = useState<UserWithFrStatus[] | null>(null);

  const onSearch = async () => {
    setIsLoading(true);
    try {
      const response = await fetchWithTokenCheck(`/api/user/search-users/${search}`, {});
      if (response.ok) {
        const data = await response.json();
        setSearchResult(data.sort((u: UserWithFrStatus) => u.isFriend));
      }
    } catch (error) {
      console.error(error);
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div className="flex flex-col items-center pt-12">
      <InputField inputValue={search} type="text" inputState={setSearch}>
        Search for users:
      </InputField>
      <SearchButton onSearch={onSearch} />
      <div className='mt-4 w-full max-w-lg h-96 overflow-y-auto'>
        {searchResult && searchResult.length > 0 ? (
          searchResult.map((res) => {
            return <SearchResultItem key={res.id} result={res} />;
          })
        ) : isLoading ? (
          <Spinner />
        ) : searchResult?.length === 0 ? (
          <p>No result</p>
        ) : (
          ''
        )}
      </div>
    </div>
  );
};

export default Search;
