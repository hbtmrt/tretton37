using System.Collections.Generic;

namespace Tretton37.Extennsions
{
    public static class QueueExtensions
    {
        public static IEnumerable<T> DequeueChunk<T>(this Queue<T> queue, int chunkSize)
        {
            List<T> items = new List<T>();

            for (int i = 0; i < chunkSize && queue.Count > 0; i++)
            {
                items.Add(queue.Dequeue());
            }

            return items;
        }
    }
}