using System;

namespace Assets.Scripts.Utils
{
    public static class ConvertUtil
    {
        private static string[] BigNumsChars = new string[] { "", "K", "M", "B", "T" };

        public static string ConvertK(this int value)
        {
            int temp = value;
            int ranks = 0;
            while ((temp = temp / 1000) > 0)
            {
                ranks++;
            }
            int finalRank = BigNumsChars.Length > ranks
                ? ranks
                : BigNumsChars.Length - 1;
            return (value / Math.Pow(1000, finalRank)).ToString("0.##") + BigNumsChars[finalRank];
        }
    }
}
