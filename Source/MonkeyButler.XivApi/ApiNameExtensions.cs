using System;
using System.Linq;
using System.Net;
using System.Reflection;
using MonkeyButler.XivApi.Attributes;

namespace MonkeyButler.XivApi
{
    internal static class ApiNameExtensions
    {
        public static string GetApiName(this Enum data)
        {
            var memInfo = data.GetType().GetMember(data.ToString()).FirstOrDefault();
            var attribute = memInfo?.GetCustomAttributes(typeof(ApiNameAttribute), false).FirstOrDefault() as ApiNameAttribute;
            return attribute?.Name ?? data.ToString();
        }

        public static string GetApiName(this PropertyInfo propertyInfo)
        {
            var attribute = propertyInfo?.GetCustomAttributes(typeof(ApiNameAttribute), false).FirstOrDefault() as ApiNameAttribute;
            return attribute?.Name ?? propertyInfo.Name.ToLower();
        }

        public static string GetApiString(this Enum data)
        {
            var values = from value in Enum.GetValues(data.GetType()).Cast<Enum>()
                         where data.HasFlag(value)
                         select value.GetApiName();

            return string.Join(",", values);
        }

        public static string GetQueryString(this CriteriaBase criteria)
        {
            var values = from propInfo in criteria.GetType().GetProperties()
                         let value = propInfo.GetValue(criteria)?.ToString()
                         where !string.IsNullOrEmpty(value)
                         select $"{propInfo.GetApiName()}={WebUtility.UrlEncode(value)}";

            return string.Join("&", values);
        }
    }
}
