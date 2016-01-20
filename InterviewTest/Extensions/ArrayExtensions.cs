using System;

namespace InterviewTest.Extensions
{
    public static class ArrayExtensions
    {
        private static readonly Random _random = new Random();
        public static string GetRandom(this string[] array) => array[_random.Next(0, array.Length)];
    }
}