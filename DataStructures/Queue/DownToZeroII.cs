using HackerRank.Utility.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackerRank.DataStructures.Queue
{

    public class State
    {
        public int N, Steps;

        public State(int n, int steps)
        {
            N = n;
            Steps = steps;
        }
    }

    public class DownToZeroII
    {
        public static List<int> FindPossibleTargets(int n) {
            var targets = new List<int>();
            var sqrt = Math.Sqrt(n);
            var start = (int)Math.Ceiling(sqrt);
            
            for (;start < n; start++)
            {
                if (n % start == 0) targets.Add(start);
            }

            targets.Add(n - 1);

            return targets;
        }


        public static int Solve(int n)
        {
            var steps = 0;
            var queue = new Heap<State>((State a, State b) => a.Steps > b.Steps);
            var seen = new HashSet<int>();

            queue.Add(new State(n, 0));
            seen.Add(n);

            while (queue.Size > 0)
            {
                var current = queue.Pop();

                if (current.N == 0) return current.Steps;
                


                foreach (var target in FindPossibleTargets(current.N)) {
                    
                    if (!seen.Contains(target))
                    {
                        queue.Add(new State(target, current.Steps + 1));
                        seen.Add(target);
                    } 
                }
                
            }

            return steps;
        }
    }


    [TestClass]
    public class DownToZero2Tests
    {

        [TestMethod]
        [DataRow(  4, new [] { 2, 3 })]
        [DataRow(  5, new [] { 4 })]
        [DataRow(  6, new [] { 3, 5 })]
        [DataRow(  8, new [] { 4,7 })]
        [DataRow( 14, new [] { 7, 13 })]
        [DataRow( 12, new [] { 4, 6, 11 })]
        [DataRow(720, new [] { 30, 36, 40, 45, 48, 60, 72, 80, 90, 120, 144, 180, 240, 360, 719 })]
        public void TestLowestMaximumDivisor(int n, int[] expected) {

            var actual = DownToZeroII.FindPossibleTargets(n);
            for (int i = 0; i < expected.Length; i++) {
                Assert.AreEqual(expected[i], actual[i]);
            }
        }


        [TestMethod]
        [DataRow(3,3)]
        [DataRow(4,3)]
        public void TestNumber(int n, int expected)
        {
            Assert.AreEqual(expected, DownToZeroII.Solve(n));
        }



        [TestMethod]
        [DataRow(
            new [] { 3, 34, 86, 73, 40,  7, 87, 57, 81, 32, 83, 39, 98, 89, 86, 44, 29, 36, 53, 44, 72, 31, 88, 30, 78, 78, 55, 27, 39, 42, 95, 95, 25, 88, 51,  6, 93, 41, 90, 34, 96, 68, 81, 63,  6,  7, 33, 26, 66, 79,  4, 89, 14, 33, 22, 48, 17, 62, 11 },
            new [] { 3,  6,  7,  6,  5,  5,  8,  6,  5,  5,  7,  6,  7,  8,  7,  7,  7,  5,  7,  7,  5,  6,  7,  5,  6,  6,  6,  5,  6,  6,  7,  7,  5,  7,  6,  4,  7,  6,  6,  6,  5,  6,  5,  5,  4,  5,  6,  6,  7,  7,  3,  8,  6,  6,  7,  5,  5,  7,  6 }
        )]
        public void TestMultipleNumbers(int[] numbers, int[] expected)
        {
            for (int i = 0; i < numbers.Length; i++)
            {
                int actual = DownToZeroII.Solve(numbers[i]);
                Assert.AreEqual(expected[i], actual, $"Solved for number {numbers[i]} should be {expected[i]}, but was {actual}");
            }
        }
    }

}
