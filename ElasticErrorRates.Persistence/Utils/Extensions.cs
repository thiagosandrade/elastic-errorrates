using System;
using System.Collections.Generic;
using System.Text;

namespace ElasticErrorRates.Persistence.Utils
{
    public static class Extensions
    {
        public static IEnumerable<IEnumerable<T>> Chunk<T>(this IEnumerable<T> source, int chunkSize)
        {
            var chunk = new List<T>(chunkSize);

            foreach (var item in source)
            {
                chunk.Add(item);
                if (chunk.Count != chunkSize) continue;

                yield return chunk;
                chunk.Clear();
            }

            if (chunk.Count > 0)
            {
                yield return chunk;
            }
        }
    }
}
