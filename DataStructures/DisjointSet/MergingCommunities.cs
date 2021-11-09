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

    public class Community {

        public int Size {
            get => Parent?.Size ?? size;
            set => size = value;
        } 

        private int size = 1;

        public Community Parent;
        

        public void Merge(Community other) {
            
            size += other.Size;
            
            other.Parent = this;


        }

    }

    public class Person {

        public Community Community;

    }


    public class MergingCommunities
    {
        


        public static List<int> Solve(StreamReader input) {
            var parts = input
                .ReadLine()
                .Trim()
                .Split(' ');
            var n = int.Parse(parts[0]);
            var q = int.Parse(parts[1]);

            var communities = new Community[n];
            var people = new Person[n];


            for (int i = 0; i < n; i++) {
                people[i] = new Person();
                communities[i] = new Community();
                
                people[i].Community = communities[i];
                
            }

            var output = new List<int>();
            for (var i = 0; i < q; i++) {
                var raw = input
                    .ReadLine()
                    .Trim()
                    .Split(' ');


                if (raw[0] == "Q") {
                    var index = int.Parse(raw[1]) - 1;
                    output.Add(people[index].Community.Size);
                }
                else {
                    var a = int.Parse(raw[1]) - 1;
                    var b = int.Parse(raw[2]) - 1;


                    if (people[a].Community.Size > people[b].Community.Size) {
                        people[a].Community.Merge(people[b].Community);
                    }
                    else {
                        people[b].Community.Merge(people[a].Community);
                    }
                }
            }

            return output;

        }

    }


    [TestClass]
    public class MergingCommunitiesTests {

        [TestMethod]
        [DataRow("3 5\nM 0 1\nM 1 2\nQ 0\nQ 1\nQ 2", new[] { 3, 3, 3 })]
        public void TestSolveFromString(string input, int[] expected) {
            var actual = MergingCommunities.Solve(input.ToStreamReader());

            Assert.AreEqual(expected.Length, actual.Count);

            for (int i = 0; i < expected.Length; i++) {
                Assert.AreEqual(expected[i], actual[i]);
            }
        }

        [TestMethod]
        [DataRow(@"DataStructures\DisjointSet\Tests", "MergingCommunities_Case1.txt")]
        public void TestSolveFromFile(string baseLocation, string filename) {

            var inputFile = Path.Combine(baseLocation, "Input", filename);
            var outputFile = Path.Combine(baseLocation, "Output", filename);

            var actual = MergingCommunities.Solve(
                new StreamReader(new FileStream(inputFile, FileMode.Open))
            );


            var expected =
                new StreamReader(new FileStream(outputFile, FileMode.Open))
                    .ReadToEnd()
                    .Split('\n');

            Assert.AreEqual(expected.Length, actual.Count);

            for (int i = 0; i < expected.Length; i++) {
                Assert.AreEqual(expected[i], actual[i].ToString());
            }
        }

    }
}
