using System;
using System.Collections.Generic;

namespace Utils
{
    public class PriorityQueueElement<T>
    {
        public T data;
        public float priority;
    }

    public class PriorityQueue<T>
    {
        private List<PriorityQueueElement<T>> queue = new List<PriorityQueueElement<T>>();

        public bool IsEmpty()
        {
            return queue.Count == 0;
        }

        public void Queue(T element, float priority)
        {
            queue.Add(new PriorityQueueElement<T>()
            {
                data = element,
                priority = priority
            });
        }

        public T Dequeue()
        {
            T result = default(T);
            float bestPriority = float.MinValue;
            int removeIndex = -1;
            for (var i = queue.Capacity - 1; i >= 0; i--)
            {
                var element = queue[i];
                if (element.priority > bestPriority)
                {
                    result = element.data;
                    bestPriority = element.priority;
                    removeIndex = i;
                }
            }
            if (removeIndex >= 0) queue.RemoveAt(removeIndex);
            return result;
        }

        public T Find(Func<T, bool> finder)
        {
            var rs = queue.Find(element => finder(element.data));
            if (rs != null)
            {
                return rs.data;
            }
            return default(T);
        }
    }
}