using System.Collections.Generic;
using System.Text;

namespace DirectoriesComparator.CommandLine
{
   public class ParametersSplitter
   {
      public static List<string> SplitString( string args, out bool quotesOpened )
      {
         quotesOpened = false;
         var slashPassed = false;
         var word = new StringBuilder();

         var words = new List<string>();

         foreach (var inputChar in args)
         {
            if (inputChar == '\"')
            {
               quotesOpened = !quotesOpened;

               AddWord(word, words);
               continue;
            }

            if (quotesOpened)
            {
               word.Append( inputChar );
               continue;
            }

            if (inputChar == '/')
            {
               slashPassed = true;

               AddWord( word, words );
               word.Append( inputChar );
               continue;
            }

            if ( slashPassed && (inputChar == ':') )
            {
               slashPassed = false;

               word.Append( inputChar );
               AddWord( word, words );
               continue;
            }

            if ((inputChar == ' ') || (inputChar == '\t'))
            {
               slashPassed = false;

               AddWord( word, words );
               continue;
            }

            word.Append( inputChar );
         }

         if (!quotesOpened)
            AddWord( word, words );
         return words;
      }

      private static void AddWord(StringBuilder word, List<string> words)
      {
         if (word.Length > 0)
         {
            words.Add(word.ToString());
            word.Clear();
         }
      }
   }
}