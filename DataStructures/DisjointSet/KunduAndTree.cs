using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;

namespace HackerRank.DataStructures.DisjointSet
{
    class KunduAndTree
    {

        private readonly int[] size;
        private readonly int n;

        public int FindRoot(int start) {
            var chain = new HashSet<int>();
            var i = 0;
            while (size[start] > 0)
            {
                chain.Add(start);
                start = size[start];

                if (++i > n) throw new Exception($"Loop when finding root. Members {string.Join(", ", chain)}");
            }
            return start;
        }

        public KunduAndTree(int n, List<(int,int)> edges)
        {
            this.n = n;


            size = new int[n];

            for (int i = 0; i < n; i++)
            {
                size[i] = -1;
            }


            foreach (var (a, b) in edges)
            {
                AddEdge(a, b);
            }
        }

        public void AddEdge(int a, int b)
        {
            int rootA = FindRoot(a);
            int rootB = FindRoot(b);

            if (a == b || rootA == rootB) return;

            var sizeA = -size[rootA];


            var sizeB = -size[rootB];

            if (size[rootA] < size[rootB])
            {
                size[rootA] += size[rootB];
                size[rootB] = rootA;
            }
            else
            {
                size[rootB] += size[rootA];
                size[rootA] = rootB;
            }
        }

        static long Choose2(long n) => (n - 1) * n / 2;
        static long Choose3(long n) => (n - 2) * (n - 1) * n / 6;

        public long Solve()
        {
            var total = Choose3(n);

            for (var i = 0; i < n; i++) {
                if (size[i] >= 0) continue;
                var s = -size[i];
                Console.WriteLine($"Size: {s}");
                total -= Choose2(s) * (n-s);
                total -= Choose3(s);
            }
            return total % 1_000_000_007L;
        }



    }


    [TestClass]
    public class KunduAndTreeTests
    {
        [TestMethod]
        [DataRow("5\n1 2 b\n2 3 r\n3 4 r\n4 5 b", 4)]
        [DataRow("5\n1 2 r\n2 3 r\n3 4 r\n4 5 r", 10)]
        [DataRow("5\n1 2 b\n2 3 b\n3 4 b\n4 5 b", 0)]
        public void FromString(string input, long expected)
        {

            ParseInput(input, out int n, out List<(int, int)> edges);

            long actual = new KunduAndTree(n, edges).Solve();

            Assert.AreEqual(expected, actual);


        }

        [TestMethod]
        [DataRow("KunduAndTree_Case5.txt", 980449749)]
        public void Fromfile(string filename, long expected)
        {
            var fullName = Path.Combine(@"DataStructures\DisjointSet\Tests\Input", filename);

            var input = new StreamReader(new FileStream(fullName, FileMode.Open)).ReadToEnd();

            ParseInput(input, out int n, out List<(int, int)> edges);

            long actual = new KunduAndTree(n, edges).Solve();

            Assert.AreEqual(expected, actual);
        }

        private static void ParseInput(string input, out int n, out List<(int, int)> edges)
        {
            var parts = input.Split("\n");

            n = int.Parse(parts[0]);
            edges = new List<(int, int)>();
            for (int i = 1; i < n; i++)
            {
                var edge_parts = parts[i].Trim().Split(" ");

                if (edge_parts[2] == "r") continue;

                edges.Add(
                    (
                        int.Parse(edge_parts[0]) - 1,
                        int.Parse(edge_parts[1]) - 1
                    )
                );
            }
        }
    }
}
