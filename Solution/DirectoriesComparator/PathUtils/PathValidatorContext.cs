using DirectoriesComparator.PathUtils.Terms;

namespace DirectoriesComparator.PathUtils
{
   public class PathValidatorContext
   {
      public PathValidatorContext( string p_String )
      {
         TotalLength = 0;
         CurrentIndex = 0;
         IsLongPath = false;
         ValidatedString = p_String;
      }


      public int TotalLength { get; private set; }
      public bool IsLongPath { get; set; }
      public int CurrentIndex { get; private set; }
      public string ValidatedString { get; private set; }

      public bool TryAddTerm( int p_Length )
      {
         var newLength = TotalLength + p_Length;
         bool result;
         if (IsLongPath)
         {
            result = Validator.MaxLongPath >= newLength;
         }
         else
         {
            result = Validator.MaxPath >= newLength;
         }
         if (result)
         {
            TotalLength += p_Length;
            CurrentIndex += p_Length;
         }

         return result;
      }
   }
}