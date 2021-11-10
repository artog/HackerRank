using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HackerRank.Utility;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HackerRank.DataStructures.DisjointSet
{
    public class MergingCommunities
    {

        private int[] size;
        private int[] people;
        private readonly int n;
        private readonly int q;
        private readonly List<string> queries;

        public int FindRoot(int start)
        {
            while (people[start] != start)
            {
                start = people[start];
            }
            return start;
        }

        public MergingCommunities(int n, int q, List<string> queries)
        {
            this.n = n;
            this.q = q;
            this.queries = queries;


            size = new int[n];
            people = new int[n];

            for (int i = 0; i < n; i++)
            {
                people[i] = i;
                size[i] = 1;
            }
        }

        public List<int> Solve() {

            var output = new List<int>();
            foreach (var query in queries) {

                var raw = query.Split(' ');

                if (raw[0] == "Q") {
                    var index = int.Parse(raw[1]) - 1;
                    output.Add(size[FindRoot(index)]);
                }
                else {
                    var a = int.Parse(raw[1]) - 1;
                    var b = int.Parse(raw[2]) - 1;

                    
                    int rootA = FindRoot(a);
                    int rootB = FindRoot(b);

                    if (a == b || rootA == rootB) continue;

                    if (size[rootA] > size[rootB]) {
                        size[rootA] += size[rootB];
                        people[rootB] = rootA;
                    }
                    else {
                        size[rootB] += size[rootA];
                        people[rootA] = rootB;
                        
                    }
                }
            }

            return output;

        }

        

    }


    [TestClass]
    public class MergingCommunitiesTests {

        private (int n, int q, List<string> queries) ParseInput(StreamReader input)
        {
            var parts = input
                .ReadLine()
                .Trim()
                .Split(' ');

            var n = int.Parse(parts[0]);
            var q = int.Parse(parts[1]);

            var queries = new List<string>();
            for (var i = 0; i < q; i++)
            {
                queries.Add(
                    input.ReadLine().Trim()
                );
            }

            return (n, q, queries);
        }


        [TestMethod]
        [DataRow("3 5\nM 1 2\nM 2 3\nQ 1\nQ 2\nQ 3", new[] { 3, 3, 3 })]
        [DataRow("3 6\nQ 1\nM 1 2\nQ 2\nM 2 3\nQ 3\nQ 2", new[] { 1,2,3,3 })]
        [DataRow("3 4\nM 1 2\nM 2 3\nM 3 3\nQ 3", new[] { 3 })]
        [DataRow("3 5\nM 1 2\nM 2 3\nQ 1\nM 1 3\nQ 3", new[] { 3, 3 })]
        public void TestSolveFromString(string input, int[] expected) {


            (int n, int q, List<string> queries) = ParseInput(input.ToStreamReader());

            var actual = new MergingCommunities(n, q, queries).Solve();

            Assert.AreEqual(expected.Length, actual.Count);

            for (int i = 0; i < expected.Length; i++) {
                Assert.AreEqual(expected[i], actual[i]);
            }
        }

        [TestMethod]
        [DataRow(@"DataStructures\DisjointSet\Tests", "MergingCommunities_Case1.txt")]
        [DataRow(@"DataStructures\DisjointSet\Tests", "MergingCommunities_Case2.txt")]
        [DataRow(@"DataStructures\DisjointSet\Tests", "MergingCommunities_Case7.txt")]
        public void TestSolveFromFile(string baseLocation, string filename) {

            var inputFile = Path.Combine(baseLocation, "Input", filename);
            var outputFile = Path.Combine(baseLocation, "Output", filename);

            var input = new StreamReader(new FileStream(inputFile, FileMode.Open));



            (int n, int q, List<string> queries) = ParseInput(input);

            var actual = new MergingCommunities(n, q, queries).Solve();


            var expected =
                new StreamReader(new FileStream(outputFile, FileMode.Open))
                    .ReadToEnd()
                    .Split("\r\n");

            var prints = queries
                .Select((e,i) => (e,i))
                .Where(q => q.e.StartsWith("Q"))
                .ToList();

            Assert.AreEqual(expected.Length, actual.Count);

            for (int i = 0; i < expected.Length; i++) {
                Assert.AreEqual(expected[i], actual[i].ToString(), $"Check failed for query #{i} [{prints[i]}] ");
            }
        }

    }
}
