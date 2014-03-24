using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DirectoriesComparator.PathUtils.Terms
{
   static class ValidName
   {
      /// <summary>
      /// Проверяет строку на наличие валидного имени элемента файловой системы.
      /// В случае удачи возвращает собственную длинну, обратно - (-1)
      /// </summary>
      /// <param name="p_ValidatorContext"></param>
      /// <returns></returns>
      public static int TryParse(PathValidatorContext p_ValidatorContext)
      {
         int i = 0;
         for (; i < Validator.MaxLongPath; i++)
         {
            int index = p_ValidatorContext.CurrentIndex + i;

            if (index >= p_ValidatorContext.ValidatedString.Length)
            {
               break;
            }

            if (p_ValidatorContext.ValidatedString[index].ContainedIn( Validator.SpecialChars ))
            {
               break;
            }
         }

         return i > 0 ? i : -1;
      }

      internal static bool ParseNext(PathValidatorContext p_ValidatorContext)
      {
         int length;
         if ((length = Root.TryParse( p_ValidatorContext )) > 0)
         {
            if (p_ValidatorContext.TryAddTerm( length ))
               if (p_ValidatorContext.TotalLength == p_ValidatorContext.ValidatedString.Length)
               {
                  return true;
               }
               return Root.ParseNext(p_ValidatorContext);
         }
         return length == 0;
      }
   }
}
