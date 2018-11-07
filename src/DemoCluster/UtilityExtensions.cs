using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace DemoCluster
{
    public static class UtilityExtensions
    {
        public static TRelated Load<TRelated>(
            this Action<object, string> loader,
            object entity,
            ref TRelated navigationField,
            [CallerMemberName] string navigationName = null)
            where TRelated : class
        {
            loader?.Invoke(entity, navigationName);

            return navigationField;
        }

        public static PaginatedList<T> ToPaginatedList<T>(
            this IEnumerable<T> source,
            int pageIndex,
            int pageSize) where T : class
        {
            var enumerable = source as T[] ?? source.ToArray();
            var count = enumerable.Length;
            var items = enumerable.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            return new PaginatedList<T>(items, count, pageIndex, pageSize);
        }
    }
}
