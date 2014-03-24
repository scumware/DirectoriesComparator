using DirectoriesComparator.PathUtils.Terms;

namespace DirectoriesComparator.PathUtils
{
   public static class Validator
   {
      public const string SpecialChars = "<>:\"/\\|?*";
      public const int MaxLongPath = 32767;
      public const int MaxPath = 260;


      public static bool ValidatePath( string p_String )
      {
         if (p_String.Length <= MaxLongPath)
         {
            var pathValidatorContext = new PathValidatorContext( p_String );
            return ValidatePath( pathValidatorContext );
         }

         return false;
      }

      private static bool ValidatePath(PathValidatorContext pathValidatorContext)
      {
         int length;
         if ((length = LongPathPrefix.TryParse(pathValidatorContext)) > 0)
         {
            if (pathValidatorContext.TryAddTerm(length))
               return LongPathPrefix.ParseNext(pathValidatorContext);
            return false;
         }

         if ((length = UNCPrefix.TryParse(pathValidatorContext)) > 0)
         {
            if (pathValidatorContext.TryAddTerm(length))
               return UNCPrefix.ParseNext(pathValidatorContext);
            return false;
         }

         if ((length = Root.TryParse(pathValidatorContext)) > 0)
         {
            if (pathValidatorContext.TryAddTerm(length))
               return Root.ParseNext(pathValidatorContext);
            return false;
         }

         if ((length = Drive.TryParse(pathValidatorContext)) > 0)
         {
            if (pathValidatorContext.TryAddTerm(length))
               return Drive.ParseNext(pathValidatorContext);
            return false;
         }

         if ((length = ValidName.TryParse(pathValidatorContext)) > 0)
         {
            if (pathValidatorContext.TryAddTerm(length))
               return ValidName.ParseNext(pathValidatorContext);
            return false;
         }

         return false;
      }
   }
}
