using System.IO;

namespace DirectoriesComparator.CommandLine.Parser.States
{
   internal class InitialState :AbstractState
   {
      private static AbstractState m_Item;

      public static AbstractState Item
      {
         get
         {
            if (m_Item == null)
            {
               m_Item = new InitialState();
            }
            return m_Item;
         }
      }

      public override AbstractState ProcessWord( string p_Word, ParsingContext p_ParsingContext )
      {
         var currentProcess = System.Diagnostics.Process.GetCurrentProcess();

         var appFileFullName = currentProcess.MainModule.FileName.ToLower();
         
         if ((appFileFullName == p_Word)|| (Path.GetFileName(appFileFullName)== p_Word))
         {
            if (p_ParsingContext.ApplicationFileSpecified)
            {
               return new ErrorState();
            }
            p_ParsingContext.ApplicationFileSpecified = true;
            return this;
         }

         if (p_Word.Equals( (CommandLineParser.SourcePrefix) ))
         {
            return ClauseSourceInitiated.Item;
         }

         if (p_Word.Equals( (CommandLineParser.TargetPrefix) ))
         {
            return ClauseTargetInitiated.Item;
         }

         return new ErrorState();
      }
   }
}