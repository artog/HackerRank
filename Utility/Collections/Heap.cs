using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackerRank.Utility.Collections
{
  

    #region Heap
    public class Heap<T>
    { 

        public List<T> Data { get; set; } = new List<T>();
        public int Size => Data.Count;

        private readonly Func<T,T, bool> violatesHeapProperty;

        public Heap(Func<T,T, bool> violatesHeapProperty)
        {
            this.violatesHeapProperty = violatesHeapProperty;
        }

        public static Heap<T> FromList(List<T> list, Func<T,T, bool> violatesHeapProperty)
        {
            var heap = new Heap<T>(violatesHeapProperty);
            foreach (var item in list)
            {
                heap.Add(item);
            }

            return heap;
        }

        public void Add(T v)
        {

            Data.Add(v);
            ReCalculateUp(Size-1);
        }

        internal void ReCalculateUp(int child)
        {
            int parent = (child - 1) / 2;
            while (0 <= parent && parent < Size)
            {
                if (violatesHeapProperty(Data[parent], Data[child]))
                {
                    Swap(parent, child);
                }
                // If we didn't swap, we dont need to keep going as we know the heap property still applies to the rest of elements
                else { return; }
                child = parent;
                parent = (child - 1) / 2;
            }
        }

        internal void ReCalculateDown()
        {
            int index = 0;
            var left = 2 * index + 1;
            var right = 2 * index + 2;
            while (left < Size || right < Size)
            {

                int bestChild;
                if (left >= Size)
                {
                    bestChild = right;
                } else if (right >= Size)
                {
                    bestChild = left;
                } else
                {
                    bestChild = (violatesHeapProperty(Data[left], Data[right])) ? right : left;
                }

                if (violatesHeapProperty(Data[index], Data[bestChild]))
                {
                    Swap(index, bestChild);
                }
                else
                {
                    return;
                }
                index = bestChild;
                left = 2 * index + 1;
                right = 2 * index + 2;

            }
        }

        internal void Swap(int parent, int child)
        {
            var temp = Data[child];
            Data[child] = Data[parent];
            Data[parent] = temp;
        }

        public T Pop()
        {
            if (Size > 0)
            {
                var value = Data[0];
                Swap(0, Size - 1);
                Data.RemoveAt(Size - 1);

                ReCalculateDown();

                return value;
            }
            throw new InvalidOperationException("Cannot pop item from empty heap");
        }

        public bool Verify()
        {
            return VerifySubTree(0);
        }

        public bool VerifySubTree(int root)
        {
            if (root >= Size) return true;

            var left = 2 * root + 1;
            var right = 2 * root + 2;

            var holdsForLeft = !violatesHeapProperty(Data[root], Data[left]);
            var holdsForRight = !violatesHeapProperty(Data[root], Data[right]);

            return holdsForLeft
                && holdsForRight
                && VerifySubTree(left)
                && VerifySubTree(right);
        }

    }
#endregion
}
