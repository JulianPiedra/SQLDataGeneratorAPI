using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlDataGenerator.Logic.GenerationUtils
{
    public static class RandomDataGeneration
    {
        public static string GenerateRandomData(int length, string allowedChars, Random random, bool? startWithZero = false)
        {
            try
            { // Use StringBuilder to construct the ID efficiently
                var stringBuilder = new StringBuilder(length);
                if (!startWithZero.Value)
                    for (int i = 0; i < length; i++)
                    {
                        stringBuilder.Append(allowedChars[random.Next(allowedChars.Length)]);
                    }
                else
                {
                    for (int i = 0; i < length; i++)
                    {
                        if (i != 0) stringBuilder.Append(allowedChars[random.Next(allowedChars.Length)]);
                        else
                        {
                            stringBuilder.Append(allowedChars.Substring(1)[random.Next(allowedChars.Length-1)]);
                        }
                    }
                }

                return stringBuilder.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
       

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
