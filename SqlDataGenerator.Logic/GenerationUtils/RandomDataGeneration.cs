using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlDataGenerator.Logic.GenerationUtils
{
    public static class RandomDataGeneration
    {
        //Generate random string with the given characters
        public static string GenerateRandomData(int length, string allowedChars, Random random)
        {
            try
            { // Use StringBuilder to construct the ID efficiently
                var stringBuilder = new StringBuilder(length);
                for (int i = 0; i < length; i++)
                {
                    stringBuilder.Append(allowedChars[random.Next(allowedChars.Length)]);
                }

                return stringBuilder.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        //Generate random string with the given characters, if startWithZero is true, the first character will not be zero
        public static string GenerateRandomData(int length, string allowedChars, Random random, bool startWithZero = false)
        {
            try
            { // Use StringBuilder to construct the ID efficiently
                var stringBuilder = new StringBuilder(length);
                if (startWithZero)
                {
                    for (int i = 0; i < length; i++)
                    {
                        stringBuilder.Append(allowedChars[i == 0
                            ? random.Next(1, allowedChars.Length)
                            : random.Next(allowedChars.Length)]);
                    }
                }
                return stringBuilder.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        //Generate random string with the given characters, if prefix is given,
        //string will be generated to the lenght given with the prefix included
        public static string GenerateRandomData(int length, string allowedChars, Random random, string prefix)
        {
            try
            { // Use StringBuilder to construct the ID efficiently
                var stringBuilder = new StringBuilder(length);
                stringBuilder = !string.IsNullOrEmpty(prefix) ? stringBuilder.Append(prefix) : stringBuilder;
                for (int i = 0; i < length; i++)
                {
                    stringBuilder.Append(allowedChars[random.Next(allowedChars.Length)]);
                }

                return stringBuilder.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        //Generate random number between the given range
        public static int GenerateRandomNumber(int minLength, int maxLength, Random random)
        {
            try
            {
                var randomInt = random.Next(minLength, maxLength);

                return randomInt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        //Generate random number between 0 and the given range
        public static int GenerateRandomNumber(int maxLength, Random random)
        {
            try
            {
                var randomInt = random.Next(maxLength);
                return randomInt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        //Generate random date between the given range
        public static dynamic GenerateRandomDate(DateTime? minDate, DateTime? maxDate, bool includeTime, Random random)
        {
            try
            {
                var randomDate = minDate.Value.AddDays(GenerateRandomNumber((maxDate - minDate).Value.Days + 1, random));

                if (includeTime)
                {
                    // Add random hours, minutes, seconds, and milliseconds
                    var randomTimeSpan = new TimeSpan(
                        hours: GenerateRandomNumber(0, 24, random),
                        minutes: GenerateRandomNumber(0, 60, random),
                        seconds: GenerateRandomNumber(0, 60, random)
                    );

                    randomDate = randomDate.Add(randomTimeSpan);
                }

                return !includeTime ? randomDate.ToString("yyyy-MM-dd") : randomDate;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        //Pick random data from the given list
        public static string PickRandomData(List<string> itemList, Random random)
        {
            try
            {
                var item = itemList[random.Next(itemList.Count)];
                return item;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
    }
}
