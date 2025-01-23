using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlDataGenerator.Logic
{
    public static class RandomDataGeneration
    {
        public static string GenerateRandomData(int length, string allowedChars, Random random)
        {
            // Use StringBuilder to construct the ID efficiently
            var stringBuilder = new StringBuilder(length);

            for (int i = 0; i < length; i++)
            {
                stringBuilder.Append(allowedChars[random.Next(allowedChars.Length)]);
            }

            return stringBuilder.ToString();
        }

        public static string PickRandomData(List<string> itemList, Random random)
        {
            var item = itemList[random.Next(itemList.Count)];
            return item;
        }
    }
}
