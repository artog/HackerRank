using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.IO;
using System.Threading.Tasks;

using HackerRank.Utility.Collections;

using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace HackerRank.DataStructures.Queue
{

    public class Grid : Dictionary<Point, char> {

        public new char this[Point key] {
            get {
                if (TryGetValue(key, out char c))
                {
                    return c;
                }

                return 'X';

            }
            set => base[key] =  value;
        }


        public static Grid FromList(List<string> rows) {
            var grid = new Grid();
            int y = 0;
            foreach (var row in rows) {
                int x = 0;


                foreach (var cell in row) {
                    grid[new Point(x, y)] = cell;
                    x++;
                }

                y++;

            }

            return grid;
        }

    }


    public class Point {

        public int X, Y;

        public Point(int x, int y) {
            X = x;
            Y = y;
        }

        public override bool Equals(object? obj) {
            if (obj == null || !(obj is Point that) ) return false;


            return that.X == this.X && that.Y == this.Y;
        }

        public override int GetHashCode() { return X.GetHashCode() + Y.GetHashCode(); }

        public static bool operator ==(Point a, Point b) => a?.Equals(b) ?? false;
        
        public static bool operator !=(Point a, Point b) => !(a == b);

        public static Point operator +(Point a, Point b) => new Point(a.X + b.X, a.Y + b.Y);

        public Point Copy() => new Point(X, Y);

        public override string ToString() { return $"{X},{Y}"; }

    }

    public class Target {

        public int Steps;
        public Point Coordinate;

    }

    public class CastleOnTheGrid {

        public static int Solve(Grid grid, Point start, Point goal) {
            var seen = new HashSet<Point>();
            var queue = new Heap<Target>((t1, t2) => t1.Steps > t2.Steps);

            queue.Add(new Target() {
                Coordinate = start,
                Steps = 0
            });

            while (queue.Size > 0) {
                var current = queue.Pop();


                if (current.Coordinate == goal) {
                    return current.Steps;
                }
                
                foreach (var target in FindValidTargets(grid, current.Coordinate)) 
                {
                    if (!seen.Contains(target)) {
                        queue.Add(new Target() {
                            Coordinate = target,
                            Steps = current.Steps + 1
                        });
                        seen.Add(target);
                    }
                }


            }

            return -1;
        }

        private static IEnumerable<Point> FindValidTargets(Grid grid, Point pos) {
            var directions = new [] {
                new Point(0, 1),
                new Point(0, -1),
                new Point(1, 0),
                new Point(-1, 0),
            };

            
            foreach (var d in directions) {
                var target = pos.Copy();
                while (grid[target + d] != 'X') {
                    target += d;

                    yield return target;
                }
            }
        }


    }

    [TestClass]
    public class CastleOnTheGridTests {


        [TestMethod]
        [DataRow(".X.\n.X.\n...", 0, 0, 0, 2, 3)]
        [DataRow(".X..XX...X\nX.........\n.X.......X\n..........\n........X.\n.X...XXX..\n.....X..XX\n.....X.X..\n..........\n.....X..XX", 9, 1, 9, 6, 3)]
        public void TestBfs(
            string source,
            int startY,
            int startX,
            int goalY,
            int goalX,
            int expected
        ) {

            var grid = Grid.FromList(source.Split('\n').ToList());

            int actual = CastleOnTheGrid.Solve(grid, new Point(startX, startY), new Point(goalX, goalY));
            Assert.AreEqual(expected, actual);
        }



        [TestMethod]
        [DataRow(@"DataStructures\Queue\CastleOnTheGrid_Case3.txt", 16, DisplayName = "Test Case 3")]
        public void TestBfsFromFile(
            string filename,
            int expected
        ) {


            var rows = new List<string>();

            using var stream = new StreamReader(new FileStream(filename, FileMode.Open));


            var n = int.Parse(
                stream.ReadLine().Trim()
            );
            for (int i = 0; i < n; i++) {
                rows.Add(stream.ReadLine());
            }

            string[] firstMultipleInput = stream.ReadLine().Trim().Split(' ');

            var startX = Convert.ToInt32(firstMultipleInput[0]);

            var startY = Convert.ToInt32(firstMultipleInput[1]);

            var goalX = Convert.ToInt32(firstMultipleInput[2]);

            var goalY = Convert.ToInt32(firstMultipleInput[3]);

            var grid = Grid.FromList(rows);

            int actual = CastleOnTheGrid.Solve(grid, new Point(startX, startY), new Point(goalX, goalY));
            Assert.AreEqual(expected, actual);
        }


    }
}
