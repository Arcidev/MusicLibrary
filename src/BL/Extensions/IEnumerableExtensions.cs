using System;
using System.Collections.Generic;
using System.Linq;

namespace BL.Extensions
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<IEnumerable<T>> Chunk<T>(this IEnumerable<T> source, int chunksize)
        {
            if (chunksize <= 0)
                throw new ArgumentException($"{nameof(chunksize)} must be higher than 0");

            var pos = 0;
            while (source.Skip(pos).Any())
            {
                yield return source.Skip(pos).Take(chunksize);
                pos += chunksize;
            }
        }
    }
}
