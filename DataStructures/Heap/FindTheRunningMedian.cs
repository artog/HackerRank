using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HackerRank.DataStructures.Heap.FindTheRunningMedian
{

    #region Heap
    public enum HeapType
    {
        Max, Min
    }

    public class Heap
    {

        public int?[] Data { get; set; } = new int?[100000];
        public int Size { get; private set; } = 0;

        private readonly Func<int?, int?, bool> ViolatesHeapProperty;

        public Heap(HeapType type)
        {
            if (type == HeapType.Max)
            {
                ViolatesHeapProperty = (int? parent, int? child) => (parent ?? int.MinValue) < (child ?? int.MinValue);
            }
            else
            {
                ViolatesHeapProperty = (int? parent, int? child) => (parent ?? int.MaxValue) > (child ?? int.MaxValue);
            }
        }

        public static Heap FromList(List<int> list, HeapType type)
        {
            var heap = new Heap(type);
            foreach (var item in list)
            {
                heap.Add(item);
            }

            return heap;
        }

        public void Add(int v)
        {

            var i = Size++;
            Data[i] = v;
            ReCalculateUp(i);
        }

        internal void ReCalculateUp(int child)
        {
            int parent = (child - 1) / 2;
            while (parent >= 0)
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
            while (Data[left].HasValue || Data[right].HasValue)
            {

                int bestChild = (ViolatesHeapProperty(Data[left], Data[right])) ? right : left;

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
            (Data[parent], Data[child]) = (Data[child], Data[parent]);
        }

        public int? Pop()
        {
            if (Size > 0)
            {
                int? value = Data[0];
                Swap(0, --Size);
                Data[Size] = null;

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
            if (!Data[root].HasValue) return true;

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

    public class MedianHeap
    {
        public Heap MaxHeap { get; set; } = new Heap(HeapType.Max);
        public Heap MinHeap { get; set; } = new Heap(HeapType.Min);

        public void Add(int v)
        {

            if (v > Median())
            {
                Console.WriteLine($"    Adding {v} to min");
                MinHeap.Add(v);
            } else
            {
                Console.WriteLine($"    Adding {v} to max");
                MaxHeap.Add(v);
            }
            MaintainBalance();
        }

        public void MaintainBalance()
        {
            if (MaxHeap.Size > MinHeap.Size + 1)
            {
                var value = MaxHeap.Pop();
                Console.WriteLine($"    Moving {value} to min");
                MinHeap.Add(value.Value);
                Console.WriteLine($"    New sizes: Min {MinHeap.Size} Max {MaxHeap.Size}");
            }

            if (MinHeap.Size > MaxHeap.Size + 1)
            {
                var value = MinHeap.Pop();
                Console.WriteLine($"    Moving {value} to max");
                MaxHeap.Add(value.Value);
                Console.WriteLine($"    New sizes: Min {MinHeap.Size} Max {MaxHeap.Size}");
            }


        }

        public double Median()
        {
            if (MaxHeap.Size > MinHeap.Size)
            {
                return MaxHeap.Data[0].Value;
            } 
            
            if (MaxHeap.Size < MinHeap.Size)
            {
                return MinHeap.Data[0].Value;
            }

            double a = MaxHeap.Data[0] ?? 0;
            double b = MinHeap.Data[0] ?? 0;

            return (a+b) / 2.0;
        }
    }

    public class Result
    {
        public static List<string> RunningMedian(List<long> list)
        {

            return list.Select(e => e.ToString()).ToList();
        }
    }


    [TestClass]
    public class FindTheRunningMedianTests
    {

        [TestMethod] public void TestTree()
        {
            Result.RunningMedian(new long[] { 1, 2, 3, 4, 5 }.ToList<long>());
        }


    }

    #region Heap Tests

    [TestClass]
    public class HeapTests
    {
        [DataRow(new[] { 0, 1 }, 0, 1, new[] { 1, 0 })]
        [DataRow(new[] { 0, 1, 2 }, 1, 2, new[] { 0, 2, 1 })]
        [DataRow(new[] { 0, 1, 2 }, 0, 2, new[] { 2, 1, 0 })]
        [TestMethod]
        public void TestSwap(int[] data, int a, int b, int[] expected)
        {
            var heap = new Heap(HeapType.Min);
            for (int i = 0; i < data.Length; i++)
            {
                heap.Data[i] = data[i];
            }

            heap.Swap(a, b);

            for (int i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(expected[i], heap.Data[i]);
            }
        }


        [DataRow(new[] { 0, 1 }    , HeapType.Min, new[] { 0, 1 })]
        [DataRow(new[] { 0, 1, 2 } , HeapType.Min, new[] { 0, 1, 2 })]
        [DataRow(new[] { 0, 1, 10 }, HeapType.Min, new[] { 0, 1, 10 })]
        [DataRow(new[] { 0, 1, 10 }, HeapType.Max, new[] { 10, 0, 1 })]
        [DataRow(new[] { 0,1,2,3,4,5,6,7,8,9 }, HeapType.Max, new[] { 9,8,5,6,7,1,4,0,3,2 })]
        [TestMethod]
        public void TestAdd(int[] data, HeapType type, int[] expected)
        {
            var heap = new Heap(type);

            for (int i = 0; i < data.Length; i++)
            {
                heap.Add(data[i]);
            }


            for (int i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(expected[i], heap.Data[i]);
            }

            Assert.IsTrue(heap.Verify());
        }


        [DataRow(new[] { 0, 1 }    , HeapType.Min, new[] { 0, 1 })]
        [DataRow(new[] { 0, 1, 2 } , HeapType.Min, new[] { 0, 1, 2 })]
        [DataRow(new[] { 0, 1, 10 }, HeapType.Min, new[] { 0, 1, 10 })]
        [DataRow(new[] { 0, 1, 10 }, HeapType.Max, new[] { 10, 0, 1 })]
        [DataRow(new[] { 0,1,2,3,4,5,6,7,8,9 }, HeapType.Max, new[] { 9,8,5,6,7,1,4,0,3,2 })]
        [TestMethod]
        public void TestFromList(int[] data, HeapType type, int[] expected)
        {
            var heap = Heap.FromList(data.ToList(), type);

            for (int i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(expected[i], heap.Data[i]);
            }
            Assert.IsTrue(heap.Verify());
        }

        [DataRow(HeapType.Min,  true, new[] { 0, 1 })]
        [DataRow(HeapType.Max, false, new[] { 0, 1, 2 })]
        [DataRow(HeapType.Min,  true, new[] { 0, 1, 10 })]
        [DataRow(HeapType.Max,  true, new[] { 10, 0, 1 })]
        [DataRow(HeapType.Max,  true, new[] { 9, 8, 5, 6, 7, 1, 4, 0, 3, 2 })]
        [DataRow(HeapType.Min, false, new[] { 9, 8, 5, 6, 7, 1, 4, 0, 3, 2 })]
        [TestMethod] public void TestVerify(HeapType type, bool expected, int[] data)
        {
            var heap = new Heap(type);

            for (int i = 0; i < data.Length; i++)
            {
                heap.Data[i] = data[i];
            }


            Assert.AreEqual(expected, heap.Verify());
        }




        [DataRow(new[] { 0, 1 }, HeapType.Min, new[] { 1 }, 0)]
        [DataRow(new[] { 0, 1, 2 }, HeapType.Min, new[] { 1, 2 }, 0)]
        [DataRow(new[] { 0, 1, 10 }, HeapType.Min, new[] { 1, 10 }, 0)]
        [DataRow(new[] { 0, 1, 10 }, HeapType.Max, new[] { 1, 0 }, 10)]
        [DataRow(new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }, HeapType.Max, new[] { 8, 7, 5, 6, 2, 1, 4, 0, 3 }, 9)]
        [TestMethod]
        public void TestRemove(int[] data, HeapType type, int[] expected, int expectedPoppedElement)
        {
            var heap = Heap.FromList(data.ToList(), type);

            var top = heap.Pop();

            Assert.AreEqual(expectedPoppedElement, top);

            for (int i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(expected[i], heap.Data[i]);
            }
            Assert.IsTrue(heap.Verify());
        }

    }

    #endregion

    #region MedianHeap Tests

    [TestClass]
    public class MedianHeapTests
    {


        [DataRow(new [] {1,1,1}, new[] { 1.0, 1.0, 1.0 })]
        [DataRow(new [] {1,2,3}, new[] { 1.0, 1.5, 2.0 })]
        [DataRow(new [] {7,3,5,2}, new[] { 7.0,5.0,5.0,4.0 })]
        [DataRow(new [] {1,2,3,4,5,6,7,8,9,10}, new[] { 1.0,1.5,2.0,2.5,3.0,3.5,4.0,4.5,5.0,5.5 })]
        [TestMethod]
        public void TestMedian(int[] data, double[] expected)
        {
            var medianHeap = new MedianHeap();

            for (int i = 0; i < data.Length; i++)
            {
                medianHeap.Add(data[i]);
                Console.WriteLine($"Added {data[i]}. Median is {medianHeap.Median()}");
                Assert.AreEqual(medianHeap.Median(), expected[i]);
            }
        }
    }

    #endregion
}
