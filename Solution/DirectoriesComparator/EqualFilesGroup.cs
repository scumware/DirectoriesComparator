using System.Collections.Generic;

namespace DirectoriesComparator
{
   public class EqualFilesGroup
   {
      public EqualFilesGroup(IEnumerable<File> p_SourceFilesGroup, IEnumerable<File> p_TargetFilesGroup)
      {
         SourceFiles = p_SourceFilesGroup;
         TargetFiles = p_TargetFilesGroup;
      }

      public IEnumerable<File> SourceFiles { get; set; }
      public IEnumerable<File> TargetFiles { get; set; }
   }
}
