using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DirectoriesComparator.PathUtils.Terms
{
   static class Drive
   {
      public const int Length = 2;

      /// <summary>
      /// Проверяет строку на наличие имени диска, например "d:".
      /// В случае удачи возвращает собственную длинну, обратно - (-1)
      /// </summary>
      /// <param name="p_ValidatorContext"></param>
      /// <returns></returns>
      public static int TryParse(PathValidatorContext p_ValidatorContext)
      {
         if ((p_ValidatorContext.ValidatedString.Length + p_ValidatorContext.CurrentIndex + Length) < p_ValidatorContext.ValidatedString.Length)
         {
            return -1;
         }

         if (Char.IsLetter( p_ValidatorContext.ValidatedString, p_ValidatorContext.CurrentIndex ) && (p_ValidatorContext.ValidatedString[p_ValidatorContext.CurrentIndex + 1] == ':'))
         {
            return Length;
         }

         return -1;
      }

      internal static bool ParseNext(PathValidatorContext p_ValidatorContext)
      {
          if (p_ValidatorContext.CurrentIndex >= p_ValidatorContext.TotalLength)
              return true;

         int length = Root.TryParse(p_ValidatorContext);
         if (length > 0)
         {
            if (p_ValidatorContext.TryAddTerm( length ))
               if (p_ValidatorContext.TotalLength == p_ValidatorContext.ValidatedString.Length)
               {
                  return true;
               }
            return Root.ParseNext( p_ValidatorContext );
         }

         return length == 0;
      }
   }
}
