using System.Collections.Generic;

namespace MonkeyButler.Business.Models.CharacterSearch
{
    public class CharacterSearchResult
    {
        public IAsyncEnumerable<Character>? Characters { get; set; }
    }
}
