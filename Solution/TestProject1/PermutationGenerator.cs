using System.Collections.Generic;

namespace TestProject1
{
   class PermutationGenerator
   {
      public PermutationGenerator(ICollection<int> p_InputSet)
      {
         m_WorkingArray = new int[p_InputSet.Count];
         p_InputSet.CopyTo(m_WorkingArray, 0);

         System.Array.Sort( m_WorkingArray );
         m_StartingSet = (int[])m_WorkingArray.Clone();
      }

      public int[] Array
      {
         get { return m_WorkingArray; }
      }

      public bool GenerateNext()
      {
         return ExecuteNarayanaAlgoritm();
      }

      public void Reset()
      {
         System.Array.Copy(m_StartingSet, Array, Array.Length);
      }

      private readonly int[] m_WorkingArray;
      private readonly int[] m_StartingSet;

      private void SwapArrayItems(  int p_Index1, int p_Index2 )
      {
         var t = Array[p_Index1];
         Array[p_Index1] = Array[p_Index2];
         Array[p_Index2] = t;
      }

      //алгоритм Нарайаны
      private bool ExecuteNarayanaAlgoritm( )
      {
         var length = Array.Length;

         var i = length - 2;
         while ((i >= 0) && (Array[i] >= Array[i + 1]))
            --i;
         if (i == -1)
            return false;

         var t = length - 1;
         while ((Array[i] >= Array[t]) && (t >= i + 1))
            --t;

         SwapArrayItems( i, t );


         int j = i + 1;
         while (j <= (length + i) / 2 )
         {
            t = length + i - j;
            SwapArrayItems( j, t );
            ++j;
         }

         return j != 0;
      }
   }
}
