namespace DirectoriesComparator.CommandLine.Parser
{
   internal class ParsingContext
   {
      private readonly Parameters m_Parameters;

      public Parameters Parameters
      {
         get { return m_Parameters; }
      }

      public bool ApplicationFileSpecified { get; set; }


      public ParsingContext( Parameters p_Parameters )
      {
         m_Parameters = p_Parameters;
      }
   }
}