using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DirectoriesComparator.PathUtils
{
   public static class Utils
   {
      public static bool ContainedIn(this Char p_Char, params char[] p_Chars)
      {
         return p_Chars.Contains(p_Char);
      }

      public static bool ContainedIn(this Char p_Char, string p_String)
      {
         return p_String.Contains(p_Char);
      }

      public static char[] Without(this string p_String, params char[] p_Chars)
      {
         var size = p_String.Count(item => !p_Chars.Contains(item));
         var result = new char[size];

         var index = 0;
         foreach (var item in p_String.Where(item => !p_Chars.Contains(item)))
         {
            result[index] = item;
            ++index;
         }
         return result;
      }
   }
}
