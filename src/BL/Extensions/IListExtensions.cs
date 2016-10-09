using System;
using System.Collections.Generic;

namespace BL.Extensions
{
    public static class IListExtensions
    {
        public static void Shuffle<T>(this Random rng, IList<T> array)
        {
            int n = array.Count;
            while (n > 1)
            {
                int k = rng.Next(n--);
                T temp = array[n];
                array[n] = array[k];
                array[k] = temp;
            }
        }
    }
}
