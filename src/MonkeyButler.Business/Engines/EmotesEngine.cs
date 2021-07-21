using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace MonkeyButler.Business.Engines
{
    internal static class EmotesEngine
    {
        private static readonly Regex _emotesRegex = new Regex(
            @"(<:\w+:\d+>|\u00a9|\u00ae|[\u2000-\u3300]|\ud83c[\ud000-\udfff]|\ud83d[\ud000-\udfff]|\ud83e[\ud000-\udfff])",
            RegexOptions.Compiled);

        public static List<string> Split(string emotes)
        {
            var matches = _emotesRegex.Matches(emotes);

            return matches.Select(match => match.Value).ToList();
        }
    }
}
