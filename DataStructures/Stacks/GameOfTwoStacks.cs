using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using HackerRank.Utility;

namespace HackerRank.DataStructures.Stacks
{
    public struct Input
    {
        public long MaxSum;
        public List<long> A, B;

    }


    class Result
    {

        /*
         * Complete the 'twoStacks' function below.
         *
         * The function is expected to return an INTEGER.
         * The function accepts following parameters:
         *  1. INTEGER maxSum
         *  2. INTEGER_ARRAY a
         *  3. INTEGER_ARRAY b
         */

        public static long TwoStacks(long maxSum, List<long> a, List<long> b)
        {
            var aMax = 0;
            var sum = 0L;
            while (aMax < a.Count && sum+a[aMax] <= maxSum)
            {
                sum += a[aMax++];
            }
            var maxCount = aMax;

            var bMax = 0;

            do {
                // Take as many bs as we can
                while (bMax < b.Count && sum + b[bMax] <= maxSum)
                {
                    sum += b[bMax++];
                }
                // Did we use more moves?
                if (aMax + bMax > maxCount)
                {
                    maxCount = aMax + bMax;
                }

                if (aMax > 0)
                {
                    // Remove last added aMax to make room for some bs
                    sum -= a[--aMax];
                } else
                {
                    break;
                }

            } while (bMax < b.Count && aMax >= 0);

            return maxCount;
        }

        public static Input[] ParseInput(string input)
        {
            var lines = input.Split("\n");

            var n = long.Parse(lines[0]);

            var result = new Input[n];


            for (var i = 0; i < n; i++)
            {
                var baseIndex = 3 * i + 1;

                var metrics = lines[baseIndex].Split(" ");

                result[i].MaxSum = long.Parse(metrics[2]);

                result[i].A = lines[baseIndex + 1].ToListOfLong();
                result[i].B = lines[baseIndex + 2].ToListOfLong();

            }

            return result;

            
        }


    }

    [TestClass]
    public class GameOfTwoStacksTests
    {

        [TestMethod]
        [DataRow(
            "1\n5 4 10\n4 2 4 6 1\n2 1 8 5",
            1,
            new long[] { 10 },
            new string[] { "4 2 4 6 1" },
            new string[] { "2 1 8 5" }
        )]

        [DataRow(
            "2\n1 2 2\n1 2\n3 4\n1 3 3\n1 2 3\n4 5 6",
            2,
            new long[] { 2, 3 },
            new string[] { "1 2", "1 2 3" },
            new string[] { "3 4", "4 5 6" }
        )]
        public void TestParseInput(string source, int count, long[] maxSum, string[] a, string[] b)
        {
            var inputs = Result.ParseInput(source);

            Assert.AreEqual(count, inputs.Length);

            for (int i = 0; i < count; i++)
            {
                Assert.AreEqual(maxSum[i], inputs[i].MaxSum);
                a[i].ToListOfLong()
                    .AreEqualTo(inputs[i].A);

                b[i].ToListOfLong()
                    .AreEqualTo(inputs[i].B);
            }
        }


        [TestMethod]
        [DataRow("1\n5 4 10\n4 2 4 6 1\n2 1 8 5", new long[] { 4 })]
        [DataRow("1\n5 4 10\n4 2 4 6 1\n2 1 1 5", new long[] { 5 })]
        public void Case0(string inputString, long[] expected)
        {
            var index = 0;
            foreach (var input in Result.ParseInput(inputString))
            {
                Assert.AreEqual(
                    expected[index++], 
                    Result.TwoStacks(input.MaxSum, input.A, input.B)
                );
            }
        }

        [TestMethod]
        [DataRow("Datastructures/Stacks/Input/GameOfTwoStacks_Case1.txt", new long[] { 6, 1, 6, 1, 6, 6, 8, 7, 8, 9, 12, 8, 2, 8, 13, 1, 5, 10, 4, 10, 7, 4, 5, 8, 6, 10, 4, 9, 7, 2, 13, 5, 11, 3, 3, 6, 5, 7, 5, 3, 7, 14, 14, 8, 0, 8, 5, 2, 11, 9 })]
        [DataRow("Datastructures/Stacks/Input/GameOfTwoStacks_Case5.txt", new long[] { 17, 32, 36,  3, 32, 13, 23, 22, 22, 13, 15,  9, 35, 14,  1, 21, 13, 24,  4,  6, 24, 11,  7, 26, 34,  5, 39, 36, 24, 29,  9, 21, 36, 34,  2, 29, 20, 26,  7,  7, 14,  6, 29, 22, 31,  4, 34, 15,  2,  6 })]
        [DataRow("Datastructures/Stacks/Input/GameOfTwoStacks_Case6.txt", new long[] {  1, 26,  7,  4, 19, 15, 27, 36, 28, 20,  4, 11, 13,  6,  0, 26,  7, 22, 32, 28,  8, 24, 22,  2, 36, 30, 17, 15, 12,  6, 29,  3,  0,  0, 26, 12, 34, 27, 25, 41,  1, 37,  3,  2, 25, 25,  2, 34,  8, 34 })]
        [DataRow("Datastructures/Stacks/Input/GameOfTwoStacks_Case7.txt", new long[] { 16, 20, 22, 11, 11, 16, 12, 7, 17, 19, 1, 12, 11, 4, 13, 6, 11, 1, 21, 10, 4, 19, 12, 23, 14, 4, 14, 1, 18, 12, 0, 13, 12, 7, 16, 10, 9, 12, 11, 7, 3, 10, 15, 15, 4, 18, 19, 13, 13, 15 })]
        [DataRow("Datastructures/Stacks/Input/GameOfTwoStacks_Case9.txt", new long[] { 178290, 178667, 178047, 178652, 178123, 178366, 178601, 178572, 178808, 179776, 179283, 179144, 179358, 178076, 178674, })]
        public void CaseFromFile(string fileName, long[] expected)
        {
            var index = 0;
            using var file = new StreamReader(new FileStream(fileName, FileMode.Open));

            var count = 1;
            foreach (var input in Result.ParseInput(file.ReadToEnd()))
            {
                Assert.AreEqual(
                    expected[index++], 
                    Result.TwoStacks(input.MaxSum, input.A, input.B),
                    $"\nPart #{count++} of file {fileName} failed.\nMax Sum {input.MaxSum}\nA: {input.A.PrettyString()}\nB: {input.B.PrettyString()}"
                );
            }
        }

    }
}
