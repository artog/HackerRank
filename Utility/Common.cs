using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HackerRank.Utility
{
    class Common
    {

        public static StreamReader ToStreamReader(string source) {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(source);
            writer.Flush();
            stream.Position = 0;

            return new StreamReader(stream);
        }

    }

    [TestClass]
    public class CommonTest {
        
        [TestMethod]
        [DataRow("a")]
        [DataRow("1111111111111111111111111111111111111111111111111111111111")]
        [DataRow("-1")]
        [DataRow(@"a#%""#%/%&(/""#¤%&!#¤&!""#¤%!""#¤""!%¤!#¤&""#¤&""#&¤")]
        public void SingleLineRead(string input) {

            var reader = Common.ToStreamReader(input);

            Assert.AreEqual(input, reader.ReadToEnd());

        }


        
        [TestMethod]
        [DataRow("a"      , 1, new string[] {"a"})]
        [DataRow("a\nb"   , 2, new string[] {"a", "b"})]
        [DataRow("a\nb\nc", 3, new string[] {"a", "b", "c"})]
        public void MultipleLineRead(string input, long rowCount, string[] rows) {

            var reader = Common.ToStreamReader(input);

            long count = 0;
            foreach (var row in rows) {
                Assert.AreEqual(row, reader.ReadLine());
                count++;
            }
            Assert.AreEqual(rowCount, count);
        }


    }
}
