using DirectoriesComparator;
using DirectoriesComparator.CommandLine.Parser;

namespace TestProject1
{
   public static class TestConsts
   {
      public const string sourceDir1 = "c:\\source1\\";
      public const string targetDir1 = "c:\\target1\\";
      public const string sourceDir2 = "c:\\source2\\";
      public const string targetDir2 = "c:\\target2\\";
      public const string sourceDir3 = "c:\\source3\\";
      public const string targetDir3 = "c:\\target3\\";

      public const string arg0 = "D:\\WorkingArea\\VS_Projects[ DefaultDir ]\\DirectoriesComparator\\DirectoriesComparator\\bin\\Debug\\DirectoriesComparator.exe";
      public const string arg0Quoted = "\"" + arg0 + "\"";

      public const string param1 = "/C";
      public const string param2 = "/param2";

      const string ArgSource = CommandLineParser.SourcePrefix + TestConsts.sourceDir1;
      const string ArgTarget = CommandLineParser.TargetPrefix + TestConsts.targetDir1;

      public const string TypicalUsageArgs = TestConsts.arg0Quoted + " " + ArgSource + " " + ArgTarget;

   }
}