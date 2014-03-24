using System.Collections.Generic;


namespace DirectoriesComparator
{
   public class Parameters
   {
      public bool Empty { get; set; }
      public bool Incorrect { get; set; }
      /// <summary>
      /// Сообщение парсера
      /// </summary>
      public string AdditionalMessage { get; set; }
      public ICollection<string> SourcesList { get; private set; }
      public ICollection<string> TargetsList { get; private set; }

 
      public void Validate()
      {
         if (InvestigateJunctions && IgnoreJunctions)
         {
            Incorrect = true;
            string.Concat(AdditionalMessage,
                          "\n\r Params InvestigateJunctions and IgnoreJunctions can't be seted simultaneously");
         }
      }

      public Parameters()
      {
         Empty = true;
         Incorrect = false;
         SourcesList = new List<string>();
         TargetsList = new List<string>();
         InvestigateJunctions = false;
         ShowWarnings = false;
         IgnoreInaccessible = true;
      }
      
      //----------------------не реализовано
      public bool InvestigateJunctions { get; set; } //always false
      public bool IgnoreJunctions { get; set; } //always false
      public bool ShowWarnings { get; set; } //always false
      public bool IgnoreInaccessible { get; set; }//always true
   }
}
