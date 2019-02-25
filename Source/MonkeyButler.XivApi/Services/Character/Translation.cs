using System;
using System.Linq;
using MonkeyButler.XivApi.Attributes;

namespace MonkeyButler.XivApi.Services.Character
{
    internal static class Translation
    {
        public static string ToApiString(this GetCharacterData data)
        {
            var values = from value in Enum.GetValues(typeof(GetCharacterData)).Cast<GetCharacterData>()
                         where data.HasFlag(value)
                         select value.GetApiName();

            return string.Join(",", values);
        }

        private static string GetApiName(this GetCharacterData data)
        {
            var memInfo = typeof(GetCharacterData).GetMember(data.ToString()).FirstOrDefault();
            var attribute = memInfo?.GetCustomAttributes(typeof(ApiNameAttribute), false).FirstOrDefault() as ApiNameAttribute;

            return attribute?.Name;
        }
    }
}
