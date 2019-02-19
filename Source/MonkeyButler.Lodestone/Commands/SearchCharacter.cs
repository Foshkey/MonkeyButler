using System;
using System.Threading.Tasks;

namespace MonkeyButler.XivApi.Commands
{
    internal class SearchCharacter : ISearchCharacter
    {
        public Task<SearchCharacterResponse> Process(SearchCharacterCriteria criteria)
        {
            throw new NotImplementedException();
        }
    }
}
