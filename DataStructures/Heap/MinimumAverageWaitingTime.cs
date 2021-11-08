using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;

namespace HackerRank.DataStructures.Heap.MinimumAverageWaitingTime
{
    using ComparerFunction = Func<Customer, Customer, bool>;

    class Result
    {

        /*
         * Complete the 'minimumAverage' function below.
         *
         * The function is expected to return an INTEGER.
         * The function accepts 2D_INTEGER_ARRAY customers as parameter.
         */

        public static long MinimumAverage(Heap customers)
        {
            long n = customers.Size;
            Heap current = new Heap((c1, c2) => c1.CookTime > c2.CookTime);

            long time = 0;

            long totalWait = 0;

            while (customers.Size > 0 || current.Size > 0)
            {
                while (customers.Size > 0 && customers.Data[0].ArrivalTime <= time)
                {
                    var arrival = customers.Pop();
                    current.Add(arrival);
                }
                if (current.Size > 0)
                {
                    var customer = current.Pop();
                    time += customer.CookTime;
                    totalWait += time - customer.ArrivalTime;

                } else
                {
                    time += 1;
                }
            }

            return totalWait / n;
        }

    }


    class Solution
    {

        public static long Solve(StreamReader input)
        {
            int n = Convert.ToInt32(input.ReadLine().Trim());

            Heap customers = new Heap((c1, c2) => c1.ArrivalTime > c2.ArrivalTime);
            for (int i = 0; i < n; i++)
            {
                var c = input
                .ReadLine()
                .TrimEnd()
                .Split(' ')
                .ToList()
                .Select(customersTemp => Convert.ToInt32(customersTemp))
                .ToList();

                customers.Add(new Customer()
                {
                    ArrivalTime = c[0],
                    CookTime = c[1]
                });
            }

            return Result.MinimumAverage(customers);
        }
    }

        [TestClass]
    public class MinimumAverageWaitingTimeTests
    {

        [TestMethod]
        public void TestCookTimePrio()
        {

            var input = new List<(long, long)[]>(){
                new  (long, long)[] { (0, 10), (1, 2) },
                new  (long, long)[] { (0, 10), (1, 2), (1, 8), (3, 7) },
            };
            var expected = new List<(long, long)[]>(){
                new (long, long)[] { (1, 2), (0, 10) },
                new (long, long)[] { (1, 2), (3, 7), (1, 8), (0, 10) },
            };

            for (int i = 0; i < input.Count; i++)
            {
                var heap = new Heap(
                    (c1, c2) => c1.CookTime > c2.CookTime
                );
                foreach (var c in input[i])
                {
                    heap.Add(new Customer { ArrivalTime = c.Item1, CookTime = c.Item2 });
                }

                foreach (var item in expected[i])
                {
                    var poppedCustomer = heap.Pop();
                    Assert.AreEqual(item.Item1, poppedCustomer.ArrivalTime);
                    Assert.AreEqual(item.Item2, poppedCustomer.CookTime);
                }
            }
        }

        [TestMethod]
        [DataRow(@"DataStructures\Heap\Input\MinimumAverageWaitingTime_Case1.txt", 8485548331)]
        public void TestFromFile(string filename, long expected)
        {
            var stream = new StreamReader(new FileStream(filename, FileMode.Open));
            Assert.AreEqual(expected, Solution.Solve(stream));
        }

    }

    public struct Customer
    {
        public long ArrivalTime, CookTime;
    }

    #region Heap
    public enum HeapType
    {
        Max, Min
    }
    public class Heap 
    { 

        public List<Customer> Data { get; set; } = new List<Customer>();
        public int Size => Data.Count;

        private readonly ComparerFunction ViolatesHeapProperty;

        public Heap(ComparerFunction violatesHeapProperty)
        {
            ViolatesHeapProperty = violatesHeapProperty;
        }

        public static Heap FromList(List<Customer> list, ComparerFunction violatesHeapProperty)
        {
            var heap = new Heap(violatesHeapProperty);
            foreach (var item in list)
            {
                heap.Add(item);
            }

            return heap;
        }

        public void Add(Customer v)
        {

            Data.Add(v);
            ReCalculateUp(Size-1);
        }

        internal void ReCalculateUp(int child)
        {
            int parent = (child - 1) / 2;
            while (0 <= parent && parent < Size)
            {
                if (ViolatesHeapProperty(Data[parent], Data[child]))
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
                    bestChild = (ViolatesHeapProperty(Data[left], Data[right])) ? right : left;
                }

                if (ViolatesHeapProperty(Data[index], Data[bestChild]))
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

        public Customer Pop()
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

            var holdsForLeft = !ViolatesHeapProperty(Data[root], Data[left]);
            var holdsForRight = !ViolatesHeapProperty(Data[root], Data[right]);

            return holdsForLeft
                && holdsForRight
                && VerifySubTree(left)
                && VerifySubTree(right);
        }

    }
    #endregion


}
