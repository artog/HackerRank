using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.CompilerServices;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HackerRank.DataStructures.Queue
{

    class Counter {

        public Dictionary<int, int> Counts { get; set; } = new ();

        public void Add(int n) {

            Counts.TryGetValue(n, out var x);

            Counts[n] = x+1;
        }

        public void Remove(int n) {
            Counts[n]--;
        }

        public int this[int index] {
            get => Counts[index];
            set => Counts[index] = value;
        }

    }

    class Result
    {

        public static int SolveQuery(List<int> arr, int d) {
            var max = 0;
            var min = arr[0];
            var counter = new Counter();

            var queue = new LinkedList<int>();

            for (var i = 0; i < arr.Count; i++) {
                if (i < d) {
                    if (max < arr[i]) {
                        max = arr[i];
                        min = max;
                    }

                    queue.AddLast(arr[i]);

                    counter.Add(arr[i]);
                }
                else {
                    var left = queue.First.Value;
                    counter.Remove(left);

                    queue.RemoveFirst();

                    queue.AddLast(arr[i]);
                    counter.Add(arr[i]);
                    


                    if (counter[left] == 0 && left == max) {
                        // Recalculate max
                        max = queue.Max();


                        if (max < min) {
                            min = max;
                        }
                    }

                }
            }
            return min;
        }

        public static List<int> Solve(List<int> arr, List<int> queries) {
            return queries
                .Select(q => SolveQuery(arr, q))
                .ToList();
        }

    }


    [TestClass]
    public class QueriesWithFixedLengthTests
    {


        [TestMethod]
        [DataRow(new [] { 1,2,3,1 }, new [] { 1,2,3 }, new [] { 2,1,1 })]
        [DataRow(new int[] { }, new int[] { }, new int[] { })]
        [DataRow(new [] { 1,1,1,1,1 }, new [] { 1 }, new [] { 5 })]
        public void CounterTest(int[] arr, int[] keys, int[] counts) {

            var counter = new Counter();


            foreach (var i in arr) {
                counter.Add(i);
            }


            Assert.AreEqual(keys.Length, counter.Counts.Keys.Count);

            foreach (var key in keys) {
                Assert.IsTrue(counter.Counts.ContainsKey(key));
            }

            var idx = 0;
            var sortedKeys = counter.Counts.Keys.ToList();
            sortedKeys.Sort();
            foreach (var key in sortedKeys) {
                Assert.AreEqual(counts[idx++], counter.Counts[key]);
            }

        }

        [TestMethod]
        [DataRow(new []{33, 11, 44, 11, 55}, 1, 11)]
        [DataRow(new []{33, 11, 44, 11, 55}, 2, 33)]
        [DataRow(new []{33, 11, 44, 11, 55}, 3, 44)]
        [DataRow(new []{33, 11, 44, 11, 55}, 4, 44)]
        [DataRow(new []{33, 11, 44, 11, 55}, 5, 55)]
        [DataRow(new []{ 1,  2,  3,  4,  5}, 1,  1)]
        [DataRow(new []{ 1,  2,  3,  4,  5}, 2,  2)]
        [DataRow(new []{ 1,  2,  3,  4,  5}, 3,  3)]
        [DataRow(new []{ 1,  2,  3,  4,  5}, 4,  4)]
        [DataRow(new []{ 1,  2,  3,  4,  5}, 5,  5)]
        public void TestSingleQuery(int[] arr, int d, int expected) {
            Assert.AreEqual(expected, Result.SolveQuery(arr.ToList(), d));
        }


    }
}
