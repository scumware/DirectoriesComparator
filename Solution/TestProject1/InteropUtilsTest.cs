using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TestProject1
{
   [TestClass()]
   public class InteropUtilsTest
   {
      [TestMethod()]
      public void ConcatenateItegersTest()
      {
         var hiPart = (UInt32)0xffffffff;
         var lowPart = (UInt32)0xffffffff;
         var expected = (UInt64)0xffffffffffffffff;
         var actual = DirectoriesComparator.Win32.Utils.ConcatenateItegers( hiPart, lowPart );
         Assert.AreEqual( expected, (UInt64)actual );
      }
   }
}
