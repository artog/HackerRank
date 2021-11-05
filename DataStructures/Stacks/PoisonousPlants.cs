using System.Transactions;

using HackerRank.Utility;

using Microsoft.VisualBasic.CompilerServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HackerRank.DataStructures.Stacks
{

    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.Collections;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.Serialization;
    using System.Text.RegularExpressions;
    using System.Text;
    using System;

    public class Node<T> {
        public T Value { get; set; }
        public Node<T> Next { get; set; }
        public Node<T> Prev { get; set; }

        public Node(T value) {
            Value = value;
        }

        public override string ToString() {
            return Value?.ToString();
        }

    }

    public class LinkedList<T> {
        
        public Node<T> First { get; set; }
        public Node<T> Last { get; set; }
        
        public LinkedList(T firstValue) {
            First = new Node<T>(firstValue);   
            Last = First;     
        }

        public LinkedList() { }

        public static LinkedList<T> From(T[] source) {
            if (source == null) {
                throw new ArgumentException("source cannot be null");
            }
            

            var list = new LinkedList<T>();


            foreach (var value in source) {
                list.Add(value);
            }

            return list;
        }

        public void Add(T value) {
            if (First == null) {
                First = new Node<T>(value);
                Last = First;
            }
            else {
                Last.Next = new Node<T>(value) {
                    Prev = Last
                };
                Last = Last.Next;
            }
        }
        
        public void Merge(LinkedList<T> other) {
            if (First == null) {
                First = other.First;
                Last = other.Last;
            }
            else if (other.First == null) {
                // Nothing to do there, other list is empty
            }
            else {
                Last.Next = other.First;
                other.First.Prev = Last;
                Last = other.Last;
            }
        }
        
        
        public override string ToString() {
            var s = new StringBuilder();
            
            s.Append("[");
            var current = First;
            while (current != null) {
                if (current != First) {
                    s.Append(", ");
                }
                s.Append(current.Value.ToString());
                current = current.Next;
            }
            
            s.Append("]");
            
            return s.ToString();
        }

        public string ShowLinks() {

            
            var s = new StringBuilder();
            
            s.Append("Forward: [ ");
            var current = First;
            while (current != null) {
                if (current != First) {
                    s.Append(" ==> ");
                }
                s.Append(current.Value);
                current = current.Next;
            }
            
            s.Append(" ] Backward: [ ");


            var backwards = new StringBuilder();
            current = Last;
            while (current != null) {
                backwards.Insert(0, current.Value);
                if (current != First) {
                    backwards.Insert(0, " <== ");
                }
                current = current.Prev;
            }

            s.Append(backwards.ToString());
            s.Append(" ]");

            return s.ToString();

        }


        public T Pop() {
            if (First == null) {
                throw new ArgumentException("Cant pop from empty list");
            }

            var value = First.Value;
            First = First.Next;
            if (First == null) Last = null;

            return value;
        }

    }


    public class Result
    {

        /*
         * Complete the 'poisonousPlants' function below.
         *
         * The function is expected to return an INTEGER.
         * The function accepts INTEGER_ARRAY p as parameter.
         */

        public static void MergeWithNext(Node<LinkedList<long>> current)
        {
            var source = current.Value;
            var next = current.Next.Value;
            var newNext = current.Next.Next;
            
            source.Merge(next);

            current.Value = source;
            current.Next = newNext;


            if (newNext != null) {
                newNext.Prev = current;
            }

        }

        public static long PoisonousPlants(List<long> p)
        {
            var parts = new LinkedList<LinkedList<long>>(
                new LinkedList<long>(p[0])
            );
                    
            for (var i = 1; i < p.Count; i++) {
                if (p[i-1] < p[i]) {
                    parts.Add(new LinkedList<long>(p[i]));
                } else {
                    parts.Last.Value.Add(p[i]);
                }
            }
            
            

            var days = 0;
            while (parts.Last.Prev != null) {

                var current = parts.Last.Prev;
                while (current != null) {
                    if (current.Value.Last.Value < current.Next.Value.First.Value) {
                        current.Next.Value.Pop();
                    }
                    current = current.Prev;
                }
                current = parts.Last.Prev;
                while (current != null) {
                    
                    // Check if we can merge the next with current
                    if (current.Value.First == null || current.Next.Value.First == null || current.Value.Last.Value >= current.Next.Value.First.Value) 
                    {
                        // Special case. If we merge the second to last with
                        // the last, make sure to update the last of the main list.
                        if (current.Next == parts.Last) {
                            parts.Last = current;
                        }
                        MergeWithNext(current);
                    } 
                    current = current.Prev;
                }
                    
                days++;
            }

            return days;
        }

    }

    public class Solution
    {
        public static void Main_(string[] args)
        {
            TextWriter textWriter = new StreamWriter(@System.Environment.GetEnvironmentVariable("OUTPUT_PATH"), true);

            var result = Run(new StreamReader(Console.OpenStandardInput()));

            textWriter.WriteLine(result);

            textWriter.Flush();
            textWriter.Close();
        }


        public static string Run(StreamReader input) {

            var _ = Convert.ToInt32(input.ReadLine()?.Trim());

            var p = input
                .ReadLine()?
                .TrimEnd()
                .Split(' ')
                .ToList()
                .Select(pTemp => Convert.ToInt64(pTemp))
                .ToList();

            var result = Result.PoisonousPlants(p);

            return result.ToString();

        }
    }





    [TestClass]
    public class PoisonousPlantsTests {

        public void AssertLinkedListEqualToArray<T>(LinkedList<T> list, T[] source) {
            var current = list.First;
            var index = 0;
            while (current != null) {
                Assert.AreEqual(source[index++], current.Value, $"List does not match on index [{index-1}]. Expected [{source[index-1]}], Actual [{current.Value}]");
                current = current.Next;
            }

            Assert.AreEqual(source.Length, index);
        }



        [TestMethod]
        [DataRow("7\n6 5 8 4 7 10 9",                                "2", DisplayName = "Test Case 0")]
        [DataRow("4\n3 2 5 4",                                       "2", DisplayName = "Test Case 1")]
        [DataRow("7\n4 3 7 5 6 4 2",                                 "3", DisplayName = "Test Case 2")]
        [DataRow("6\n4 5 3 2 1 6",                                   "1", DisplayName = "Test Case 5")]
        [DataRow("17\n20 5 6 15 2 2 17 2 11 5 14 5 10 9 19 12 5",    "4", DisplayName = "Test Case 8")]
        public void VerifyString(string input, string expectedOutput) {

            var actual = Solution.Run(
                Common.ToStreamReader(input)
            );

            Assert.AreEqual(expectedOutput, actual);


        }

        [TestMethod]
        [DataRow("DataStructures/Stacks/Case27.txt", "49999", DisplayName = "Test Case 27")]
        public void VerifyFile(string fileName, string expectedOutput) {

            var actual = Solution.Run(new StreamReader(new FileStream(fileName, FileMode.Open)));

            Assert.AreEqual(expectedOutput, actual);


        }

        [TestMethod]
        [DataRow(new long[] {})]
        [DataRow(new long[] {1})]
        [DataRow(new long[] {1,2})]
        [DataRow(new long[] {1,3,2})]
        [DataRow(new long[] {1,0,-1,-2,-3,321654,879854})]
        public void BuildList(long[] source) {


            var list = LinkedList<long>.From(source);

            AssertLinkedListEqualToArray(list, source);

        }


        [TestMethod]
        [DataRow( new long[]          {}, new long[]          {}, new long[]                    {})]
        [DataRow( new long[]          {}, new long[]       {1,2}, new long[]                 {1,2})]
        [DataRow( new long[]       {1,2}, new long[]          {}, new long[]                 {1,2})]
        [DataRow( new long[]         {1}, new long[]         {2}, new long[]                 {1,2})]
        [DataRow( new long[]       {1,2}, new long[]         {3}, new long[]               {1,2,3})]
        [DataRow( new long[]         {1}, new long[]       {2,3}, new long[]               {1,2,3})]
        [DataRow( new long[]       {1,2}, new long[]       {3,4}, new long[]             {1,2,3,4})]
        [DataRow( new long[] {9,8,7,6,5}, new long[] {0,1,2,3,4}, new long[] {9,8,7,6,5,0,1,2,3,4})]
        public void MergeTest(long[] first, long[] second, long[] result) {

            var list = LinkedList<long>.From(first);

            list.Merge(LinkedList<long>.From(second));

            AssertLinkedListEqualToArray(list, result);

        }

        [TestMethod]
        public void NodeMergeTest() {

            Node<LinkedList<long>> first = new Node<LinkedList<long>>(
                LinkedList<long>.From(new long[] { 1, 2 })
            );


            first.Next = new Node<LinkedList<long>>(
                LinkedList<long>.From(new long[] { 3, 4 })
            );


            first.Next.Next = new Node<LinkedList<long>>(
                LinkedList<long>.From(new long[] { 5 ,6 })
            );

            Result.MergeWithNext(first);

            AssertLinkedListEqualToArray(first.Value, new long[] {1,2,3,4});

            Assert.AreEqual(first.Value.First.Value, 1);
            Assert.AreEqual(first.Value.First.Next.Value, 2);
            Assert.AreEqual(first.Value.First.Next.Next.Value, 3);
            Assert.AreEqual(first.Value.First.Next.Next.Next.Value, 4);
            Assert.AreEqual(first.Value.Last.Prev.Prev.Prev.Value, 1);
            Assert.AreEqual(first.Value.Last.Prev.Prev.Value, 2);
            Assert.AreEqual(first.Value.Last.Prev.Value, 3);
            Assert.AreEqual(first.Value.Last.Value, 4);

            Assert.IsNotNull(first.Next);

            AssertLinkedListEqualToArray(first.Next.Value, new long[] {5,6});
            
            Assert.AreEqual(first.Next.Value.First.Value, 5);
            Assert.AreEqual(first.Next.Value.First.Next.Value, 6);
            Assert.AreEqual(first.Next.Value.Last.Prev.Value, 5);
            Assert.AreEqual(first.Next.Value.Last.Value, 6);

            Assert.IsNull(first.Next.Next);
        }


        [TestMethod]                       
        [DataRow(new long[] { 1, 2, 3 },  1L,    3L, new long[] {2,3})]
        [DataRow(new long[]    { 2, 3 },  2L,    3L, new long[] {3})]
        [DataRow(new long[]       { 3 },  3L,  null, new long[] {})]
        [DataRow(new long[]   { -1, 1 }, -1L,    1L, new long[] {1})]
        public void PopTest(long[] source, long poppedValue, long? lastValue, long[] resultingList) {
            var list = LinkedList<long>.From(source);

            AssertLinkedListEqualToArray(list, source);

            Assert.AreEqual(poppedValue, list.Pop());

            Assert.AreEqual(lastValue, list.Last?.Value);


            
            AssertLinkedListEqualToArray(list, resultingList);
        }

        [TestMethod]
        [DataRow(new long[] {1,2,3,4,5})]
        [DataRow(new long[] {1})]
        [DataRow(new long[] {})]
        [DataRow(new long[] {9,8,7,6,5,4,3,2,1,0,-1})]
        public void ForwardIteration(long[] numbers) {
            var list = LinkedList<long>.From(numbers);

            var index = 0;
            var current = list.First;


            while (current != null) {
                Assert.AreEqual(numbers[index++], current.Value);
                current = current.Next;
            }
        }

        

        [TestMethod]
        [DataRow(new long[] {1,2,3,4,5})]
        [DataRow(new long[] {1})]
        [DataRow(new long[] {})]
        [DataRow(new long[] {9,8,7,6,5,4,3,2,1,0,-1})]
        public void BackwardIteration(long[] numbers) {
            var list = LinkedList<long>.From(numbers);

            var index = numbers.Length - 1;
            var current = list.Last;


            while (current != null) {
                Assert.AreEqual(numbers[index--], current.Value);
                current = current.Prev;
            }
        }


        

        [TestMethod]
        [DataRow( new long[]          {}, new long[]          {}, new long[]                    {})]
        [DataRow( new long[]          {}, new long[]       {1,2}, new long[]                 {1,2})]
        [DataRow( new long[]       {1,2}, new long[]          {}, new long[]                 {1,2})]
        [DataRow( new long[]         {1}, new long[]         {2}, new long[]                 {1,2})]
        [DataRow( new long[]       {1,2}, new long[]         {3}, new long[]               {1,2,3})]
        [DataRow( new long[]         {1}, new long[]       {2,3}, new long[]               {1,2,3})]
        [DataRow( new long[]       {1,2}, new long[]       {3,4}, new long[]             {1,2,3,4})]
        [DataRow( new long[] {9,8,7,6,5}, new long[] {0,1,2,3,4}, new long[] {9,8,7,6,5,0,1,2,3,4})]
        public void IterationAfterMerge(long[] first, long[] second, long [] expected) {

            var list = LinkedList<long>.From(first);
            list.Merge(LinkedList<long>.From(second));


            
            var index = 0;
            var current = list.First;


            while (current != null) {
                Assert.AreEqual(expected[index++], current.Value);
                current = current.Next;
            }


            index = expected.Length - 1;
            current = list.Last;


            while (current != null) {
                Assert.AreEqual(expected[index--], current.Value);
                current = current.Prev;
            }
        }

        [TestMethod]
        public void AdvancedMergeAndIteration() {

            var list = new LinkedList<long>(1);

            list.Merge(new LinkedList<long>(2));
            list.Merge(new LinkedList<long>());

            var listB = new LinkedList<long>(3);
            listB.Add(3);
            listB.Add(4);
            listB.Merge(new LinkedList<long>());
            listB.Merge(new LinkedList<long>(5));

            listB.Merge(new LinkedList<long>());
            listB.Merge(new LinkedList<long>());
            listB.Merge(new LinkedList<long>());
            listB.Merge(new LinkedList<long>());

            var listC = new LinkedList<long>(6);
            listC.Add(7);

            listB.Merge(listC);

            list.Merge(new LinkedList<long>());
            list.Merge(new LinkedList<long>());
            list.Merge(new LinkedList<long>());
            listB.Pop();

            list.Merge(listB);

            var listD = new LinkedList<long>(456);
            listD.Pop();
            listD.Merge(new LinkedList<long>(123));
            listD.Pop();
            list.Merge(listD);

            list.Merge(new LinkedList<long>(8));
            list.Merge(new LinkedList<long>(9));

            list.Merge(new LinkedList<long>());
            list.Merge(new LinkedList<long>());
            list.Merge(new LinkedList<long>());
            list.Merge(new LinkedList<long>());


            var expected = new long[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            

            Assert.AreEqual("[1, 2, 3, 4, 5, 6, 7, 8, 9]", list.ToString());

            var index = 0;
            var current = list.First;

            while (current != null) {
                Assert.AreEqual(expected[index++], current.Value);
                current = current.Next;
            }
            

            index = expected.Length - 1;
            current = list.Last;


            while (current != null) {
                Assert.AreEqual(expected[index--], current.Value);
                current = current.Prev;
            }
            
        }

        [TestMethod]
        public void TestMergeWithNext() {

            var lists = new LinkedList<LinkedList<long>>();
            lists.Add(new LinkedList<long>(1));
            lists.Add(new LinkedList<long>(2));
            lists.Add(new LinkedList<long>(3));
            var listA = new LinkedList<long>(4);
            listA.Add(5);
            lists.Add(listA);
            
            var listB = new LinkedList<long>(6);
            listB.Add(7);
            lists.Add(listB);
            
            var listC = new LinkedList<long>(8);
            listC.Add(9);
            lists.Add(listC);

            lists.Add(new LinkedList<long>(10));

            var currentList = lists.First;
            while (currentList.Next != null) {
                Result.MergeWithNext(currentList);
            }

            
            var expected = new long[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            Assert.AreEqual("[1, 2, 3, 4, 5, 6, 7, 8, 9, 10]", lists.First.ToString());

            var index = 0;
            var current = lists.First.Value.First;

            while (current != null) {
                Assert.AreEqual(expected[index++], current.Value);
                current = current.Next;
            }
            

            index = expected.Length - 1;
            current = lists.Last.Value.Last;


            while (current != null) {
                Assert.AreEqual(expected[index--], current.Value);
                current = current.Prev;
            }

        }


    }

}
