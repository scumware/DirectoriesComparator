using System.Collections.Generic;

namespace TestProject1
{
   public static class Utils
   {

      public static bool IsOrdered<T>(this IEnumerable<T> p_Target, IComparer<T> p_Comparer = null)
      {
         p_Comparer = p_Comparer ?? Comparer<T>.Default;
         var previousItem = default(T);
         var previousComparisonResult = 0;
         var firstPass = true;
         var result = true;
         foreach (var currentItem in p_Target)
         {
            if (firstPass)
            {
               firstPass = false;
               previousItem = currentItem;
               continue;
            }

            var comparisonResult = p_Comparer.Compare(currentItem, previousItem);
            comparisonResult = (comparisonResult < 0) ? -1
               : (comparisonResult > 0) ? 1
               : comparisonResult;

            if (comparisonResult != previousComparisonResult)
            {
               if ((previousComparisonResult != 0)
                  && (comparisonResult != 0))
               {
                     result = false;
                     break;
               }
               previousComparisonResult = comparisonResult;

               previousItem = currentItem;
            }
         }
         return result;
      }

      public static bool CompareIgnoringOrder<T>(ICollection<T> p_First, ICollection<T> p_Second)
      {
         if (p_First.Count != p_Second.Count)
         {
            return false;
         }

         if (p_First.Count == 0)
         {
            return true;
         }

         var second = FillDictionary( p_Second );
         var first = FillDictionary(p_First);

         if (first.Count != second.Count)
         {
            return false;
         }

         foreach (var firstPair in first)
         {
            int count;
            if (second.TryGetValue(firstPair.Key, out count))
            {
               if (count != firstPair.Value)
               {
                  return false;
               }
            }
            else
            {
               return false;
            }
         }

         return true;
      }

      private static Dictionary<T, int> FillDictionary<T>( IEnumerable<T> p_Source )
      {
         var first = new Dictionary<T, int>();
         foreach (var key in p_Source)
         {
            if (first.ContainsKey(key))
            {
               first[key] = ++first[key];
            }
            else
            {
               first.Add(key, 1);
            }
         }
         return first;
      }
   }
}
