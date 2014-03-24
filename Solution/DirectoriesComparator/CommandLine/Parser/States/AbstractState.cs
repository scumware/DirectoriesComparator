namespace DirectoriesComparator.CommandLine.Parser.States
{
   internal abstract class AbstractState
   {
      //protected AbstractState( AbstractState p_ParentState )
      //{
      //   ParentState = p_ParentState;
      //}

      protected AbstractState()
      {
            
      }

      //public AbstractState ParentState { get; private set; }
      public abstract AbstractState ProcessWord(string p_Word, ParsingContext p_ParsingContext);
   }
}