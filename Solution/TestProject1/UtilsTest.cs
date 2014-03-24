using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestProject1
{
   [TestClass]
   public class UtilsTest
   {
      [TestMethod]
      public void IsSortedTest()
      {
         // ReSharper disable InvokeAsExtensionMethod
         int[] array;
         bool isSorted;

         array = new int[1];
         isSorted = Utils.IsOrdered( array, Comparer<int>.Default );
         Assert.IsTrue( isSorted );

         array = new int[2];
         isSorted = Utils.IsOrdered( array, Comparer<int>.Default );
         Assert.IsTrue( isSorted );

         array = new int[3];
         array[0] = 2;
         array[1] = 1;
         array[2] = 0;
         isSorted = Utils.IsOrdered( array, Comparer<int>.Default );
         Assert.IsTrue( isSorted );

         array[0] = 0;
         array[1] = 1;
         array[2] = 2;
         isSorted = Utils.IsOrdered( array, Comparer<int>.Default );
         Assert.IsTrue( isSorted );

         array[0] = 0;
         array[1] = 0;
         array[2] = 2;
         isSorted = Utils.IsOrdered( array, Comparer<int>.Default );
         Assert.IsTrue( isSorted );

         array[0] = 2;
         array[1] = 0;
         array[2] = 0;
         isSorted = Utils.IsOrdered( array, Comparer<int>.Default );
         Assert.IsTrue( isSorted );

         array[0] = 2;
         array[1] = 0;
         array[2] = 2;
         isSorted = Utils.IsOrdered( array, Comparer<int>.Default );
         Assert.IsFalse( isSorted );

         array[0] = 0;
         array[1] = 2;
         array[2] = 0;
         isSorted = Utils.IsOrdered( array, Comparer<int>.Default );
         Assert.IsFalse( isSorted );

         array = new int[4];
         array[0] = 3;
         array[1] = 2;
         array[2] = 1;
         array[3] = 0;
         isSorted = Utils.IsOrdered( array, Comparer<int>.Default );
         Assert.IsTrue( isSorted );

         array[0] = 0;
         array[1] = 1;
         array[2] = 2;
         array[3] = 3;
         isSorted = Utils.IsOrdered( array, Comparer<int>.Default );
         Assert.IsTrue( isSorted );

         array[0] = 0;
         array[1] = 1;
         array[2] = 1;
         array[3] = 3;
         isSorted = Utils.IsOrdered( array, Comparer<int>.Default );
         Assert.IsTrue( isSorted );

         array[0] = 3;
         array[1] = 1;
         array[2] = 1;
         array[3] = 0;
         isSorted = Utils.IsOrdered( array, Comparer<int>.Default );
         Assert.IsTrue( isSorted );

         array[0] = 1;
         array[1] = 1;
         array[2] = 1;
         array[3] = 0;
         isSorted = Utils.IsOrdered( array, Comparer<int>.Default );
         Assert.IsTrue( isSorted );

         array[0] = 0;
         array[1] = 1;
         array[2] = 1;
         array[3] = 1;
         isSorted = Utils.IsOrdered( array, Comparer<int>.Default );
         Assert.IsTrue( isSorted );

         array[0] = 1;
         array[1] = 0;
         array[2] = 1;
         array[3] = 1;
         isSorted = Utils.IsOrdered( array, Comparer<int>.Default );
         Assert.IsFalse( isSorted );

         for (int size = 5; size < 20; size++)
         {
            array = new int[size];
            var lastIndex = array.Length - 1;

            for (int i = 0; i < array.Length; i++)
            {
               array[i] = i;

               isSorted = Utils.IsOrdered( array, Comparer<int>.Default );
               if ((i == 0) || (i == lastIndex))
                  Assert.IsTrue( isSorted );
               else
                  Assert.IsFalse( isSorted );
            }

            array[lastIndex] = array[lastIndex - 1];
            isSorted = Utils.IsOrdered(array, Comparer<int>.Default);
            Assert.IsTrue( isSorted );

            array[0] = array[lastIndex];
            isSorted = Utils.IsOrdered( array, Comparer<int>.Default );
            Assert.IsFalse( isSorted );


            array = new int[size];
            for (int index = 0; index < array.Length; index++)
            {
               array[index] = array.Length-index;

               isSorted = Utils.IsOrdered( array, Comparer<int>.Default );
               if ((index == 0) || (index == lastIndex))
                  Assert.IsTrue( isSorted );
            }

            array[lastIndex] = array[lastIndex - 1];
            isSorted = Utils.IsOrdered( array, Comparer<int>.Default );
            Assert.IsTrue( isSorted );


            array = new int[size];
            array[lastIndex] = array.Length;
            isSorted = Utils.IsOrdered( array, Comparer<int>.Default );
            Assert.IsTrue( isSorted );
            array[lastIndex] = default(int);

            array[0] = array.Length;
            isSorted = Utils.IsOrdered( array, Comparer<int>.Default );
            Assert.IsTrue( isSorted );

            // ReSharper restore InvokeAsExtensionMethod
         }
      }
   }
}
