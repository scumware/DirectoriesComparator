using System;

namespace DirectoriesComparator.CommandLine.Parser.States
{
   internal class ErrorState:AbstractState
   {
      public override AbstractState ProcessWord(string p_Word, ParsingContext p_ParsingContext)
      {
         throw new InvalidOperationException("После выявления ошибки, парсинг должен быть прекращён");
      }
   }
}