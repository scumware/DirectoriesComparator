using System;

namespace DirectoriesComparator.CommandLine.Parser.States
{
   internal abstract class ClauseInitiated :AbstractState
   {
      public override AbstractState ProcessWord(string p_Word, ParsingContext p_ParsingContext)
      {
         if (PathUtils.Validator.ValidatePath(p_Word))
         {
            PathRecognized(p_Word, p_ParsingContext);
            return this;
         }

         if (p_Word.Equals( (CommandLineParser.SourcePrefix) ))
         {
            if (this is ClauseSourceInitiated)
            {
               return this;
            }
            if (this is ClauseTargetInitiated)
            {
               return ClauseSourceInitiated.Item;
            }

            throw new NotImplementedException();
         }

         if (p_Word.Equals( (CommandLineParser.TargetPrefix) ))
         {
            if (this is ClauseTargetInitiated)
            {
               return this;
            }
            if (this is ClauseSourceInitiated)
            {
               return ClauseTargetInitiated.Item;
            }

            throw new NotImplementedException();
         }

         if (p_Word[0]=='/')
         {
            p_ParsingContext.Parameters.AdditionalMessage = string.Format("Опция {0} не опознана", p_Word);
         }
         else
         {
            p_ParsingContext.Parameters.AdditionalMessage = string.Format("Строка {0} не опознана как путь или оция", p_Word);
         }
         p_ParsingContext.Parameters.Incorrect = true;


         return new ErrorState();
      }

      protected virtual void PathRecognized(string p_Path, ParsingContext p_Context)
      {
         p_Context.Parameters.Empty = false;
      }
   }
}