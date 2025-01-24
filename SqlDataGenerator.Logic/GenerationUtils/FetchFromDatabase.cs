using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SqlDataGenerator.Logic.GenerationUtils
{
    public static class FetchFromDatabase
    {
        public static async Task<List<object>> FetchObjectListFromDatabase<TEntity>(
            int? records,
            string objectIdentifier,
            DbSet<TEntity> dbSet,
            Expression<Func<TEntity, string>> selector)
            where TEntity : class
        {
            try
            {
                // Ensure the number of records is valid
                if (records == null || records <= 0)
                    throw new ArgumentException("Records must be a positive integer.", nameof(records));

                // Fetch random records from the database
                var randomResults = await dbSet
                    .OrderBy(r => Guid.NewGuid())
                    .Take(records.Value)
                    .Select(selector)
                    .ToListAsync();

                // Ensure the list has the desired number of items
                while (randomResults.Count < records.Value)
                {
                    randomResults.AddRange(randomResults.Take(records.Value - randomResults.Count));
                }
                // Create a formatted result for output with dynamic property names
                var formatedResult = randomResults
                    .Take(records.Value)
                    .Select(i =>
                    {
                        dynamic expando = new ExpandoObject();
                        var dictionary = (IDictionary<string, object>)expando;
                        dictionary[objectIdentifier] = i;  // Use the objectIdentifier as the property name
                        return expando;
                    })
                    .ToList<object>();

                // Return the formatted result
                return formatedResult;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
                
        }
        public static async Task<List<string>> FetchStringListFromDatabase<TEntity>(
           int? records,
           DbSet<TEntity> dbSet,
           Expression<Func<TEntity, string>> selector)
           where TEntity : class
        {
            try {
                // Ensure the number of records is valid
                if (records == null || records <= 0)
                    throw new ArgumentException("Records must be a positive integer.", nameof(records));

                // Fetch random records from the database
                var randomResults = await dbSet
                    .OrderBy(r => Guid.NewGuid())
                    .Take(records.Value)
                    .Select(selector)
                    .ToListAsync();

                // Ensure the list has the desired number of items
                while (randomResults.Count < records.Value)
                {
                    randomResults.AddRange(randomResults.Take(records.Value - randomResults.Count));
                }

                // Return the formatted result
                return randomResults;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }       
        }
    }
}
