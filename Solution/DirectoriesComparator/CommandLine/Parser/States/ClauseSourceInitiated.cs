namespace DirectoriesComparator.CommandLine.Parser.States
{
   internal class ClauseSourceInitiated :ClauseInitiated
   {
      private static AbstractState m_Item;

      public static AbstractState Item
      {
         get { return m_Item ?? (m_Item = new ClauseSourceInitiated()); }
      }

      protected ClauseSourceInitiated()
      {
         
      }

      protected override void PathRecognized(string p_Path, ParsingContext p_Context)
      {
         base.PathRecognized(p_Path, p_Context);
         p_Context.Parameters.SourcesList.Add( p_Path );
      }
   }
}