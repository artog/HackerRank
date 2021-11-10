using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackerRank.DataStructures.DisjointSet
{
    class KunduAndTree
    {

        private readonly int[] size;
        private readonly int[] people;
        private readonly int n;

        public int FindRoot(int start)
        {
            while (people[start] != start)
            {
                start = people[start];
            }
            return start;
        }

        public KunduAndTree(int n, List<(int,int)> edges)
        {
            this.n = n;


            size = new int[n];
            people = new int[n];

            for (int i = 0; i < n; i++)
            {
                people[i] = i;
                size[i] = 1;
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

            if (size[rootA] > size[rootB])
            {
                size[rootA] += size[rootB];
                people[rootB] = rootA;
            }
            else
            {
                size[rootB] += size[rootA];
                people[rootA] = rootB;

            }
        }


        public long Solve()
        {
            HashSet<int> seen = new HashSet<int>();

            static long choose2(int n) => (n - 1) * n / 2;
            static long choose3(int n) => (n - 2) * (n - 1) * n / 6;

            long total = choose3(n);

            for (int i = 0; i < n; i++)
            {
                var root = FindRoot(i);
                if (!seen.Contains(root))
                {
                    seen.Add(root);
                    int s = size[root];
                    total -= choose2(s) * (n-s);
                    total -= choose3(s);
                }
            }
            return total;
        }



    }


    [TestClass]
    public class KunduAndTreeTests
    {
        [TestMethod]
        [DataRow("5\n1 2 b\n2 3 r\n3 4 r\n4 5 b", 4)]
        public void FromString(string input, long expected)
        {
            var parts = input.Split("\n");

            var n = int.Parse(parts[0]);
            var edges = new List<(int, int)>();
            for (int i = 1; i < n; i++)
            {
                var edge_parts = parts[i].Split(" ");
                
                if (edge_parts[2] == "r") continue;

                edges.Add(
                    (
                        int.Parse(edge_parts[0]) - 1,
                        int.Parse(edge_parts[1]) - 1
                    )
                );
            }

            long actual = new KunduAndTree(n, edges).Solve();

            Assert.AreEqual(expected, actual);
        }
    }
}
