using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using DirectoriesComparator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TestProject1
{
   /// <summary>
   ///This is a test class for SortedArray class and is intended
   ///to contain all SortedArray Unit Tests
   ///</summary>
   [TestClass]
   public class SortedArrayTest
   {
      private const int ConstructionTestMaximumArraySize = 10;

      [TestMethod]
      [Timeout(5 * 60 * 1000)]
      public void SortedArrayConstructionTest()
      {
         var sourceSet = new List<int>(ConstructionTestMaximumArraySize);

         //============- 1 -=================
         for (var size = 1; size <= ConstructionTestMaximumArraySize; size++)
         {
            sourceSet.Clear();
            FillSet( sourceSet, size, FillingSetType.WithBaseValue );

            var testSubject = CreateAndFillSortedArray(sourceSet);

            var resultArray = testSubject.GetAll().ToArray();
            Assert.AreEqual(sourceSet.Count, resultArray.Length);

            var setsAreEquivalent = Utils.CompareIgnoringOrder(sourceSet, resultArray);
            Assert.IsTrue(setsAreEquivalent);
         }

         //============- 2 -=================
         for (var size = 2; size <= ConstructionTestMaximumArraySize; size++)
         {
            sourceSet.Clear();
            FillSet( sourceSet, size );

            for (var partSize = 0; partSize <= size - 1; partSize++)
               for (int itemValue = 1; itemValue < size; itemValue++)
               {
                  FillSetPartialy(sourceSet, 0, partSize, itemValue);

                  TestAllPemutations(sourceSet);
               }
         }
      }

      [TestMethod]
      public void GetRangeTest()
      {
         var testSubject = new SortedArray<int>();
         for (int value = 0; value <= ConstructionTestMaximumArraySize; value++)
         {
            testSubject.Add(value);
         }

         var rangeStartValue = 3;
         var rangeEndValue = ConstructionTestMaximumArraySize - 1;

         var range = testSubject.GetRange(rangeStartValue, rangeEndValue).ToArray();

         for (int value = 0; value <= ConstructionTestMaximumArraySize; value++)
         {
            var valueContained = range.Contains(value);
            if ((rangeStartValue <= value) && (value <= rangeEndValue))
            {
               Assert.IsTrue(valueContained);
            }
            else
            {
               Assert.IsFalse(valueContained);
            }
         }


         testSubject = new SortedArray<int>();
         for (int value = 0; value <= ConstructionTestMaximumArraySize * 2; value = value + 2)
         {
            testSubject.Add(value);
         }


         range = testSubject.GetRange(rangeStartValue, rangeEndValue).ToArray();
         for (int value = 0; value <= ConstructionTestMaximumArraySize * 2; value = value + 2)
         {
            var valueContained = range.Contains(value);
            if ((rangeStartValue <= value) && (value <= rangeEndValue))
            {
               Assert.IsTrue(valueContained);
            }
            else
            {
               Assert.IsFalse(valueContained);
            }
         }


         {
            //В этих тестах мы должны получить пустое множество
            var stepSize = 4;
            testSubject = new SortedArray<int>( ConstructionTestMaximumArraySize * stepSize );
            int value;
            for (value = 0; value < testSubject.Capacity; value = value + stepSize)
            {
               testSubject.Add( value );
            }

            rangeStartValue = 0+1;
            rangeEndValue = stepSize - 1;

            range = testSubject.GetRange( rangeStartValue, rangeEndValue ).ToArray();
            Assert.AreEqual( 0, range.Length );



            rangeStartValue = 3;
            rangeEndValue = ConstructionTestMaximumArraySize - 1;

            value = -1;
            testSubject = new SortedArray<int>(ConstructionTestMaximumArraySize);
            for (int i = 0; i < ConstructionTestMaximumArraySize; ++i)
            {
               testSubject.Add(value);
            }
            range = testSubject.GetRange( rangeStartValue, rangeEndValue ).ToArray();
            Assert.AreEqual( 0, range.Length );


            value = ConstructionTestMaximumArraySize;
            testSubject = new SortedArray<int>( ConstructionTestMaximumArraySize );
            for (int i = 0; i < ConstructionTestMaximumArraySize; ++i)
            {
               testSubject.Add( value );
            }
            range = testSubject.GetRange(rangeStartValue, rangeEndValue).ToArray();
            Assert.AreEqual(0, range.Length);
         }
      }


      /// <summary>
      ///A test for GetEqualItemsGroups
      ///</summary>

      [TestMethod()]
      public void GetEqualItemsGroupsTest()
      {
         var sourceSize = ConstructionTestMaximumArraySize;
         var sourceSet = new List<int>( sourceSize );

         //============- 1 -=================
         FillSet( sourceSet, sourceSize, FillingSetType.WithBaseValue );

         var testSubject = CreateAndFillSortedArray( sourceSet );
         var equalItemsGroups = testSubject.GetEqualItemsGroups();

         Assert.AreEqual(1, equalItemsGroups.Count);

         var collection = equalItemsGroups.First();

         Assert.AreEqual( sourceSize, collection.Count );

         //============- 2 -=================
         sourceSet.Clear();
         FillSet( sourceSet, sourceSize, FillingSetType.Sequential );
         testSubject = CreateAndFillSortedArray( sourceSet );
         equalItemsGroups = testSubject.GetEqualItemsGroups();

         Assert.AreEqual( sourceSize, equalItemsGroups.Count );

         foreach (var group in equalItemsGroups)
         {
            Assert.AreEqual( 1, group.Count );
         }
      }


      [TestMethod]
      [Ignore]
      //Специальный тест.
      //Тестируем вставку элемементов в обратном порядке
      public void SortedArrayConstructionTestSpecialCases()
      {
         const int repeatItemCount = 2;

         var sourceSet = new List<int>(ConstructionTestMaximumArraySize * repeatItemCount);

         for (var itemNum = 1; itemNum < ConstructionTestMaximumArraySize; itemNum++)
         {
            sourceSet.Clear();
            var testSubject = new SortedArray<int>(itemNum);

            for (int item = itemNum; item > 0; --item)
               for (int i = 1; i <= repeatItemCount; i++)
               {
                  sourceSet.Add(item);

                  if (Debugger.IsAttached && i == repeatItemCount)
                  {
                     var leftSpace =
                           (int)
                           testSubject.GetType()
                                      .GetProperty("LeftSpace", BindingFlags.Instance | BindingFlags.NonPublic)
                                      .GetValue(testSubject, null);

                     if (leftSpace == 0)
                     {
                        var rightSpace =
                              (int)
                              testSubject.GetType()
                                         .GetProperty("RightSpace", BindingFlags.Instance | BindingFlags.NonPublic)
                                         .GetValue(testSubject, null);

                        var movingDistance = rightSpace / 2;
                        if (movingDistance > 1)
                        {
                           Debugger.Break();
                        }
                     }
                  }
                  testSubject.Add(item);

                  CheckConstructionResult(sourceSet, testSubject);
               }
         }
      }

      private static SortedArray<T> CreateAndFillSortedArray<T>(ICollection<T> p_SourceSet)
      {
         var testSubject = new SortedArray<T>(p_SourceSet.Count);
         foreach (var item in p_SourceSet)
         {
            testSubject.Add(item);
         }
         return testSubject;
      }

      [TestMethod]
      [Ignore]
      [Timeout(5 * 60 * 1000 )]
      public void ComplexityInverstigation()
      {
         const int startSetSize = 1024;
         const int maxSetSize = 128*1024;
         const int stepsCount = 20;

         const int maxMultiplier = maxSetSize / startSetSize;

         var set = new List<double>( maxSetSize );
         var rnd = new Random();
         var comparer = Comparer<double>.Default;

         foreach (FillingSetType fillingType in Enum.GetValues( typeof( FillingSetType ) ))
         {
            Console.WriteLine( "Inverstigation creation from {0} data", fillingType );

            var sizeMultiplier = (double)1;

            MesureStep( startSetSize, sizeMultiplier, set, rnd, fillingType, comparer);

            var multiplierStep = Math.Pow(maxMultiplier, 1 / (double) stepsCount);
            for (var step = 1; step <= (stepsCount-1); step++)
            {
               sizeMultiplier = Math.Pow(multiplierStep, (double) step);
               MesureStep(startSetSize, sizeMultiplier, set, rnd, fillingType, comparer);
            }

            set.Clear();
         }
      }

      private static void MesureStep(int p_StartSetSize, double p_SizeMultiplier, List<double> p_Set, Random p_Rnd,
                                     FillingSetType p_FillingType,
                                     Comparer<double> p_Comparer)
      {
         var setSize = (int) (p_StartSetSize * p_SizeMultiplier);

         FillSet(p_Set, setSize, p_Rnd, p_FillingType);

         var set = new double[setSize];
         for (int i = 0; i < setSize; i++)
         {
            var index = p_FillingType == FillingSetType.InterchangeMaxMinReversed ? (setSize - i - 1) : i;
            set[index] = p_Set[i];
         }

         double[] tmpArray = null;
         var quickSortTime = MeasureExceution
               (
                // ReSharper disable ImplicitlyCapturedClosure 
                () => tmpArray = (double[]) set.Clone(),
                () => Array.Sort(tmpArray, p_Comparer)
               // ReSharper restore ImplicitlyCapturedClosure
               );


         SortedArray<double> sortedArray = null;
         var bestTime = MeasureExceution( null, () => sortedArray = new SortedArray<double>( set, p_Comparer.Compare ) );
         var maxGroupLenght = GetMaximumGroupLenght(sortedArray);

         Console.WriteLine(
                           "set size = {0} \t\t\t maxGroupLength = {1} \t\t\t SortedArrayCreationTime = {2} \t\t\t QuickSortTime = {3}",
                           setSize, maxGroupLenght,
                           bestTime, quickSortTime);
         Console.Out.Flush();
      }


      private static long MeasureExceution( Action p_PrepareSubjectFunction, Action p_MeasureSubjectFunction, int p_CountMesaures = 6 )
      {
         var sw = new Stopwatch();
         var minTime = long.MaxValue;
         for (var mesureNumber = 1; mesureNumber <= p_CountMesaures; mesureNumber++)
         {
            if (p_PrepareSubjectFunction != null)
            {
               p_PrepareSubjectFunction();
            }
            sw.Start();
            p_MeasureSubjectFunction();
            sw.Stop();

            minTime = Math.Min( minTime, sw.ElapsedTicks );
            sw.Reset();
         }
         return minTime;
      }

      private static void FillSet(IList<double> p_TargetSet, int p_TargetSetSize, Random p_Rnd,
                                  FillingSetType p_FillingType = FillingSetType.Shuffled)
      {
         Func<int, double> generateValueFunction;

         double baseVaule;
         switch (p_FillingType)
         {
            case FillingSetType.Shuffled:
               generateValueFunction = p_Index => p_Rnd.NextDouble();
               break;

            case FillingSetType.Sequential:
               baseVaule = p_TargetSet.Count == 0 ? p_Rnd.NextDouble() : p_TargetSet[p_TargetSet.Count - 1];
               generateValueFunction = p_Index => baseVaule * p_Index;
               break;

            case FillingSetType.WithBaseValue:
               baseVaule = p_TargetSet.Count == 0 ? p_Rnd.NextDouble() : p_TargetSet[p_TargetSet.Count - 1];
               generateValueFunction = p_Index => baseVaule;
               break;


            case FillingSetType.InterchangeMaxMin:
            case FillingSetType.InterchangeMaxMinReversed:
               double maxValue, minValue, epsilon;
               if (p_TargetSet.Count < 2)
               {
                  do
                  {
                     baseVaule = p_Rnd.NextDouble();
                     var tmp = p_Rnd.NextDouble();

                     minValue = Math.Min(baseVaule, tmp);
                     maxValue = Math.Max(baseVaule, tmp);

                     epsilon = maxValue - minValue;
                  } while (double.Epsilon > epsilon);
               }
               else
               {
                  minValue = Math.Min(p_TargetSet[p_TargetSet.Count - 1], p_TargetSet[p_TargetSet.Count - 2]);
                  maxValue = Math.Max(p_TargetSet[p_TargetSet.Count - 1], p_TargetSet[p_TargetSet.Count - 2]);
                  epsilon = 2 * (maxValue - minValue) / p_TargetSet.Count;
               }
               generateValueFunction = p_Index => 0 != p_Index % 2
                                                        ? minValue - (epsilon * p_Index)
                                                        : maxValue + (epsilon * p_Index);

               break;
            default:
               throw new ArgumentOutOfRangeException("p_FillingType");
         }


         var startedSize = p_TargetSet.Count;
         for (var i = startedSize; i < p_TargetSetSize; i++)
         {
            var value = generateValueFunction(i);
            p_TargetSet.Add(value);
         }
      }

      private static void FillSet( IList<int> p_TargetSet, int p_Count, FillingSetType p_FillingType = FillingSetType.Sequential,
                                    int p_BaseValue = 1)
      {
         for (var i = 0; i < p_Count; i++)
         {
            switch (p_FillingType)
            {
               case FillingSetType.Sequential:
                  p_TargetSet.Add(i + p_BaseValue);
                  break;

               case FillingSetType.WithBaseValue:
                  p_TargetSet.Add(p_BaseValue);
                  break;

               default:
                  throw new ArgumentOutOfRangeException("p_FillingType");
            }
         }
      }

      private static int GetMaximumGroupLenght(SortedArray<double> p_SortedArray)
      {
         var firstPass = true;
         var maxGroupLenght = 1;
         var groupLength = 1;
         double previousValue = 1;
         foreach (var value in p_SortedArray.GetAll())
         {
            if (firstPass)
            {
               firstPass = false;
               previousValue = value;
               continue;
            }
            if (value == previousValue)
            {
               ++groupLength;
               continue;
            }
            else
            {
               previousValue = value;
               maxGroupLenght = Math.Max(maxGroupLenght, groupLength);
               groupLength = 1;
            }
         }
         maxGroupLenght = Math.Max( maxGroupLenght, groupLength );
         return maxGroupLenght;
      }

      private static void TestAllPemutations(ICollection<int> p_SourceArray)
      {
         var arrayUtils = new PermutationGenerator(p_SourceArray);
         var permutationNum = 0;
         do
         {
            try
            {
               ICollection<int> sourceArray = arrayUtils.Array;
               CheckConstructionResult(sourceArray, CreateAndFillSortedArray( sourceArray ));
            }
            catch
            {
               Console.WriteLine("Test failed on array size = {0} and permutation num = {1}", p_SourceArray.Count,
                                 permutationNum);
               throw;
            }
            ++permutationNum;
         } while (arrayUtils.GenerateNext());
      }

      private static void CheckConstructionResult(ICollection<int> p_SourceSet, SortedArray<int> p_TestSubject)
      {
         var resultArray = p_TestSubject.GetAll().ToArray();
         Assert.AreEqual(p_SourceSet.Count, resultArray.Length,
                         "Result array length not equal to source array length");

         var setsAreEquivalent = Utils.CompareIgnoringOrder(p_SourceSet, resultArray);
         Assert.IsTrue(setsAreEquivalent, "Result array is'nt equivalent to source array");

         var isOrdered = resultArray.IsOrdered();
         if ((Debugger.IsAttached) && !isOrdered)
         {
            Debugger.Break();
            p_TestSubject = CreateAndFillSortedArray( p_SourceSet );
         }
         Assert.IsTrue(isOrdered, "Result array is'nt ordered");

      }

      private enum FillingSetType
      {
         WithBaseValue,
         InterchangeMaxMin,
         InterchangeMaxMinReversed,
         Shuffled,
         Sequential
      }

      private static void FillSetPartialy(IList<int> p_InputArray, int p_FirstIndex, int p_LastIndex, int p_BaseValue)
      {
         for (var i = p_FirstIndex; i <= p_LastIndex; i++)
         {
            p_InputArray[i] = p_BaseValue;
         }
      }
   }
}
