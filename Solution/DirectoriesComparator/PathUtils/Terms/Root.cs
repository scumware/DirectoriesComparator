using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DirectoriesComparator.PathUtils.Terms
{
   static class Root
   {
      public const int Length = 1;

      /// <summary>
      /// Проверяет строку на наличие префикса '\'.
      /// В случае удачи возвращает собственную длинну, обратно - (-1)
      /// </summary>
      /// <param name="p_ValidatorContext"></param>
      /// <returns></returns>
      public static int TryParse(PathValidatorContext p_ValidatorContext)
      {
          var index = p_ValidatorContext.CurrentIndex;
          var str = p_ValidatorContext.ValidatedString;

          if ((index + Length) > str.Length)
          {
              return -1;
          }

          if (str[index] == '\\')
          {
              return Length;
          }

          return -1;
      }

      internal static bool ParseNext(PathValidatorContext p_ValidatorContext)
      {
         int length;
         if ((length = ValidName.TryParse( p_ValidatorContext )) > 0)
         {
            if (p_ValidatorContext.TryAddTerm( length ))
               if (p_ValidatorContext.TotalLength == p_ValidatorContext.ValidatedString.Length)
               {
                  return true;
               }
            return ValidName.ParseNext( p_ValidatorContext );
         }

         return length == 0;
      }
   }
}
