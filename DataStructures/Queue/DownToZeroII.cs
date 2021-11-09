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
        public static int FindLowestMaxDivisor(int n)
        {
            double sqrt = Math.Sqrt(n);
            int start = (int)Math.Ceiling(sqrt);

            if (sqrt == start)
            {
                return start;
            }

            for (;start <= n; start++)
            {
                if (n % start == 0) return start;
            }

            return n;
        }


        public static int Solve(int n)
        {
            int steps = 0;
            Heap<State> queue = new Heap<State>((State a, State b) => a.Steps > b.Steps);
            queue.Add(new State(n, 0));
            HashSet<int> seen = new HashSet<int>();
            seen.Add(n);

            while (queue.Size > 0)
            {
                var current = queue.Pop();

                if (current.N == 0) return current.Steps;

                int divisor = FindLowestMaxDivisor(current.N);

                if (divisor != current.N && !seen.Contains(divisor))
                {
                    queue.Add(new State(divisor, current.Steps + 1));
                    seen.Add(divisor);
                } 
                if (!seen.Contains(current.N-1))
                {
                    queue.Add(new State(current.N-1, current.Steps + 1));
                    seen.Add(current.N-1);
                }
                
            }

            return steps;
        }
    }


    [TestClass]
    public class DownToZero2Tests
    {

        [TestMethod]
        [DataRow(4, 2)]
        [DataRow(5, 5)]
        [DataRow(6, 3)]
        [DataRow(8, 4)]
        [DataRow(14, 7)]
        [DataRow(12, 4)]
        [DataRow(12, 4)]
        [DataRow(720, 30)]
        public void TestLowestMaximumDivisor(int n, int expected)
        {
            Assert.AreEqual(expected, DownToZeroII.FindLowestMaxDivisor(n));
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
