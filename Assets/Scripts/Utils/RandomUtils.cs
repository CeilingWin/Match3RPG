using System;
using System.Collections.Generic;

namespace Utils
{
    public class RandomUtils
    {
        public static void ShuffleArray<T>(T[] arr)
        {
            Random rnd = new Random();
            int n = arr.Length;
            while (n > 1)
            {
                int k = rnd.Next(n--);
                (arr[n], arr[k]) = (arr[k], arr[n]);
            }
        }
    }
}