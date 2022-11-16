using System;
using System.Collections.Generic;

namespace MusicLibrary.Extensions
{
    public static class IListExtensions
    {
        public static void Shuffle<T>(this Random rng, IList<T> array)
        {
            int n = array.Count;
            while (n > 1)
            {
                int k = rng.Next(n--);
                (array[k], array[n]) = (array[n], array[k]);
            }
        }
    }
}
