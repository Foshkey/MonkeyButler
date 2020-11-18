using LiteDB;

namespace MonkeyButler.Data.Database
{
    internal static class LiteDbExtensions
    {
        /// <summary>
        /// Correctly finds by ulong... until really high numbers.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <remarks>
        /// Unfortunately, LiteDB does not support ulongs. This likely won't cause issues in the
        /// lifetime of Discord... fingers crossed.
        /// </remarks>
        public static T FindByUlongId<T>(this ILiteCollection<T> collection, ulong id) => collection.FindById((long)id);
    }
}
