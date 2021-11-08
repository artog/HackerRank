using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace HackerRank.DataStructures.Stacks.BalancedBrackets
{

    class Result
    {

        /*
         * Complete the 'isBalanced' function below.
         *
         * The function is expected to return a STRING.
         * The function accepts STRING s as parameter.
         */
        private static readonly Dictionary<char, char> bracketMap = new Dictionary<char, char>() {
            {')', '('},
            {']', '['},
            {'}', '{'}
        };
        public static string IsBalanced(string s)
        {
            Stack<char> stack = new Stack<char>();


            foreach (var c in s)
            {
                switch (c)
                {
                    case '(':
                    case '[':
                    case '{':
                        stack.Push(c);
                        break;
                    case ')':
                    case ']':
                    case '}':
                        if (stack.Count > 0 && stack.Peek() == bracketMap[c]) {
                            stack.Pop();
                        } else {
                            return "NO";
                        }
                        break;
                    default:
                        throw new ArgumentException("String contains invalid characters");

                }
            }

            if (stack.Count > 0)
            {
                return "NO";
            }

            return "YES";
        }

    }

    [TestClass]
    public class BalancedBracketsTests
    {
        [TestMethod]
        [DataRow(      "}][}}(}][))]",  "NO")]
        [DataRow(          "[](){()}", "YES")]
        [DataRow(                "()", "YES")]
        [DataRow(    "({}([][]))[]()", "YES")]
        [DataRow("{)[](}]}]}))}(())(",  "NO")]
        [DataRow(              "([[)",  "NO")]
        public void Case0(string brackets, string expected)
        {
            Assert.AreEqual(expected, Result.IsBalanced(brackets)); 
        }
    }

}
/*






*/
