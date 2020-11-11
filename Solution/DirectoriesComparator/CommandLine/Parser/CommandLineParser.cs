using DirectoriesComparator.CommandLine.Parser.States;

namespace DirectoriesComparator.CommandLine.Parser
{
   public static class CommandLineParser
   {
      public const string SourcePrefix = "/source:";
      public const string TargetPrefix = "/target:";
      public const string MinFileSizePrefix = "/minsize:";

        public static Parameters ParseCommandLine( string p_Args )
      {
         var result = new Parameters();

         if (string.IsNullOrEmpty( p_Args ))
         {
            //result.Empty = true; default value for that property
            return result;
         }

         string args = p_Args.Trim().ToLower();

         bool quotesOpened;
         var words = ParametersSplitter.SplitString(args, out quotesOpened);
         if (quotesOpened)
         {
            result.Empty = false;
            result.Incorrect = true;
            return result;
         }

         var parserContext = new ParsingContext( result );
         AbstractState currentState = InitialState.Item;
         foreach (var word in words)
         {
            currentState = currentState.ProcessWord( word, parserContext );

            if (currentState is ErrorState)
            {
               break;
            }
         }


         return result;
      }
   }
}