using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Linq;
using HackerRank.Utility;

namespace HackerRank.ProblemSolving.InsertionSortAdvancedAnalysis
{

    class BinaryIndexTree
    {

        private readonly long[] tree;

        private readonly long[] values;

        public BinaryIndexTree(long limit)
        {
            tree = new long[limit];
            values = new long[limit];
        }


        public long Sum(long end)
        {

            long result = 0;
            long index = end;
            while (index > 0)
            {
                result += tree[index];
                index -= index & -index;
            }
            return result;
        }


        public void Add(long index, long value)
        {
            values[index] += value;

            while (index < tree.Length)
            {
                tree[index] += value;
                index += index & -index;
            }
        }

        public void Inc(long index)
        {
            Add(index, 1);
        }

        public void Prlong()
        {
            Console.WriteLine(string.Join(", ", tree));
            Console.WriteLine(string.Join(", ", values));
        }
    }


    class Result
    {

        public static long InsertionSort(List<long> arr)
        {
            var tree = new BinaryIndexTree(arr.Max() + 1);
            long shifts = 0;
            for (var i = arr.Count - 1; i >= 0; i--)
            {
                //Console.WriteLine(i);
                //Console.WriteLine(new String('-', 30));
                //Console.WriteLine(new String(' ', i*3) + " *");
                //Console.WriteLine($"[{string.Join(", ",arr)}]");
                //tree.Prlong();
                tree.Inc(arr[i]);
                shifts += tree.Sum(arr[i] - 1);
            }

            return shifts;
        }

    }

    class Solution
    {
        public static void Main_(string[] args)
        {
            TextWriter textWriter = new StreamWriter(@System.Environment.GetEnvironmentVariable("OUTPUT_PATH"), true);

            using var inputStream = new StreamReader(Console.OpenStandardInput());
            foreach (var result in Run(inputStream))
            {
                textWriter.WriteLine(result);
            }

            textWriter.Flush();
            textWriter.Close();
        }

        public static List<long> Run(StreamReader stream)
        {
            var results = new List<long>();

            long t = Convert.ToInt64(stream.ReadLine().Trim());

            for (long tItr = 0; tItr < t; tItr++)
            {
                long n = Convert.ToInt64(stream.ReadLine().Trim());

                List<long> arr = stream.ReadLine().TrimEnd().Split(' ').ToList().Select(arrTemp => Convert.ToInt64(arrTemp)).ToList();

                long result = Result.InsertionSort(arr);

                results.Add(result);
            }

            return results;
        }
    }

}


namespace HackerRank.ProblemSolving.InsertionSortAdvancedAnalysis.Tests
{
    class InsertionSortAdvancedAnalysis
    {

    }




    [TestClass]
    public class Tests
    {
        [TestMethod]
        public void CaseOne()
        {

            var input = @"2
5
1 1 1 2 2
5
2 1 3 1 2".ToStreamReader();

            var expected = new Stack<long>(new long[] { 4, 0 });

            foreach ( var actual in Solution.Run(input))
            {
                Assert.AreEqual(expected.Pop(), actual);
            }

        }


        [TestMethod]
        public void Case7()
        {
            using var input = new StreamReader(new FileStream("ProblemSolving/InsertionSortAdvancedAnalysis/Case7.txt", FileMode.Open));


            var expected = new Stack<long>(new long[] { 
                4999950000,
                4999950000, 
                0,
                0,
                0
            });

            foreach (var actual in Solution.Run(input))
            {
                Assert.AreEqual(expected.Pop(), actual);
            }
        }
    }
}


