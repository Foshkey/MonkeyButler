using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace MonkeyButler.Business.Engines
{
    internal class EmotesEngine : IEmotesEngine
    {
        private readonly Regex _emotesRegex = new Regex(
            @"(<:\w+:\d+>|\u00a9|\u00ae|[\u2000-\u3300]|\ud83c[\ud000-\udfff]|\ud83d[\ud000-\udfff]|\ud83e[\ud000-\udfff])",
            RegexOptions.Compiled);

        public List<string> Split(string emotes)
        {
            var matches = _emotesRegex.Matches(emotes);

            return matches.Select(match => match.Value).ToList();
        }
    }

    internal interface IEmotesEngine
    {
        List<string> Split(string emotes);
    }
}
